using Bonyan.Layer.Domain.Events;

namespace PMS.Domain.BoundedContexts.TaskManagement.Kanban.DomainEvents;

public class KanbanBoardCreatedEvent : DomainEventBase
{
    public Guid BoardId { get; }
    public string Name { get; }
    public Guid SprintId { get; }

    public KanbanBoardCreatedEvent(Guid boardId, string name, Guid sprintId)
    {
        BoardId = boardId;
        Name = name;
        SprintId = sprintId;
    }
}