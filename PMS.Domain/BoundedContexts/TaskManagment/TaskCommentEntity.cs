using SharedKernel.DomainDrivenDesign.Domain;

namespace PMS.Domain.BoundedContexts.TaskManagment
{
    public class TaskCommentEntity : Entity<Guid>
    {
        public Guid UserId { get; private set; }
        public string Content { get; private set; }
        public DateTime CreatedAt { get; private set; }
        public Guid SprintTaskId { get; private set; }

        protected TaskCommentEntity() { }

        public TaskCommentEntity(Guid userId, string content)
        {
            UserId = userId;
            Content = content;
            CreatedAt = DateTime.UtcNow;
        }

        public void UpdateContent(string newContent)
        {
            Content = newContent;
        }
    }
}
