using PMS.Domain.BoundedContexts.TaskManagment;
using PMS.Domain.BoundedContexts.TenantManagment;
using PMS.Domain.Core;

namespace PMS.Domain.BoundedContexts.ProjectManagement;

public class ProjectSprintEntity : TenantAggregateRootBase
    {
        public string Name { get; private set; }
        public DateTime StartDate { get; private set; }
        public DateTime EndDate { get; private set; }

        public virtual ProjectEntity Project { get; private set; }
        public Guid ProjectId { get; private set; }

        protected ProjectSprintEntity() { }

        public ProjectSprintEntity(ProjectEntity entity,string name, DateTime startDate, DateTime endDate, TenantEntity tenant)
            : base(tenant)
        {
            if (endDate <= startDate)
                throw new InvalidOperationException("End date must be after start date.");

            Project = entity;
            Name = name;
            StartDate = startDate;
            EndDate = endDate;
        }

        public void UpdateDetails(string name, DateTime startDate, DateTime endDate)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentException("Sprint name cannot be empty.");

            if (endDate <= startDate)
                throw new InvalidOperationException("End date must be after start date.");

            Name = name;
            StartDate = startDate;
            EndDate = endDate;
        }
    }