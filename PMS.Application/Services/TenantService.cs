using AutoMapper;
using PMS.Application.DTOs;
using PMS.Application.Exceptions;
using PMS.Application.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PMS.Application.UseCases.Sprints.Specs;
using PMS.Application.UseCases.TenantMembers.Specs;
using PMS.Domain.BoundedContexts.TenantManagment.Repositories;
using PMS.Domain.BoundedContexts.UserManagment.Repositories;
using SharedKernel.Model;
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

        // Get tenantEntity info with members using subdomain
        public async Task<TenantDto> GetTenantInfoAsync(string tenantSubdomain, Guid userId)
        {
            // Retrieve tenantEntity by subdomain
            var tenant = await _tenantRepository.GetTenantBySubdomainAsync(tenantSubdomain);
            if (tenant == null)
            {
                throw new TenantNotFoundException("Tenant not found.");
            }

            // Check if user is part of the tenantEntity
            var tenantMember = await _tenantMemberRepository.GetUserTenantByUserIdAndTenantIdAsync(userId, tenant.Id);
            if (tenantMember == null)
            {
                throw new UnauthorizedException("User is not a member of this tenantEntity.");
            }

            // Check if the tenantEntity member has 'tenant:read' permission
            if (!tenantMember.HasPermission("tenant:read"))
            {
                throw new UnauthorizedAccessException("User does not have permission to view tenantEntity details.");
            }

            // Map tenantEntity entity to TenantDto
            var tenantDto = _mapper.Map<TenantDto>(tenant);

            // Check if the member has 'member:read' permission to view members
            if (tenantMember.HasPermission("member:read"))
            {
                var members = await _tenantMemberRepository.GetMembersByTenantIdAsync(tenant.Id);
                tenantDto.Members = _mapper.Map<List<TenantMemberDto>>(members);
            }

            return tenantDto;
        }

        // Remove a member from tenantEntity using subdomain
        public async Task RemoveMemberAsync(string tenantSubdomain, Guid userId, Guid memberToRemoveId)
        {
            // Retrieve tenantEntity by subdomain
            var tenant = await _tenantRepository.GetTenantBySubdomainAsync(tenantSubdomain);
            if (tenant == null)
            {
                throw new TenantNotFoundException("Tenant not found.");
            }

            // Check if the current user has permission to remove members
            var tenantMember = await _tenantMemberRepository.GetUserTenantByUserIdAndTenantIdAsync(userId, tenant.Id);
            if (tenantMember == null || 
                !tenantMember.HasPermission("member:remove"))
            {
                throw new UnauthorizedAccessException("You do not have permission to remove a member.");
            }

            // Fetch the member to be removed
            var memberToRemove = await _tenantMemberRepository.GetUserTenantByUserIdAndTenantIdAsync(memberToRemoveId, tenant.Id);
            if (memberToRemove == null)
            {
                throw new MemberNotFoundException("Member not found in tenantEntity.");
            }

            // Check if the member being removed is not the tenantEntity owner
            if (memberToRemove.Roles.Any(role => role.Key == "Owner"))
            {
                throw new InvalidOperationException("The tenantEntity owner cannot be removed.");
            }

            // Proceed with member removal
            await _tenantMemberRepository.DeleteAsync(memberToRemove);
        }

        // Update a member's role in the tenantEntity using subdomain
        public async Task UpdateMemberRoleAsync(string tenantSubdomain, Guid userId, Guid memberToUpdateId, Guid newRoleId)
        {
            // Retrieve tenantEntity by subdomain
            var tenant = await _tenantRepository.GetTenantBySubdomainAsync(tenantSubdomain);
            if (tenant == null)
            {
                throw new TenantNotFoundException("Tenant not found.");
            }

            // Check if the current user has permission to update member roles
            var tenantMember = await _tenantMemberRepository.GetUserTenantByUserIdAndTenantIdAsync(userId, tenant.Id);
            if (tenantMember == null || 
                !tenantMember.HasPermission("member:update"))
            {
                throw new UnauthorizedAccessException("You do not have permission to update member roles.");
            }

            // Fetch the member whose role is being updated
            var memberToUpdate = await _tenantMemberRepository.GetUserTenantByUserIdAndTenantIdAsync(memberToUpdateId, tenant.Id);
            if (memberToUpdate == null)
            {
                throw new MemberNotFoundException("Member not found in tenantEntity.");
            }

            // Fetch the new role to assign
            var newRole = tenant.Roles.FirstOrDefault(x => x.Id == newRoleId);
            if (newRole == null)
            {
                throw new Exception("The specified role does not exist.");
            }

            // Clear current roles and assign the new role
            memberToUpdate.ClearRoles();
            memberToUpdate.AddRole(newRole);

            await _tenantMemberRepository.UpdateAsync(memberToUpdate);
        }

        public async Task<PaginatedResult<TenantMemberDto>> GetMembers(string tenantName, Guid userId,TenantMembersFilterDto filter)
        {
            // Retrieve tenantEntity by subdomain
            var tenant = await _tenantRepository.GetTenantBySubdomainAsync(tenantName);
            if (tenant == null)
            {
                throw new TenantNotFoundException("Tenant not found.");
            }

            // Check if the user has the permission to view members
            var tenantMember = await _tenantMemberRepository.GetUserTenantByUserIdAndTenantIdAsync(userId, tenant.Id);
            if (tenantMember == null || 
                !tenantMember.HasPermission("tenant:read"))
            {
                throw new UnauthorizedAccessException("You do not have permission to view these members.");
            }

            var data = await _tenantMemberRepository.PaginatedAsync(new TenanMemberByTenantSpec(tenant.Id, filter));


            return _mapper.Map<PaginatedResult<TenantMemberDto>>(data);
        }

        // Add a new member to the tenantEntity
       

        // Get members of the tenantEntity based on role permission
        public async Task<List<TenantMemberDto>> GetMembersByRoleAsync(string tenantSubdomain, Guid userId, string permissionKey)
        {
            // Retrieve tenantEntity by subdomain
            var tenant = await _tenantRepository.GetTenantBySubdomainAsync(tenantSubdomain);
            if (tenant == null)
            {
                throw new TenantNotFoundException("Tenant not found.");
            }

            // Check if the user has the permission to view members
            var tenantMember = await _tenantMemberRepository.GetUserTenantByUserIdAndTenantIdAsync(userId, tenant.Id);
            if (tenantMember == null || 
                !tenantMember.HasPermission(permissionKey))
            {
                throw new UnauthorizedAccessException("You do not have permission to view these members.");
            }

            var members = await _tenantMemberRepository.GetMembersByTenantIdAsync(tenant.Id);
            return _mapper.Map<List<TenantMemberDto>>(members);
        }
    }
}
