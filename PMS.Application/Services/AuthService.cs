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
    public class AuthService
        : IAuthService
    {
        
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<ApplicationRole> _roleManager;
        private readonly IJwtService _jwtService;
        private readonly ITenantRepository _tenantRepository;
        private readonly ITenantMemberRepository _tenantMemberRepository;
        private readonly IUserRepository _userRepository;
        private readonly IPermissionRepository _permissionRepository;
        private readonly IRoleRepository _roleRepository;

        public AuthService(
            UserManager<ApplicationUser> userManager,
            RoleManager<ApplicationRole> roleManager,
            IJwtService jwtService,
            ITenantRepository tenantRepository,
            IPermissionRepository permissionRepository,
            IRoleRepository roleRepository, IUserRepository userRepository, ITenantMemberRepository tenantMemberRepository)
        {
            _userManager = userManager;
            this._roleManager = roleManager;
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
        public async Task RegisterAsync(RegisterDto registerDto)
        {
            var existingUser = await _userRepository.GetUserByPhoneNumberAsync(registerDto.PhoneNumber);
            if (existingUser != null)
            {
                throw new UserAlreadyRegisteredException("User already exists.");
            }

            var user = new ApplicationUser(registerDto.FullName, registerDto.PhoneNumber, registerDto.Email, deletable: false);
            user.GenerateRefreshToken();
            var result = await _userManager.CreateAsync(user, registerDto.Password);
            if (!result.Succeeded)
            {
                throw new RegistrationFailedException("Registration failed.");
            }

            // Add user to the "User" role after successful registration
            await _userManager.AddToRoleAsync(user, "User");
        }

        public async Task<AuthResponseDto> LoginAsync(LoginDto loginDto, string tenantId)
        {
            var tenant = await _tenantRepository.GetTenantBySubdomainAsync(tenantId);
            if (tenant == null)
            {
                throw new TenantNotFoundException();
            }

            var user = await _userRepository.GetUserByPhoneNumberAsync(loginDto.PhoneNumber);  // Use phone number for login
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
            if (!await _userManager.CheckPasswordAsync(user, loginDto.Password))
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
                throw new UnauthorizedAccessException("User is not part of the tenant.");
            }

            var roles = await _userManager.GetRolesAsync(user);
            var token = _jwtService.GenerateToken(user.Id.ToString(), roles);

            user.GenerateRefreshToken();
            await _userManager.UpdateAsync(user);

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

            return new AuthResponseDto
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

            // Get the tenant
            var tenant = await _tenantRepository.GetTenantBySubdomainAsync(tenantId);
            if (tenant == null)
            {
                throw new TenantNotFoundException("Tenant not found.");
            }

            // Check if the user is part of the tenant
            var tenantMember = await _tenantMemberRepository.IsUserInTenantAsync(user.Id, tenant.Id);
            if (!tenantMember)
            {
                throw new UnauthorizedAccessException("User is not part of this tenant.");
            }

            // Retrieve the user's roles for this tenant
            var roles = await _userManager.GetRolesAsync(user);
    
            // Get the permissions for the roles in this tenant
            var rolePermissions = new List<string>();
            foreach (var roleName in roles)
            {
                var role = await _roleManager.Roles
                    .Include(r => r.Permissions)
                    .FirstOrDefaultAsync(r => r.Name == roleName && (r.TenantId == tenant.Id || r.TenantId == null));

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
                Roles = roles.ToList(),  // List of roles assigned to the user in this tenant
                Permissions = rolePermissions.Distinct().ToList()  // List of permissions assigned to the user
            };

            return userProfile;
        }

        
         public async Task<List<RoleWithPermissionsDto>> GetRolesForTenantAsync(string tenantId)
        {
            var tenant = await _tenantRepository.GetTenantBySubdomainAsync(tenantId);
            if (tenant == null) throw new TenantNotFoundException();

            var roles =  _roleManager.Roles.Where(x=>x.TenantId == tenant.Id);

            var roleDtos = roles.Select(r => new RoleWithPermissionsDto
            {
                RoleId = r.Id,
                RoleName = r.Name,
                Permissions = r.Permissions.Select(p => new PermissionDto
                {
                    Key = p.Key,
                    Name = p.Name
                }).ToList()
            }).ToList();

            return roleDtos;
        }

        // Add a new role with permissions for a tenant
        public async Task AddRoleAsync(string tenantId, CreateRoleDto createRoleDto)
        {
            var tenant = await _tenantRepository.GetTenantBySubdomainAsync(tenantId);
            if (tenant == null) throw new TenantNotFoundException();

            // Create new role entity
            var role = new ApplicationRole(createRoleDto.RoleName, tenantId: tenant.Id, deletable: true,
                isSystemRole: false);

            // Add the role using the repository
            await _roleRepository.AddAsync(role);

            // Add permissions to the role
            foreach (var permissionKey in createRoleDto.PermissionKeys)
            {
                var permission = await _permissionRepository.GetPermissionByKeyAsync(permissionKey);
                if (permission == null)
                {
                    throw new Exception(permissionKey);
                }

                role.Permissions.Add(permission);
            }

            // Save changes to the role and its permissions
            await _roleRepository.UpdateAsync(role);
        }

        // Update role permissions and name for a tenant
        public async Task UpdateRoleAsync(string tenantId, Guid roleId, UpdateRoleDto updateRoleDto)
        {
            var tenant = await _tenantRepository.GetTenantBySubdomainAsync(tenantId);
            if (tenant == null) throw new TenantNotFoundException();
            
            var role =  await _roleManager.Roles.Include(c=>c.Permissions).FirstOrDefaultAsync(c=>c.Id == roleId);
            if (role == null || role.TenantId == null || role.TenantId != tenant.Id)
            {
                throw new Exception();
            }

            // Update role name
            role.Name = updateRoleDto.RoleName;

            // Clear existing permissions
            role.Permissions.Clear();

            // Add updated permissions
            foreach (var permissionKey in updateRoleDto.PermissionKeys)
            {
                var permission = await _permissionRepository.GetPermissionByKeyAsync(permissionKey);
                if (permission == null)
                {
                    throw new Exception(permissionKey);
                }

                role.Permissions.Add(permission);
            }

            // Update role in the repository
            await _roleRepository.UpdateAsync(role);
        }

        // Delete a role if no users are assigned
        public async Task DeleteRoleAsync(string tenantId, Guid roleId)
        {
            var tenant = await _tenantRepository.GetTenantBySubdomainAsync(tenantId);
            if (tenant == null) throw new TenantNotFoundException();
            
            var role =  await _roleManager.Roles.Include(c=>c.Permissions).FirstOrDefaultAsync(c=>c.Id == roleId);
            if (role == null || role.TenantId == null || role.TenantId != tenant.Id)
            {
                throw new Exception();
            }

            // Delete the role from the repository
            await _roleRepository.DeleteAsync(role);
        }

        // Fetch permission groups and their permissions
        public async Task<List<PermissionGroupDto>> GetPermissionGroupsAsync()
        {
            var permissionGroups = await _permissionRepository.GetPermissionGroupsAsync();

            var permissionGroupDtos = permissionGroups.Select(pg => new PermissionGroupDto
            {
                Key = pg.Key,
                Name = pg.Name,
                Permissions = pg.Permissions.Select(p => new PermissionDto
                {
                    Key = p.Key,
                    Name = p.Name
                }).ToList()
            }).ToList();

            return permissionGroupDtos;
        }

    }
    
}
