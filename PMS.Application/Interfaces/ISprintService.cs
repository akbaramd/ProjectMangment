using PMS.Application.DTOs;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PMS.Application.Interfaces
{
    public interface ISprintService
    {
        Task<List<SprintDto>> GetSprintsByProjectIdAsync(Guid projectId, string tenantId, Guid userId);
    }
}
