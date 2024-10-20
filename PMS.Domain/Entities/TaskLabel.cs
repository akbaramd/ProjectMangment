using SharedKernel.DomainDrivenDesign.Domain;

namespace PMS.Domain.Entities;

public abstract class TaskLabel : Entity<Guid>
{
    public string Name { get; private set; }

    protected TaskLabel() { }

    public TaskLabel(string name)
    {
        Name = name;
    }
}