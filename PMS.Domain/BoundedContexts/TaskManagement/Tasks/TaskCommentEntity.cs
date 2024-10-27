using Bonyan.DomainDrivenDesign.Domain.Entities;
using PMS.Domain.BoundedContexts.ProjectManagement.Projects;

namespace PMS.Domain.BoundedContexts.TaskManagement.Tasks
{
    public class TaskCommentEntity : Entity<Guid>
    {
        public virtual ProjectMemberEntity ProjectMember { get; private set; }
        public Guid  ProjectMemberId { get; private set; }
        public string Content { get; private set; }
        public DateTime CreatedAt { get; private set; }
        public Guid TaskId { get; private set; }
        public virtual TaskEntity Task { get; private set; }

        protected TaskCommentEntity() { }

        public TaskCommentEntity(TaskEntity task,ProjectMemberEntity member, string content)
        {
            Task = task;
            ProjectMember = member;
            ProjectMemberId = member.Id;
            Content = content;
            CreatedAt = DateTime.UtcNow;
        }

        public void UpdateContent(string newContent)
        {
            Content = newContent;
        }
        
    }
}
