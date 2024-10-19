using PMS.Domain.Entities;
using SharedKernel.DomainDrivenDesign.Domain.Repository;

namespace PMS.Domain.Repositories
{
    public interface ISprintRepository : IGenericRepository<Sprint>
    {
        List<Sprint> GetAllWithRelations();
        Task<Sprint?> GetByIdWithRelationsAsync(Guid sprintId);
        List<Sprint> GetByProjectId(Guid projectId);
    }
}