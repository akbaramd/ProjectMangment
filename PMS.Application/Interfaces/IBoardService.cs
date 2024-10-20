using PMS.Application.DTOs;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PMS.Application.Interfaces
{
    public interface IBoardService
    {
        Task<List<BoardDto>> GetBoardsBySprintIdAsync(Guid sprintId, string tenantId, Guid userId);
        Task<BoardDto> GetBoardDetailsAsync(Guid boardId, string tenantSubdomain, Guid userId);
    }
}
