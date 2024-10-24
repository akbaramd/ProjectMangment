using PMS.Domain.Core;

namespace PMS.Domain.Entities;

public class Sprint : TenantEntity
{
    public string Name { get; private set; }
    public DateTime StartDate { get; private set; }
    public DateTime EndDate { get; private set; }

    public Project Project { get; private set; }
    public Guid ProjectId { get; private set; }
    
    private readonly List<SprintTask> _tasks = new List<SprintTask>();
    public ICollection<SprintTask> Tasks => _tasks.AsReadOnly();
    
    private readonly List<Board> _boards = new List<Board>();
    public ICollection<Board> Boards => _boards.AsReadOnly();

    protected Sprint() { }

    public Sprint(string name, DateTime startDate, DateTime endDate, Tenant tenant)
        : base(tenant)
    {
        Name = name;
        StartDate = startDate;
        EndDate = endDate;
    }

    public void AddTask(SprintTask task)
    {
        _tasks.Add(task);
    }

    public void UpdateDetails(string name, DateTime startDate, DateTime endDate)
    {
        Name = name;
        StartDate = startDate;
        EndDate = endDate;
    }
}