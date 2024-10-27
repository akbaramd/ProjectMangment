using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using PMS.Application.Interfaces;
using PMS.Application.UseCases.Auth.Exceptions;
using PMS.Application.UseCases.Auth.Models;
using PMS.Application.UseCases.Tenant.Exceptions;
using PMS.Application.UseCases.User.Exceptions;
using PMS.Application.UseCases.User.Models;
using PMS.Domain.BoundedContexts.TenantManagement.Repositories;
using PMS.Domain.BoundedContexts.UserManagment;
using PMS.Domain.BoundedContexts.UserManagment.Repositories;
// Custom exceptions
using UnauthorizedAccessException = System.UnauthorizedAccessException;

namespace PMS.Application.UseCases.Auth
{
    public class AuthService
        : IAuthService
    {
        
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IJwtService _jwtService;
        private readonly ITenantRepository _tenantRepository;
        private readonly ITenantMemberRepository _tenantMemberRepository;
        private readonly IUserRepository _userRepository;
        private readonly IPermissionRepository _permissionRepository;
        private readonly IRoleRepository _roleRepository;

        public AuthService(
            UserManager<ApplicationUser> userManager,
            IJwtService jwtService,
            ITenantRepository tenantRepository,
            IPermissionRepository permissionRepository,
            IRoleRepository roleRepository, IUserRepository userRepository, ITenantMemberRepository tenantMemberRepository)
        {
            _userManager = userManager;
            _jwtService = jwtService;
            _tenantRepository = tenantRepository;
            _permissionRepository = permissionRepository;
            _roleRepository = roleRepository;
            this._userRepository = userRepository;
            _tenantMemberRepository = tenantMemberRepository;
        }
        
        // 1. Send invitation to phone number
        // Send invitation using SMS

        // 3. Register the user if they are not already registered
        public async Task RegisterAsync(AuthRegisterDto authRegisterDto)
        {
            var existingUser = await _userRepository.GetUserByPhoneNumberAsync(authRegisterDto.PhoneNumber);
            if (existingUser != null)
            {
                throw new UserAlreadyRegisteredException("User already exists.");
            }

            var user = new ApplicationUser(authRegisterDto.FullName, authRegisterDto.PhoneNumber, authRegisterDto.Email, deletable: false);
            user.GenerateRefreshToken();
            var result = await _userManager.CreateAsync(user, authRegisterDto.Password);
            if (!result.Succeeded)
            {
                throw new RegistrationFailedException("Registration failed.");
            }

            // Add user to the "User" role after successful registration
            // await _userManager.AddToRoleAsync(user, "User");
        }

        public async Task<AuthJwtDto> LoginAsync(AuthLoginDto authLoginDto, string tenantId)
        {
            var tenant = await _tenantRepository.GetTenantBySubdomainAsync(tenantId);
            if (tenant == null)
            {
                throw new TenantNotFoundException();
            }

            var user = await _userRepository.GetUserByPhoneNumberAsync(authLoginDto.PhoneNumber);  // Use phone number for login
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
            if (!await _userManager.CheckPasswordAsync(user, authLoginDto.Password))
            {
                user.RecordFailedLogin();
                await _userManager.UpdateAsync(user);  // Update failed attempts and lockout state in DB
                throw new InvalidCredentialsException();  // Same exception for consistency
            }

            // If login is successful, reset failed login attempts
            user.ResetFailedLoginAttempts();

            var isUserInTenant = await _tenantMemberRepository.IsUserInTenantAsync(user.Id, tenant.Id);
            if (!isUserInTenant)
            {
                throw new UnauthorizedAccessException("User is not part of the tenantEntity.");
            }

            var roles = await _userManager.GetRolesAsync(user);
            var token = _jwtService.GenerateToken(user.Id.ToString(), roles);

            user.GenerateRefreshToken();
            await _userManager.UpdateAsync(user);

            return new AuthJwtDto
            {
                AccessToken = token,
                RefreshToken = user.RefreshToken,
                UserId = user.Id.ToString()
            };
        }

        public async Task<AuthJwtDto> RefreshTokenAsync(LoginWithRefreshTokenDto refreshTokenDto)
        {
            // Find the user by the refresh token
            var user = await _userManager.Users.SingleOrDefaultAsync(u => u.RefreshToken == refreshTokenDto.RefreshToken);
            if (user == null || user.RefreshTokenExpiryTime <= DateTime.UtcNow)
            {
                throw new InvalidTokenException();  // Custom exception for invalid or expired token
            }

            // Generate new tokens
            var roles = await _userManager.GetRolesAsync(user);
            var newToken = _jwtService.GenerateToken(user.Id.ToString(), roles);

            // Update the user with the new refresh token and expiry
            user.GenerateRefreshToken();
            await _userManager.UpdateAsync(user);

            return new AuthJwtDto
            {
                AccessToken = newToken,
                RefreshToken = user.RefreshToken,
                UserId = user.Id.ToString()
            };
        }
        
        public async Task<UserProfileDto> GetUserProfileAsync(Guid userId, string tenantId)
        {
            // Retrieve the user by ID
            var user = await _userRepository.GetByIdAsync(userId);
            if (user == null)
            {
                throw new UserNotFoundException("User not found.");
            }

            // Get the tenantEntity
            var tenant = await _tenantRepository.GetTenantBySubdomainAsync(tenantId);
            if (tenant == null)
            {
                throw new TenantNotFoundException("Tenant not found.");
            }

            // Check if the user is part of the tenantEntity
            var tenantMember = await _tenantMemberRepository.GetUserTenantByUserIdAndTenantIdAsync(user.Id, tenant.Id);
            if (tenantMember == null)
            {
                throw new UnauthorizedAccessException("User is not part of this tenantEntity.");
            }

            // Retrieve the user's roles for this tenantEntity
            var roles = await _userManager.GetRolesAsync(user);
    
            // Get the permissions for the roles in this tenantEntity
            var rolePermissions = new List<string>();
            foreach (var roleName in roles)
            {
                var role =  tenantMember.Roles
                    .FirstOrDefault(r => r.Key == roleName && (r.TenantId == tenant.Id || r.TenantId == null));

                if (role != null)
                {
                    var permissions = role.Permissions.Select(p => p.Key).ToList();
                    rolePermissions.AddRange(permissions);
                }
            }

            // Map the user entity to UserProfileDto
            var userProfile = new UserProfileDto
            {
                Id = user.Id,
                FullName = user.FullName,
                PhoneNumber = user.PhoneNumber,
                Email = user.Email,
            };

            return userProfile;
        }

        
       

    }
    
}
