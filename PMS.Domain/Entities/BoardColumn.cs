using PMS.Domain.Core;

namespace PMS.Domain.Entities;

public class BoardColumn : TenantEntity
{
    public string Name { get; private set; }
    public int Order { get; private set; }

    public Board Board { get; private set; }
    public Guid BoardId { get; private set; }
    private readonly List<SprintTask> _tasks = new List<SprintTask>();
    public IReadOnlyCollection<SprintTask> Tasks => _tasks.AsReadOnly();

    protected BoardColumn() { }

    public BoardColumn(string name, int order,Tenant tenant):base(tenant)
    {
        Name = name;
        Order = order;
      
    }

    public void AddTask(SprintTask task)
    {
        _tasks.Add(task);
    }
}