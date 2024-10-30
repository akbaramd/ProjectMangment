using Bonyan.MultiTenant;
using Bonyan.TenantManagement.Domain;
using PMS.Application.Interfaces;
using PMS.Application.UseCases.Auth.Exceptions;
using PMS.Application.UseCases.Auth.Models;
using PMS.Application.UseCases.Tenants.Exceptions;
using PMS.Application.UseCases.User.Exceptions;
using PMS.Application.UseCases.User.Models;
using PMS.Domain.BoundedContexts.TenantManagement.Repositories;
using PMS.Domain.BoundedContexts.UserManagment;
using PMS.Domain.BoundedContexts.UserManagment.Repositories;
// Custom exceptions
using UnauthorizedAccessException = System.UnauthorizedAccessException;

namespace PMS.Application.UseCases.Auth
{
    public class AuthService : IAuthService
    {
        private readonly IJwtService _jwtService;
        private readonly ITenantRepository _tenantRepository;
        private readonly ITenantMemberRepository _tenantMemberRepository;
        private readonly IUserRepository _userRepository;
        private readonly IPermissionRepository _permissionRepository;
        private readonly IRoleRepository _roleRepository;
        private readonly ICurrentTenant _currentTenant;

        public AuthService(
            IJwtService jwtService,
            ITenantRepository tenantRepository,
            IPermissionRepository permissionRepository,
            IRoleRepository roleRepository,
            IUserRepository userRepository,
            ITenantMemberRepository tenantMemberRepository, ICurrentTenant currentTenant)
        {
            _jwtService = jwtService;
            _tenantRepository = tenantRepository;
            _permissionRepository = permissionRepository;
            _roleRepository = roleRepository;
            _userRepository = userRepository;
            _tenantMemberRepository = tenantMemberRepository;
            _currentTenant = currentTenant;
        }
        
        public async Task RegisterAsync(AuthRegisterDto authRegisterDto)
        {
            var existingUser = await _userRepository.GetUserByPhoneNumberAsync(authRegisterDto.PhoneNumber);
            if (existingUser != null)
            {
                throw new UserAlreadyRegisteredException("UserEntity already exists.");
            }

            var user = new UserEntity(authRegisterDto.FullName, authRegisterDto.PhoneNumber, authRegisterDto.Email, deletable: false);
            user.GenerateRefreshToken();
            var creationResult = await _userRepository.AddAsync(user);

            // Add userEntity to the "UserEntity" role after successful registration
            // await _userRepository.AddToRoleAsync(userEntity, "UserEntity");
        }

        public async Task<AuthJwtDto> LoginAsync(AuthLoginDto authLoginDto)
        {
            var tenant = await _tenantRepository.FindOneAsync(x => x.Key == _currentTenant.Name);
            if (tenant == null)
            {
                throw new TenantNotFoundException();
            }

            var user = await _userRepository.GetUserByPhoneNumberAsync(authLoginDto.PhoneNumber);
            if (user == null)
            {
                throw new InvalidCredentialsException();
            }

            // Check if the userEntity is locked out
            if (user.IsLockedOut())
            {
                var lockoutMinutes = Math.Pow(2, user.FailedLoginAttempts / 3 - 1);
                throw new AccountLockedException($"UserEntity is locked out. Try again in {lockoutMinutes} minutes.");
            }

            // Validate password
            if (!await _userRepository.CheckPasswordAsync(user, authLoginDto.Password))
            {
                user.RecordFailedLogin();
                await _userRepository.UpdateAsync(user);  // Update failed attempts and lockout state in DB
                throw new InvalidCredentialsException();
            }

            // If login is successful, reset failed login attempts
            user.ResetFailedLoginAttempts();
            await _userRepository.UpdateAsync(user);

            var isUserInTenant = await _tenantMemberRepository.IsUserInTenantAsync(user.Id, tenant.Id);
            if (!isUserInTenant)
            {
                throw new UnauthorizedAccessException("UserEntity is not part of the tenantEntity.");
            }

            var roles = await _userRepository.GetUserRolesAsync(user.Id);
            var token = _jwtService.GenerateToken(user.Id.ToString(), roles);

            user.GenerateRefreshToken();
            await _userRepository.UpdateAsync(user);

            return new AuthJwtDto
            {
                AccessToken = token,
                RefreshToken = user.RefreshToken,
                UserId = user.Id.ToString()
            };
        }

        public async Task<AuthJwtDto> RefreshTokenAsync(LoginWithRefreshTokenDto refreshTokenDto)
        {
            var user = await _userRepository.FindOneAsync(x=>x.RefreshToken.Equals(refreshTokenDto.RefreshToken));
            if (user == null || user.RefreshTokenExpiryTime <= DateTime.UtcNow)
            {
                throw new InvalidTokenException();
            }

            var roles = await _userRepository.GetUserRolesAsync(user.Id);
            var newToken = _jwtService.GenerateToken(user.Id.ToString(), roles);

            user.GenerateRefreshToken();
            await _userRepository.UpdateAsync(user);

            return new AuthJwtDto
            {
                AccessToken = newToken,
                RefreshToken = user.RefreshToken,
                UserId = user.Id.ToString()
            };
        }

        public async Task<UserProfileDto> GetUserProfileAsync(Guid userId)
        {
            var user = await _userRepository.GetByIdAsync(userId);
            if (user == null)
            {
                throw new UserNotFoundException("UserEntity not found.");
            }

            var tenant = await _tenantRepository.FindOneAsync(x => x.Key == _currentTenant.Name);
            if (tenant == null)
            {
                throw new TenantNotFoundException("Tenants not found.");
            }

            var tenantMember = await _tenantMemberRepository.GetUserTenantByUserIdAndTenantIdAsync(user.Id, tenant.Id);
            if (tenantMember == null)
            {
                throw new UnauthorizedAccessException("UserEntity is not part of this tenantEntity.");
            }

            var roles = await _userRepository.GetUserRolesAsync(user.Id);
            var rolePermissions = new List<string>();
            foreach (var roleName in roles)
            {
                var role = tenantMember.Roles.FirstOrDefault(r => r.Key == roleName && (r.TenantId == tenant.Id.Value || r.TenantId == null));

                if (role != null)
                {
                    var permissions = role.Permissions.Select(p => p.Key).ToList();
                    rolePermissions.AddRange(permissions);
                }
            }

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
