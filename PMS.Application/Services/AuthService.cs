using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using PMS.Application.DTOs;
using PMS.Application.Interfaces;
using PMS.Application.Exceptions;  // Custom exceptions
using PMS.Domain.Entities;
using PMS.Domain.Repositories;
using System;
using UnauthorizedAccessException = System.UnauthorizedAccessException;

namespace PMS.Application.Services
{
    public class AuthService(
        UserManager<ApplicationUser> userManager,
        IJwtService jwtService,
        IInvitationRepository invitationRepository,
        ITenantRepository tenantRepository,
        ISmsService smsService,
        ITenantMemberRepository tenantMemberRepository,
        IUserRepository userRepository)
        : IAuthService
    {
        // 1. Send invitation to phone number
        // Send invitation using SMS

        // 3. Register the user if they are not already registered
        public async Task RegisterAsync(RegisterDto registerDto)
        {
            var existingUser = await userRepository.GetUserByPhoneNumberAsync(registerDto.PhoneNumber);
            if (existingUser != null)
            {
                throw new UserAlreadyRegisteredException("User already exists.");
            }

            var user = new ApplicationUser(registerDto.FullName, registerDto.PhoneNumber, registerDto.Email, deletable: false);
            user.GenerateRefreshToken();
            var result = await userManager.CreateAsync(user, registerDto.Password);
            if (!result.Succeeded)
            {
                throw new RegistrationFailedException("Registration failed.");
            }

            // Add user to the "User" role after successful registration
            await userManager.AddToRoleAsync(user, "User");
        }

        public async Task<AuthResponseDto> LoginAsync(LoginDto loginDto, string tenantId)
        {
            var tenant = await tenantRepository.GetTenantBySubdomainAsync(tenantId);
            if (tenant == null)
            {
                throw new TenantNotFoundException();
            }

            var user = await userRepository.GetUserByPhoneNumberAsync(loginDto.PhoneNumber);  // Use phone number for login
            if (user == null)
            {
                throw new InvalidCredentialsException();  // Custom exception for invalid login attempt
            }

            // Check if the user is locked out
            if (user.IsLockedOut())
            {
                var lockoutMinutes = Math.Pow(2, user.FailedLoginAttempts / 3 - 1);
                throw new AccountLockedException($"User is locked out. Try again in {lockoutMinutes} minutes.");
            }

            // Validate password
            if (!await userManager.CheckPasswordAsync(user, loginDto.Password))
            {
                user.RecordFailedLogin();
                await userManager.UpdateAsync(user);  // Update failed attempts and lockout state in DB
                throw new InvalidCredentialsException();  // Same exception for consistency
            }

            // If login is successful, reset failed login attempts
            user.ResetFailedLoginAttempts();

            var isUserInTenant = await tenantMemberRepository.IsUserInTenantAsync(user.Id, tenant.Id);
            if (!isUserInTenant)
            {
                throw new UnauthorizedAccessException("User is not part of the tenant.");
            }

            var roles = await userManager.GetRolesAsync(user);
            var token = jwtService.GenerateToken(user.Id.ToString(), roles);

            user.GenerateRefreshToken();
            await userManager.UpdateAsync(user);

            return new AuthResponseDto
            {
                AccessToken = token,
                RefreshToken = user.RefreshToken,
                UserId = user.Id.ToString()
            };
        }

        public async Task<AuthResponseDto> RefreshTokenAsync(LoginWithRefreshTokenDto refreshTokenDto)
        {
            // Find the user by the refresh token
            var user = await userManager.Users.SingleOrDefaultAsync(u => u.RefreshToken == refreshTokenDto.RefreshToken);
            if (user == null || user.RefreshTokenExpiryTime <= DateTime.UtcNow)
            {
                throw new InvalidTokenException();  // Custom exception for invalid or expired token
            }

            // Generate new tokens
            var roles = await userManager.GetRolesAsync(user);
            var newToken = jwtService.GenerateToken(user.Id.ToString(), roles);

            // Update the user with the new refresh token and expiry
            user.GenerateRefreshToken();
            await userManager.UpdateAsync(user);

            return new AuthResponseDto
            {
                AccessToken = newToken,
                RefreshToken = user.RefreshToken,
                UserId = user.Id.ToString()
            };
        }
        
        public async Task<UserProfileDto> GetUserProfileAsync(Guid userId)
        {
            // Retrieve the user by ID
            var user = await userRepository.GetByIdAsync(userId);
            if (user == null)
            {
                throw new UserNotFoundException("User not found.");
            }

            // Map the user entity to UserProfileDto
            var userProfile = new UserProfileDto
            {
                Id = user.Id,
                FullName = user.FullName,
                PhoneNumber = user.PhoneNumber,
                Email = user.Email,
                // Map other fields as needed
            };

            return userProfile;
        }
    }
}
