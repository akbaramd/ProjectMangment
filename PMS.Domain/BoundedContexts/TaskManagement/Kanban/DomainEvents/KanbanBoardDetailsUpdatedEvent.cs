using Bonyan.Layer.Domain.Events;

namespace PMS.Domain.BoundedContexts.TaskManagement.Kanban.DomainEvents;

public class KanbanBoardDetailsUpdatedEvent : DomainEventBase
{
    public Guid BoardId { get; }
    public string NewName { get; }

    public KanbanBoardDetailsUpdatedEvent(Guid boardId, string newName)
    {
        BoardId = boardId;
        NewName = newName;
    }
}