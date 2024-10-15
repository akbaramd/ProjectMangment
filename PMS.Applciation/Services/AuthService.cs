using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using PMS.Application.DTOs;
using PMS.Application.Interfaces;
using PMS.Domain.Entities;
using PMS.Domain.Repositories;
using ProjectName.Application.Interfaces;

namespace PMS.Infrastructure.Services
{
    public class AuthService : IAuthService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IJwtService _jwtService;
        private readonly IInvitationRepository _invitationRepository;
        private readonly ITenantRepository _tenantRepository;
        private readonly IUserTenantRepository _userTenantRepository;

        public AuthService(
            UserManager<ApplicationUser> userManager,
            IJwtService jwtService,
            IInvitationRepository invitationRepository,
            ITenantRepository tenantRepository,
            IUserTenantRepository userTenantRepository)
        {
            _userManager = userManager;
            _jwtService = jwtService;
            _invitationRepository = invitationRepository;
            _tenantRepository = tenantRepository;
            _userTenantRepository = userTenantRepository;
        }

        public async Task RegisterAsync(RegisterDto registerDto)
        {
            var existingUser = await _userManager.FindByEmailAsync(registerDto.Email);
            if (existingUser != null)
            {
                throw new Exception("User already registered.");
            }

            var user = new ApplicationUser(registerDto.FullName, registerDto.PhoneNumber, registerDto.Email, deletable: false);
            var result = await _userManager.CreateAsync(user, registerDto.Password);
            if (result.Succeeded)
            {
                await _userManager.AddToRoleAsync(user, "User");
            }
            else
            {
                throw new Exception("Registration failed.");
            }
        }

        public async Task SendInvitationAsync(SendInvitationDto sendInvitationDto,string tenantId)
        {
            var tenant = await _tenantRepository.GetTenantBySubdomainAsync(tenantId);
            if (tenant == null)
            {
                throw new Exception("Tenant not found.");
            }

            var invitation = new Invitation(sendInvitationDto.Email, tenant);
            await _invitationRepository.AddAsync(invitation);

            // Logic to send an invitation email with the token
            // This could involve generating a link with the invitation token
        }

        public async Task<InvitationDetailsDto> GetInvitationDetailsAsync(Guid invitationToken)
        {
            var invitation = await _invitationRepository.GetByIdAsync(invitationToken);
            if (invitation == null)
                throw new Exception("Invalid invitation token.");

            var userExists = await _userManager.FindByEmailAsync(invitation.Email) != null;

            return new InvitationDetailsDto
            {
                Email = invitation.Email,
                TenantId = invitation.TenantId.ToString(),
                UserExists = userExists
            };
        }

        public async Task AcceptInvitationAsync(AcceptInvitationDto acceptInvitationDto)
        {
            var invitation = await _invitationRepository.GetByIdAsync(acceptInvitationDto.InvitationToken);
            if (invitation == null)
                throw new Exception("Invalid invitation token.");

            var user = await _userManager.FindByIdAsync(acceptInvitationDto.UserId);
            if (user == null)
                throw new Exception("User not found.");

            var tenant = await _tenantRepository.GetByIdAsync(invitation.TenantId);
            if (tenant == null)
                throw new Exception("Tenant not found.");

            await _userTenantRepository.AddAsync(new UserTenant(user, tenant, UserTenantRole.Employer));
            await _invitationRepository.DeleteAsync(invitation);
        }

        public async Task<AuthResponseDto> LoginAsync(LoginDto loginDto, string tenantId)
        {
            var tenant = await _tenantRepository.GetTenantBySubdomainAsync(tenantId);
            if (tenant == null)
            {
                throw new Exception("Tenant not found.");
            }

            var user = await _userManager.FindByEmailAsync(loginDto.Email);
            if (user != null && await _userManager.CheckPasswordAsync(user, loginDto.Password))
            {
                var isUserInTenant = await _userTenantRepository.IsUserInTenantAsync(user.Id, tenant.Id);
                if (!isUserInTenant)
                    throw new Exception("User is not part of the tenant.");

                var roles = await _userManager.GetRolesAsync(user);
                var token = _jwtService.GenerateToken(user.Id.ToString(), roles);
                return new AuthResponseDto { Token = token, UserId = user.Id.ToString() };
            }
            throw new Exception("Invalid login attempt.");
        }
    }
}