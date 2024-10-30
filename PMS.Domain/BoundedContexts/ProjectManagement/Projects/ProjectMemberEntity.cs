using Bonyan.Layer.Domain.Entities;
using PMS.Domain.BoundedContexts.ProjectManagement.Projects.Enums;
using PMS.Domain.BoundedContexts.TaskManagement.Tasks;
using PMS.Domain.BoundedContexts.TenantManagement;

namespace PMS.Domain.BoundedContexts.ProjectManagement.Projects;

public class ProjectMemberEntity : Entity<Guid>
{
    protected ProjectMemberEntity() {}

    public ProjectMemberEntity( TenantMemberEntity tenantMember, ProjectEntity project, ProjectMemberAccessEnum accessEnum) 
    {
        TenantMember = tenantMember;
        Project = project;
        AccessEnum = accessEnum;
    }

    public virtual TenantMemberEntity TenantMember { get; private set; }
    public Guid TenantMemberId { get; private set; }
        
    public virtual ProjectEntity Project { get; private set; }
    public Guid ProjectId { get; private set; }

    public ProjectMemberAccessEnum AccessEnum { get; private set; }

    public  virtual ICollection<TaskCommentEntity> TaskComments { get; set; }

    private readonly List<TaskEntity> _tasks = new List<TaskEntity>();
    public virtual ICollection<TaskEntity> Tasks => _tasks.AsReadOnly();
        
    internal void UpdateDetails(ProjectMemberAccessEnum accessEnum)
    {
        AccessEnum = accessEnum;
    }
}