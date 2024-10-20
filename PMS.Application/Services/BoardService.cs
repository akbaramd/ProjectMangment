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
    public class BoardService : IBoardService
    {
        private readonly IBoardRepository _boardRepository;
        private readonly ITenantRepository _tenantRepository;
        private readonly ITenantMemberRepository _tenantMemberRepository;
        private readonly IMapper _mapper;

        public BoardService(
            IBoardRepository boardRepository,
            ITenantRepository tenantRepository,
            ITenantMemberRepository tenantMemberRepository,
            IMapper mapper)
        {
            _boardRepository = boardRepository;
            _tenantRepository = tenantRepository;
            _tenantMemberRepository = tenantMemberRepository;
            _mapper = mapper;
        }

        public async Task<List<BoardDto>> GetBoardsBySprintIdAsync(Guid sprintId, string tenantSubdomain, Guid userId)
        {
            var tenant = await _tenantRepository.GetTenantBySubdomainAsync(tenantSubdomain);
            if (tenant == null)
            {
                throw new TenantNotFoundException();
            }

            var tenantMember = await _tenantMemberRepository.GetUserTenantByUserIdAndTenantIdAsync(userId, tenant.Id);
            if (tenantMember == null || !tenantMember.HasPermission("project:read"))
            {
                throw new UnauthorizedAccessException("You do not have permission to read this sprint.");
            }

            var boards = _boardRepository.GetBoardsBySprintIdAsync(sprintId);
            return _mapper.Map<List<BoardDto>>(boards);
        }

        public async Task<BoardDto> GetBoardDetailsAsync(Guid boardId, string tenantSubdomain, Guid userId)
        {
            var tenant = await _tenantRepository.GetTenantBySubdomainAsync(tenantSubdomain);
            if (tenant == null)
            {
                throw new TenantNotFoundException();
            }

            var tenantMember = await _tenantMemberRepository.GetUserTenantByUserIdAndTenantIdAsync(userId, tenant.Id);
            if (tenantMember == null || !tenantMember.HasPermission("project:read"))
            {
                throw new UnauthorizedAccessException("You do not have permission to read this board.");
            }

            var board = await _boardRepository.GetByIdAsync(boardId);
            if (board == null || board.TenantId != tenant.Id)
            {
                throw new Exception("Board not found or does not belong to the tenant.");
            }

            return _mapper.Map<BoardDto>(board);
        }
    }
}
