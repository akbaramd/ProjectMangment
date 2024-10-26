using PMS.Application.UseCases.Boards.Models;
using SharedKernel.Model;

namespace PMS.Application.UseCases.Boards
{
    public interface IBoardService
    {
        Task<PaginatedResult<BoardDto>> GetBoards(BorderFilterDto dto);
        Task<BoardDto> GetBoardDetailsAsync(Guid boardId);
        Task<BoardDto> CreateBoardAsync(BoardCreateDto boardCreateDto);
        Task<BoardDto> UpdateBoardAsync(Guid boardId, BoardUpdateDto boardUpdateDto);
        Task<bool> DeleteBoardAsync(Guid boardId);

        Task<bool> UpdateColumnOrderAsync(Guid boardId,Guid columnId, BoardColumnUpdateDto dto);
  
    }
}