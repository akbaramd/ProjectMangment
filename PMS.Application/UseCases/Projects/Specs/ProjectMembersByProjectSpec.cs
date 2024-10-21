using PMS.Application.DTOs;
using PMS.Domain.Entities;
using SharedKernel.Specification;

namespace PMS.Application.UseCases.Projects.Specs;

public class ProjectMembersByProjectSpec : PaginateSpecification<ProjectMember>
{
    public ProjectMembersByProjectSpec(Guid projectId, ProjectMemberFilterDto dto) : base(dto.Skip,dto.Take)
    {
        AddCriteria(x => x.ProjectId == projectId);

        AddInclude(x => x.Member).ThenInclude(x => x.User);
        
        if (dto.Search != null && !string.IsNullOrWhiteSpace(dto.Search))
        {
            AddCriteria(c => c.Member.User.PhoneNumber != null && (c.Member.User.FullName.Contains(dto.Search) || c.Member.User.PhoneNumber.Contains(dto.Search)));
        }
    }
}
