using AutoMapper;
using PMS.Application.Base;
using PMS.Application.UseCases.Boards.Exceptions;
using PMS.Application.UseCases.Boards.Models;
using PMS.Application.UseCases.Boards.Specs;
using PMS.Application.UseCases.Sprints.Exceptions;
using PMS.Domain.BoundedContexts.ProjectManagement;
using PMS.Domain.BoundedContexts.ProjectManagement.Projects.Repositories;
using PMS.Domain.BoundedContexts.TaskManagement.Kanban;
using PMS.Domain.BoundedContexts.TaskManagement.Kanban.Repositories;
using PMS.Domain.BoundedContexts.TaskManagement;
using SharedKernel.Model;

namespace PMS.Application.UseCases.Boards
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
            // Validate tenantEntity and permissions
            await ValidateTenantAccessAsync("board:read");

            var boards = await _boardRepository.PaginatedAsync(new BordsByTenantSpec(CurrentTenant.Id,dto));
            return _mapper.Map<PaginatedResult<BoardDto>>(boards);
        }

        // Get details of a specific board by ID
        public async Task<BoardDto> GetBoardDetailsAsync(Guid boardId)
        {
            // Validate tenantEntity and permissions
            await ValidateTenantAccessAsync("board:read");

            // Fetch the board and ensure it belongs to the current tenantEntity
            var board = await _boardRepository.GetByIdAsync(boardId);
            if (board == null || board.TenantId != CurrentTenant.Id)
            {
                throw new BoardNotFoundException();
            }

            return _mapper.Map<BoardDto>(board);
        }

        // Create a new board
        public async Task<BoardDto> CreateBoardAsync(BoardCreateDto boardCreateDto)
        {
            // Validate tenantEntity and permissions
            await ValidateTenantAccessAsync("board:create");

            // Check if the related sprint exists and belongs to the current tenantEntity
            var sprint = await _sprintRepository.GetByIdAsync(boardCreateDto.SprintId);
            if (sprint == null || sprint.TenantId != CurrentTenant.Id)
            {
                throw new SprintNotFoundException();
            }

            // Create the new board entity
            var board = new KanbanBoardEntity(boardCreateDto.Name, sprint,CurrentTenant);

            // Add the board to the repository
            await _boardRepository.AddAsync(board);

            return _mapper.Map<BoardDto>(board);
        }

        // Update an existing board
        public async Task<BoardDto> UpdateBoardAsync(Guid boardId, BoardUpdateDto boardUpdateDto)
        {
            // Validate tenantEntity and permissions
            await ValidateTenantAccessAsync("board:update");

            // Fetch the board and ensure it belongs to the current tenantEntity
            var board = await _boardRepository.GetByIdAsync(boardId);
            if (board == null || board.TenantId != CurrentTenant.Id)
            {
                throw new BoardNotFoundException();
            }

            // Update board details
            board.UpdateDetails(boardUpdateDto.Name);
            await _boardRepository.UpdateAsync(board);

            return _mapper.Map<BoardDto>(board);
        }

        // Delete a board
        public async Task<bool> DeleteBoardAsync(Guid boardId)
        {
            // Validate tenantEntity and permissions
            await ValidateTenantAccessAsync("board:remove");

            // Fetch the board and ensure it belongs to the current tenantEntity
            var board = await _boardRepository.GetByIdAsync(boardId);
            if (board == null || board.TenantId != CurrentTenant.Id)
            {
                throw new BoardNotFoundException();
            }

            // Delete the board
            await _boardRepository.DeleteAsync(board);
            return true;
        }

        public async Task<bool> UpdateColumnOrderAsync(Guid boardId, Guid columnId, BoardColumnUpdateDto dto)
        {
             // Validate tenantEntity and permissions
            await ValidateTenantAccessAsync("board:update");

            // Fetch the board and ensure it belongs to the current tenantEntity
            var board = await _boardRepository.GetByIdAsync(boardId);
            if (board == null || board.TenantId != CurrentTenant.Id)
            {
                throw new BoardNotFoundException();
            }

            // Update board details
            board.UpdateColumn(columnId,dto.Name,dto.Order);
            await _boardRepository.UpdateAsync(board);

            return true;
        }
    }
}
