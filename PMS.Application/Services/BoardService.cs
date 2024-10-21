using AutoMapper;
using PMS.Application.DTOs;
using PMS.Application.Exceptions;
using PMS.Application.Interfaces;
using PMS.Domain.Entities;
using PMS.Domain.Repositories;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using PMS.Application.UseCases.Bords.Specs;
using PMS.Application.UseCases.Sprints.Specs;
using SharedKernel.Model;

namespace PMS.Application.Services
{
    public class BoardService : BaseTenantService, IBoardService
    {
        private readonly IBoardRepository _boardRepository;
        private readonly ISprintRepository _sprintRepository;
        private readonly IMapper _mapper;

        public BoardService(
            IBoardRepository boardRepository,
            ISprintRepository sprintRepository,
            IMapper mapper,
            IServiceProvider serviceProvider)
            : base(serviceProvider)
        {
            _boardRepository = boardRepository;
            _sprintRepository = sprintRepository;
            _mapper = mapper;
        }

        // Get all boards by sprint ID
        public async Task<PaginatedResult<BoardDto>> GetBoards(BorderFilterDto dto)
        {
            // Validate tenant and permissions
            await ValidateTenantAccessAsync("board:read");

            var boards = await _boardRepository.PaginatedAsync(new BordsByTenantSpec(CurrentTenant.Id,dto));
            return _mapper.Map<PaginatedResult<BoardDto>>(boards);
        }

        // Get details of a specific board by ID
        public async Task<BoardDto> GetBoardDetailsAsync(Guid boardId)
        {
            // Validate tenant and permissions
            await ValidateTenantAccessAsync("board:read");

            // Fetch the board and ensure it belongs to the current tenant
            var board = await _boardRepository.GetByIdAsync(boardId);
            if (board == null || board.TenantId != CurrentTenant.Id)
            {
                throw new BoardNotFoundException();
            }

            return _mapper.Map<BoardDto>(board);
        }

        // Create a new board
        public async Task<BoardDto> CreateBoardAsync(CreateBoardDto createBoardDto)
        {
            // Validate tenant and permissions
            await ValidateTenantAccessAsync("board:create");

            // Check if the related sprint exists and belongs to the current tenant
            var sprint = await _sprintRepository.GetByIdAsync(createBoardDto.SprintId);
            if (sprint == null || sprint.TenantId != CurrentTenant.Id)
            {
                throw new SprintNotFoundException();
            }

            // Create the new board entity
            var board = new Board(createBoardDto.Name, sprint,CurrentTenant);

            // Add the board to the repository
            await _boardRepository.AddAsync(board);

            return _mapper.Map<BoardDto>(board);
        }

        // Update an existing board
        public async Task<BoardDto> UpdateBoardAsync(Guid boardId, UpdateBoardDto updateBoardDto)
        {
            // Validate tenant and permissions
            await ValidateTenantAccessAsync("board:update");

            // Fetch the board and ensure it belongs to the current tenant
            var board = await _boardRepository.GetByIdAsync(boardId);
            if (board == null || board.TenantId != CurrentTenant.Id)
            {
                throw new BoardNotFoundException();
            }

            // Update board details
            board.UpdateDetails(updateBoardDto.Name);
            await _boardRepository.UpdateAsync(board);

            return _mapper.Map<BoardDto>(board);
        }

        // Delete a board
        public async Task<bool> DeleteBoardAsync(Guid boardId)
        {
            // Validate tenant and permissions
            await ValidateTenantAccessAsync("board:delete");

            // Fetch the board and ensure it belongs to the current tenant
            var board = await _boardRepository.GetByIdAsync(boardId);
            if (board == null || board.TenantId != CurrentTenant.Id)
            {
                throw new BoardNotFoundException();
            }

            // Delete the board
            await _boardRepository.DeleteAsync(board);
            return true;
        }
    }
}
