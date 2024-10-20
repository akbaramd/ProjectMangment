using AutoMapper;
using PMS.Application.DTOs;
using PMS.Application.Exceptions;
using PMS.Application.Interfaces;
using PMS.Domain.Repositories;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnauthorizedAccessException = PMS.Application.Exceptions.UnauthorizedAccessException;

namespace PMS.Application.Services
{
    public class SprintService : ISprintService
    {
        private readonly ISprintRepository _sprintRepository;
        private readonly ITenantRepository _tenantRepository;
        private readonly ITenantMemberRepository _tenantMemberRepository;
        private readonly IMapper _mapper;

        public SprintService(
            ISprintRepository sprintRepository,
            ITenantRepository tenantRepository,
            ITenantMemberRepository tenantMemberRepository,
            IMapper mapper)
        {
            _sprintRepository = sprintRepository;
            _tenantRepository = tenantRepository;
            _tenantMemberRepository = tenantMemberRepository;
            _mapper = mapper;
        }

        public async Task<List<SprintDto>> GetSprintsByProjectIdAsync(Guid projectId, string tenantSubdomain, Guid userId)
        {
            var tenant = await _tenantRepository.GetTenantBySubdomainAsync(tenantSubdomain);
            if (tenant == null)
            {
                throw new TenantNotFoundException();
            }

            var tenantMember = await _tenantMemberRepository.GetUserTenantByUserIdAndTenantIdAsync(userId, tenant.Id);
            if (tenantMember == null || !tenantMember.HasPermission("project:read"))
            {
                throw new UnauthorizedAccessException("You do not have permission to read this project.");
            }

            var sprints = _sprintRepository.GetByProjectId(projectId);
            return _mapper.Map<List<SprintDto>>(sprints);
        }
    }
}
