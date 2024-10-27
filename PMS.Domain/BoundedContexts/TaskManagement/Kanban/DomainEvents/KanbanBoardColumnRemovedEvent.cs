using Bonyan.DomainDrivenDesign.Domain.Events;

namespace PMS.Domain.BoundedContexts.TaskManagement.Kanban.DomainEvents;

public class KanbanBoardColumnRemovedEvent : DomainEventBase
{
    public Guid ColumnId { get; }
    public Guid BoardId { get; }

    public KanbanBoardColumnRemovedEvent(Guid columnId, Guid boardId)
    {
        ColumnId = columnId;
        BoardId = boardId;
    }
}