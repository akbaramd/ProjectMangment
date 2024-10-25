using PMS.Domain.BoundedContexts.TaskManagment;
using PMS.Domain.BoundedContexts.TenantManagment;
using PMS.Domain.Core;
using SharedKernel.DomainDrivenDesign.Domain;

namespace PMS.Domain.BoundedContexts.ProjectManagement
{
    // Entity representing a Project
    public class ProjectEntity : TenantAggregateRootBase
    {
        public string Name { get; private set; }
        public string Description { get; private set; }
        public DateTime StartDate { get; private set; }
        public DateTime? EndDate { get; private set; }

        private readonly List<ProjectMemberEntity> _members = new List<ProjectMemberEntity>();
        public virtual ICollection<ProjectMemberEntity> Members => _members.AsReadOnly();

        protected ProjectEntity()
        {
        }

        public ProjectEntity(string name, string description, DateTime startDate, TenantEntity tenant)
            : base(tenant)
        {
            Name = name;
            Description = description;
            StartDate = startDate;
        }

        // Add members to the project
        public void AddMember(ProjectMemberEntity memberEntity)
        {
            if (_members.Any(m => m.TenantMemberId == memberEntity.TenantMemberId))
                throw new InvalidOperationException("Member already exists in the project.");

            _members.Add(memberEntity);
        }

        public void RemoveMember(ProjectMemberEntity memberEntity)
        {
            if (!_members.Contains(memberEntity))
                throw new InvalidOperationException("Member not found in the project.");

            _members.Remove(memberEntity);
        }

        // Method to update project details
        public void UpdateDetails(string name, string description, DateTime? endDate)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentException("Project name cannot be empty.");

            Name = name;
            Description = description;
            EndDate = endDate;
        }

        public void ChangeStartDate(DateTime newStartDate)
        {
            if (newStartDate > EndDate)
                throw new InvalidOperationException("Start date cannot be after the end date.");
            
            StartDate = newStartDate;
        }

        public void ChangeEndDate(DateTime? newEndDate)
        {
            if (newEndDate < StartDate)
                throw new InvalidOperationException("End date cannot be before the start date.");

            EndDate = newEndDate;
        }


        // Update project member access
        public void UpdateMemberAccess(Guid memberId, ProjectMemberAccess newAccess)
        {
            var member = _members.FirstOrDefault(m => m.Id == memberId);
            if (member == null)
                throw new InvalidOperationException("Member not found in the project.");

            member.UpdateDetails(newAccess);
        }
    }
    
    public class ProjectMemberEntity : Entity<Guid>
    {
        protected ProjectMemberEntity() {}

        public ProjectMemberEntity( TenantMemberEntity tenantMember, ProjectEntity project, ProjectMemberAccess access) 
        {
            TenantMember = tenantMember;
            Project = project;
            Access = access;
        }

        public virtual TenantMemberEntity TenantMember { get; private set; }
        public Guid TenantMemberId { get; private set; }
        
        public virtual ProjectEntity Project { get; private set; }
        public Guid ProjectId { get; private set; }

        public ProjectMemberAccess Access { get; private set; }

        public  virtual ICollection<TaskCommentEntity> TaskComments { get; set; }

        private readonly List<TaskEntity> _tasks = new List<TaskEntity>();
        public virtual ICollection<TaskEntity> Tasks => _tasks.AsReadOnly();
        
        internal void UpdateDetails(ProjectMemberAccess access)
        {
            Access = access;
        }
    }

    public class ProjectMemberAccess : Enumeration
    {
        public static ProjectMemberAccess ProductOwner = new ProjectMemberAccess(0, nameof(ProductOwner)); 
        public static ProjectMemberAccess ScrumMaster = new ProjectMemberAccess(1, nameof(ScrumMaster)); 
        public static ProjectMemberAccess Maintainer = new ProjectMemberAccess(2, nameof(Maintainer)); 
        public static ProjectMemberAccess Quest = new ProjectMemberAccess(3, nameof(Quest)); 
        public ProjectMemberAccess(int id, string name) : base(id, name)
        {
        }
    }
}
