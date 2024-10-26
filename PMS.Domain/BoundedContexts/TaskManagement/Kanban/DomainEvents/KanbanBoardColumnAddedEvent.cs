using SharedKernel.DomainDrivenDesign.Domain.DomainEvent;

namespace PMS.Domain.BoundedContexts.TaskManagement.Kanban.DomainEvents;

public class KanbanBoardColumnAddedEvent : DomainEventBase
{
    public Guid ColumnId { get; }
    public Guid BoardId { get; }

    public KanbanBoardColumnAddedEvent(Guid columnId, Guid boardId)
    {
        ColumnId = columnId;
        BoardId = boardId;
    }
}