using PMS.Domain.BoundedContexts.TenantManagement;
using PMS.Domain.Core;

namespace PMS.Domain.BoundedContexts.TaskManagement.Kanban;

public class KanbanBoardColumnEntity : TenantEntityBase
{
    public string Name { get; private set; }
    public int Order { get; private set; }

    public Guid BoardId { get; private set; }

    
    protected KanbanBoardColumnEntity() { }

    public KanbanBoardColumnEntity(string name, int order, TenantEntity tenant)
        : base(tenant)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("Column name cannot be empty.");

        if (order < 0)
            throw new ArgumentException("Order must be a positive number.");

        Name = name;
        Order = order;
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