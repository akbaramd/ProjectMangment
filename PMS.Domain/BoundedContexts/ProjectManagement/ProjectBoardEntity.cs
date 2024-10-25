using PMS.Domain.BoundedContexts.TaskManagment;
using PMS.Domain.BoundedContexts.TenantManagment;
using PMS.Domain.Core;
using SharedKernel.DomainDrivenDesign.Domain;

namespace PMS.Domain.BoundedContexts.ProjectManagement
{
    // Entity representing a Project Board
    public class ProjectBoardEntity : TenantAggregateRootBase
    {
        public string Name { get; private set; }
        private readonly List<ProjectBoardColumnEntity> _columns = new List<ProjectBoardColumnEntity>();
        public virtual ICollection<ProjectBoardColumnEntity> Columns => _columns.AsReadOnly();

        protected ProjectBoardEntity() { }
        public Guid SprintId { get; private set; }
        public virtual ProjectSprintEntity Sprint { get; private set; }

        public ProjectBoardEntity(string name, ProjectSprintEntity sprint, TenantEntity tenant)
            : base(tenant)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentException("Board name cannot be empty.");

            Name = name;
            Sprint = sprint;
            SprintId = sprint.Id;
            InitializeDefaultColumns();
        }

        // Initialize default columns for the board
        private void InitializeDefaultColumns()
        {
            AddColumn(new ProjectBoardColumnEntity("ToDo", 1, Tenant));
            AddColumn(new ProjectBoardColumnEntity("Doing", 2, Tenant));
            AddColumn(new ProjectBoardColumnEntity("Done", 3, Tenant));
        }

        // Add a new column to the board
        public void AddColumn(ProjectBoardColumnEntity columnEntity)
        {
            if (_columns.Any(c => c.Name == columnEntity.Name))
                throw new InvalidOperationException("Column with the same name already exists in the board.");

            _columns.Add(columnEntity);
        }

        // Update board details
        public void UpdateDetails(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentException("Board name cannot be empty.");

            Name = name;
        }

        // Remove a column from the board
        public void RemoveColumn(ProjectBoardColumnEntity columnEntity)
        {
            if (!_columns.Contains(columnEntity))
                throw new InvalidOperationException("Column not found in the board.");

            _columns.Remove(columnEntity);
        }
    }

    // Entity representing a Project Board Column
    public class ProjectBoardColumnEntity : TenantEntityBase
    {
        public string Name { get; private set; }
        public int Order { get; private set; }

        public virtual ProjectBoardEntity Board { get; private set; }
        public Guid BoardId { get; private set; }
        private readonly List<TaskEntity> _tasks = new List<TaskEntity>();
        public virtual ICollection<TaskEntity> Tasks => _tasks.AsReadOnly();

        protected ProjectBoardColumnEntity() { }

        public ProjectBoardColumnEntity(string name, int order, TenantEntity tenant)
            : base(tenant)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentException("Column name cannot be empty.");

            if (order < 0)
                throw new ArgumentException("Order must be a positive number.");

            Name = name;
            Order = order;
        }

        // Add a task to the column
        public void AddTask(TaskEntity taskEntity)
        {
            if (_tasks.Any(t => t.Id == taskEntity.Id))
                throw new InvalidOperationException("Task with the same ID already exists in the column.");

            _tasks.Add(taskEntity);
        }

        // Remove a task from the column
        public void RemoveTask(TaskEntity taskEntity)
        {
            if (!_tasks.Contains(taskEntity))
                throw new InvalidOperationException("Task not found in the column.");

            _tasks.Remove(taskEntity);
        }

        // Update column details
        public void UpdateDetails(string name, int order)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentException("Column name cannot be empty.");

            if (order < 0)
                throw new ArgumentException("Order must be a positive number.");

            Name = name;
            Order = order;
        }
    }
}
