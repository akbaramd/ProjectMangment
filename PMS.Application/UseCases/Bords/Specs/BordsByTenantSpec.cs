using PMS.Application.DTOs;
using PMS.Domain.Entities;
using SharedKernel.Specification;

namespace PMS.Application.UseCases.Bords.Specs;

public class BordsByTenantSpec : PaginateSpecification<Board>
{
    public BordsByTenantSpec(Guid tenantId, BorderFilterDto dto) : base(dto.Skip,dto.Take)
    {

        AddIncludeCollection(x => x.Columns).ThenIncludeCollection(x => x.Tasks);
        
        AddCriteria(x => x.TenantId == tenantId);
        
        if (dto.Search != null && !string.IsNullOrWhiteSpace(dto.Search))
        {
            AddCriteria(c => c.Name.Contains(dto.Search));
        }
        
        if (dto.SprintId != null &&dto.SprintId != Guid.Empty)
        {
            AddCriteria(c => c.SprintId == dto.SprintId);
        }
    }
}
