using PMS.Domain.BoundedContexts.ProjectManagement.Projects;
using PMS.Domain.Core;
using PMS.Domain.BoundedContexts.TaskManagement.Kanban.DomainEvents;

namespace PMS.Domain.BoundedContexts.TaskManagement.Kanban
{
    public class KanbanBoardEntity : TenantAggregateRootBase
    {
        public string Name { get; private set; }
        private readonly List<KanbanBoardColumnEntity> _columns = new List<KanbanBoardColumnEntity>();
        public virtual ICollection<KanbanBoardColumnEntity> Columns => _columns.AsReadOnly();

        protected KanbanBoardEntity() { }

        public Guid SprintId { get; private set; }
        public virtual ProjectSprintEntity Sprint { get; private set; }

        public KanbanBoardEntity(string name, ProjectSprintEntity sprint)
          
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentException("Board name cannot be empty.");

            Name = name;
            Sprint = sprint;
            SprintId = sprint.Id;
            InitializeDefaultColumns();
            
            AddDomainEvent(new KanbanBoardCreatedEvent(this.Id, Name, SprintId));
        }

        private void InitializeDefaultColumns()
        {
            AddColumn(new KanbanBoardColumnEntity("ToDo", 1));
            AddColumn(new KanbanBoardColumnEntity("Doing", 2));
            AddColumn(new KanbanBoardColumnEntity("Done", 3));
        }

        public void AddColumn(KanbanBoardColumnEntity columnEntity)
        {
            if (_columns.Any(c => c.Name == columnEntity.Name))
                throw new InvalidOperationException("Column with the same name already exists in the board.");

            _columns.Add(columnEntity);
            AddDomainEvent(new KanbanBoardColumnAddedEvent(columnEntity.Id, this.Id));
        }

        public void UpdateDetails(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentException("Board name cannot be empty.");

            Name = name;
            AddDomainEvent(new KanbanBoardDetailsUpdatedEvent(this.Id, Name));
        }

        public void RemoveColumn(KanbanBoardColumnEntity columnEntity)
        {
            if (!_columns.Contains(columnEntity))
                throw new InvalidOperationException("Column not found in the board.");

            _columns.Remove(columnEntity);
            AddDomainEvent(new KanbanBoardColumnRemovedEvent(columnEntity.Id, this.Id));
        }

        public void UpdateColumn(Guid columnId, string newName, int newOrder)
        {
            var columnToUpdate = _columns.FirstOrDefault(c => c.Id == columnId);
            if (columnToUpdate == null)
                throw new InvalidOperationException("Column not found in the board.");

            columnToUpdate.UpdateDetails(newName, newOrder);
            AddDomainEvent(new KanbanBoardColumnUpdatedEvent(columnId, newName, newOrder));
        }
    }
}
