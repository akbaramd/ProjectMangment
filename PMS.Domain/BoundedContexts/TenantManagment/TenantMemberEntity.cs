using PMS.Domain.BoundedContexts.ProjectManagement;
using PMS.Domain.BoundedContexts.TaskManagment;
using PMS.Domain.BoundedContexts.UserManagment;
using PMS.Domain.Core;
using SharedKernel.DomainDrivenDesign.Domain;

namespace PMS.Domain.BoundedContexts.TenantManagment
{
    public class TenantMemberEntity : TenantEntityBase
    {
        public Guid UserId { get; private set; }
        public virtual ApplicationUser User { get; private set; }
        public TenantMemberStatus Status { get; private set; }
        public Guid SprintTaskId { get; private set; } // اضافه کردن این ویژگی

        private readonly List<TenantRoleEntity> _roles = new List<TenantRoleEntity>();
        public virtual ICollection<TenantRoleEntity> Roles => _roles.AsReadOnly(); // Expose as read-only collection
        
        
        private readonly List<ProjectMemberEntity> _projectMembers = new List<ProjectMemberEntity>();
        public virtual ICollection<ProjectMemberEntity> ProjectMembers => _projectMembers.AsReadOnly(); // Expose as read-only collection
     public virtual ICollection<TaskEntity> Tasks { get; private set; } = new List<TaskEntity>();
        protected TenantMemberEntity() { }

        public TenantMemberEntity(ApplicationUser user, TenantEntity tenant)
            : base(tenant)
        {
            UserId = user.Id;
            User = user;
            Status = TenantMemberStatus.Active;
        }

        // Change the status of the user within the tenantEntity (Active, Inactive, Banned)
        public void ChangeStatus(TenantMemberStatus memberStatus)
        {
            if (Status == memberStatus)
            {
                throw new InvalidOperationException("The member already has the specified status.");
            }
            Status = memberStatus;
        }

        // Add a role to the member, if they don't already have it
        public void AddRole(TenantRoleEntity roleEntity)
        {
            if (HasRole(roleEntity))
            {
                throw new InvalidOperationException($"The member already has the role '{roleEntity.Key}'.");
            }
            _roles.Add(roleEntity);
        }

        // Remove a role from the member
        public void RemoveRole(TenantRoleEntity roleEntity)
        {
            if (!HasRole(roleEntity))
            {
                throw new InvalidOperationException($"The member does not have the role '{roleEntity.Key}' to remove.");
            }
            _roles.Remove(roleEntity);
        }

        // Check if the user has a specific role
        public bool HasRole(TenantRoleEntity roleEntity)
        {
            return _roles.Any(r => r.Id == roleEntity.Id);
        }

        // Clear all roles for the member
        public void ClearRoles()
        {
            _roles.Clear();
        }

        // Method to check if a specific permission key exists within the user's roles
        public bool HasPermission(string permissionKey)
        {
            return _roles.Any(role => role.Permissions.Any(permission => permission.Key == permissionKey));
        }
    }

    // Enum to represent the status of a TenantMember
    public class TenantMemberStatus : Enumeration
    {
        public static TenantMemberStatus InActive = new TenantMemberStatus(0, nameof(InActive)); 
        public static TenantMemberStatus Active = new TenantMemberStatus(1, nameof(Active)); 
        public static TenantMemberStatus Banned = new TenantMemberStatus(2, nameof(Banned)); 
        public TenantMemberStatus(int id, string name) : base(id, name)
        {
        }
    }
}
