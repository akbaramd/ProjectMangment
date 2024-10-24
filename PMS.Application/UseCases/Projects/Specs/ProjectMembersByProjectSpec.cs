using PMS.Application.DTOs;
using PMS.Domain.Entities;
using SharedKernel.Specification;

namespace PMS.Application.UseCases.Projects.Specs;

public class ProjectMembersByProjectSpec : PaginatedSpecification<ProjectMember>
{
    public  Guid ProjectId { get; set; }
    public  ProjectMemberFilterDto Filter{ get; set; }

    public ProjectMembersByProjectSpec(Guid projectId, ProjectMemberFilterDto dto) : base(dto.Skip,dto.Take)
    {
        ProjectId = projectId;
        Filter = dto;
    }

    public override void Handle(ISpecificationContext<ProjectMember> context)
    {
        context. AddCriteria(x => x.ProjectId == ProjectId);

        context.AddInclude(x => x.TenantMember).ThenInclude(x => x.User);
        
        if (Filter.Search != null && !string.IsNullOrWhiteSpace(Filter.Search))
        {
            context.AddCriteria(c => c.TenantMember.User.PhoneNumber != null && (c.TenantMember.User.FullName.Contains(Filter.Search) || c.TenantMember.User.PhoneNumber.Contains(Filter.Search)));
        }
    }
}
