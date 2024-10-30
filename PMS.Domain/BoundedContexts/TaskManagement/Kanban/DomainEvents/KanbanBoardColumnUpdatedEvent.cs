using Bonyan.Layer.Domain.Events;

namespace PMS.Domain.BoundedContexts.TaskManagement.Kanban.DomainEvents;

public class KanbanBoardColumnUpdatedEvent : DomainEventBase
{
    public Guid ColumnId { get; }
    public string NewName { get; }
    public int NewOrder { get; }

    public KanbanBoardColumnUpdatedEvent(Guid columnId, string newName, int newOrder)
    {
        ColumnId = columnId;
        NewName = newName;
        NewOrder = newOrder;
    }
}