using PMS.Application.DTOs;
using SharedKernel.Model;
using System;
using System.Threading.Tasks;

namespace PMS.Application.Interfaces
{
    public interface IBoardService
    {
        Task<PaginatedResult<BoardDto>> GetBoards(BorderFilterDto dto);
        Task<BoardDto> GetBoardDetailsAsync(Guid boardId);
        Task<BoardDto> CreateBoardAsync(CreateBoardDto createBoardDto);
        Task<BoardDto> UpdateBoardAsync(Guid boardId, UpdateBoardDto updateBoardDto);
        Task<bool> DeleteBoardAsync(Guid boardId);
    }
}