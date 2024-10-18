using AutoMapper;
using PMS.Application.DTOs;
using PMS.Application.Exceptions;
using PMS.Application.Interfaces;
using PMS.Domain.Entities;
using PMS.Domain.Repositories;
using UnauthorizedAccessException = PMS.Application.Exceptions.UnauthorizedAccessException;

namespace PMS.Application.Services
{
    public class TenantService : ITenantService
    {
        private readonly ITenantRepository _tenantRepository;
        private readonly ITenantMemberRepository _tenantMemberRepository;
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;

        public TenantService(
            ITenantRepository tenantRepository,
            ITenantMemberRepository tenantMemberRepository,
            IUserRepository userRepository,
            IMapper mapper)
        {
            _tenantRepository = tenantRepository;
            _tenantMemberRepository = tenantMemberRepository;
            _userRepository = userRepository;
            _mapper = mapper;
        }

        // Get tenant info with members using subdomain
        public async Task<TenantDto> GetTenantInfoAsync(string tenantSubdomain, Guid userId)
        {
            // Retrieve tenant by subdomain
            var tenant = await _tenantRepository.GetTenantBySubdomainAsync(tenantSubdomain);
            if (tenant == null)
            {
                throw new TenantNotFoundException("Tenant not found.");
            }

            // Check if user is part of the tenant
            var tenantMember = await _tenantMemberRepository.GetUserTenantByUserIdAndTenantIdAsync(userId, tenant.Id);
            if (tenantMember == null)
            {
                throw new UnauthorizedException("User is not a member of this tenant.");
            }

            // Map tenant entity to TenantDto
            var tenantDto = _mapper.Map<TenantDto>(tenant);
            tenantDto.CurrentUserRole = tenantMember.MemberRole; // Manually set current user's role

            // Only Owner, Manager, and Administrator can view members
            if (tenantMember.MemberRole == TenantMemberRole.Owner ||
                tenantMember.MemberRole == TenantMemberRole.Manager ||
                tenantMember.MemberRole == TenantMemberRole.Administrator)
            {
                var members = await _tenantMemberRepository.GetMembersByTenantIdAsync(tenant.Id);
                tenantDto.Members = _mapper.Map<List<TenantMemberDto>>(members); // Map members using AutoMapper
            }
            
            return tenantDto;
        }

        // Remove a member from tenant using subdomain
        // Remove a member from tenant using subdomain
        public async Task RemoveMemberAsync(string tenantSubdomain, Guid userId, Guid memberToRemoveId)
        {
            // Retrieve tenant by subdomain
            var tenant = await _tenantRepository.GetTenantBySubdomainAsync(tenantSubdomain);
            if (tenant == null)
            {
                throw new TenantNotFoundException("Tenant not found.");
            }

            // Check if the current user has the permission to remove members
            var tenantMember = await _tenantMemberRepository.GetUserTenantByUserIdAndTenantIdAsync(userId, tenant.Id);
            if (tenantMember == null || 
                !(tenantMember.MemberRole == TenantMemberRole.Owner || 
                  tenantMember.MemberRole == TenantMemberRole.Manager ||
                  tenantMember.MemberRole == TenantMemberRole.Administrator))
            {
                throw new UnauthorizedAccessException("You do not have permission to remove a member.");
            }

            // Fetch the member to be removed
            var memberToRemove = await _tenantMemberRepository.GetUserTenantByUserIdAndTenantIdAsync(memberToRemoveId, tenant.Id);
            if (memberToRemove == null)
            {
                throw new MemberNotFoundException("Member not found in tenant.");
            }

            // Prevent removal of tenant owner
            if (memberToRemove.MemberRole == TenantMemberRole.Owner)
            {
                throw new InvalidOperationException("The tenant owner cannot be removed.");
            }

            // Proceed with member removal
            await _tenantMemberRepository.DeleteAsync(memberToRemove);
        }

        // Update a member's role in the tenant using subdomain
        public async Task UpdateMemberRoleAsync(string tenantSubdomain, Guid userId, Guid memberToUpdateId, TenantMemberRole newRole)
        {
            // Retrieve tenant by subdomain
            var tenant = await _tenantRepository.GetTenantBySubdomainAsync(tenantSubdomain);
            if (tenant == null)
            {
                throw new TenantNotFoundException("Tenant not found.");
            }

            var tenantMember = await _tenantMemberRepository.GetUserTenantByUserIdAndTenantIdAsync(userId, tenant.Id);
            if (tenantMember == null ||
                !(tenantMember.MemberRole == TenantMemberRole.Owner || 
                  tenantMember.MemberRole == TenantMemberRole.Manager ||
                  tenantMember.MemberRole == TenantMemberRole.Administrator))
            {
                throw new UnauthorizedAccessException("You do not have permission to update member roles.");
            }

            var memberToUpdate = await _tenantMemberRepository.GetUserTenantByUserIdAndTenantIdAsync(memberToUpdateId, tenant.Id);
            if (memberToUpdate == null)
            {
                throw new MemberNotFoundException("Member not found in tenant.");
            }

            memberToUpdate.ChangeRole(newRole);
            await _tenantMemberRepository.UpdateAsync(memberToUpdate);
        }
    }
}
