using PMS.Domain.BoundedContexts.AttachmentManagement;
using PMS.Domain.BoundedContexts.ProjectManagement;
using SharedKernel.DomainDrivenDesign.Domain;

namespace PMS.Domain.BoundedContexts.TaskManagment
{
    public class TaskAttachmentEntity : Entity<Guid>
    {
        public Guid ProjectMemberId { get; private set; }
        public virtual  ProjectMemberEntity ProjectMember { get; private set; }
        
        public Guid TaskId { get; private set; }
        public virtual  TaskEntity Task { get; private set; }
        
        public Guid AttachmentId { get; private set; }
        public virtual  AttachmentEntity Attachment { get; private set; }

        protected TaskAttachmentEntity() { }

        public TaskAttachmentEntity(TaskEntity entity,ProjectMemberEntity member, AttachmentEntity attachment)
        {
            Task = entity;
            ProjectMember = member;
            Attachment = attachment;
        }
    }
}
