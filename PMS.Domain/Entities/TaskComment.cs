using SharedKernel.DomainDrivenDesign.Domain;

namespace PMS.Domain.Entities;

public abstract class TaskComment : Entity<Guid>
{
    public Guid UserId { get; private set; } // The user who made the comment
    public string Content { get; private set; }
    public DateTime CreatedAt { get; private set; }

    protected TaskComment() { }

    public TaskComment(Guid userId, string content)
    {
        UserId = userId;
        Content = content;
        CreatedAt = DateTime.UtcNow;
    }
}