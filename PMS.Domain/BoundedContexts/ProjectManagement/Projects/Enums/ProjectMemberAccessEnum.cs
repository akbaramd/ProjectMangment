using SharedKernel.DomainDrivenDesign.Domain;

namespace PMS.Domain.BoundedContexts.ProjectManagement.Projects.Enums;

public class ProjectMemberAccessEnum : Enumeration
{
    public static ProjectMemberAccessEnum ProductOwner = new ProjectMemberAccessEnum(0, nameof(ProductOwner)); 
    public static ProjectMemberAccessEnum ScrumMaster = new ProjectMemberAccessEnum(1, nameof(ScrumMaster)); 
    public static ProjectMemberAccessEnum Maintainer = new ProjectMemberAccessEnum(2, nameof(Maintainer)); 
    public static ProjectMemberAccessEnum Quest = new ProjectMemberAccessEnum(3, nameof(Quest)); 
    public ProjectMemberAccessEnum(int id, string name) : base(id, name)
    {
    }
}