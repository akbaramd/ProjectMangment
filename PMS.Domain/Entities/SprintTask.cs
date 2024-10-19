using PMS.Domain.Core;
using SprintTaskStatus = PMS.Domain.Core.TaskStatus;

namespace PMS.Domain.Entities;

public class SprintTask : TenantEntity
{
    public string Title { get; private set; }
    public string Description { get; private set; }
    public SprintTaskStatus Status { get; private set; }
    public int Order { get; private set; }

    public Guid SprintId { get; private set; }
    public Sprint Sprint { get; private set; }

    public Guid BoardColumnId { get; private set; }
    public BoardColumn CurrentColumn { get; private set; } // The current column in which the task is located

    protected SprintTask() { }

    public SprintTask(string title, string description, int order, BoardColumn initialColumn, Sprint sprint, Tenant tenant)
        : base(tenant)
    {
        Title = title;
        Description = description;
        Status = SprintTaskStatus.ToDo;
        Order = order;
        CurrentColumn = initialColumn;
        Sprint = sprint;
        SprintId = sprint.Id;
        BoardColumnId = initialColumn.Id;
    }

    public void ChangeStatus(SprintTaskStatus newStatus)
    {
        Status = newStatus;
    }

    public void ChangeOrder(int newOrder)
    {
        Order = newOrder;
    }

    public void MoveToColumn(BoardColumn newColumn)
    {
        CurrentColumn = newColumn;
        BoardColumnId = newColumn.Id;
    }
}