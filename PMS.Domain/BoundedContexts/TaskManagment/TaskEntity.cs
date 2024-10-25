using PMS.Domain.BoundedContexts.ProjectManagement;
using PMS.Domain.BoundedContexts.TenantManagment;
using PMS.Domain.Core;
using SharedKernel.DomainDrivenDesign.Domain;

namespace PMS.Domain.BoundedContexts.TaskManagment
{
    public class TaskEntity : TenantAggregateRootBase
    {
        public string Title { get; private set; }
        public string Description { get; private set; }
        public string Content { get; private set; }

        public int Order { get; private set; }
        public DateTime? DueDate { get; private set; }
        public DateTime CreatedAt { get; private set; }
        public DateTime? UpdatedAt { get; private set; }

        // Relations
        public Guid BoardColumnId { get; private set; }
        public virtual ProjectBoardColumnEntity BoardColumn { get; private set; }

        private readonly List<TaskLabelEntity> _labels = new List<TaskLabelEntity>();
        public virtual ICollection<TaskLabelEntity> Labels => _labels.AsReadOnly();

        private readonly List<TenantMemberEntity> _assigneeMembers = new List<TenantMemberEntity>();
        public virtual ICollection<TenantMemberEntity> AssigneeMembers => _assigneeMembers.AsReadOnly();

        private readonly List<TaskCommentEntity> _comments = new List<TaskCommentEntity>();
        public virtual ICollection<TaskCommentEntity> Comments => _comments.AsReadOnly();

        private readonly List<TaskAttachmentEntity> _attachments = new List<TaskAttachmentEntity>();
        public virtual ICollection<TaskAttachmentEntity> Attachments => _attachments.AsReadOnly();

        protected TaskEntity() { }

        public TaskEntity(string title, string description, string content, int order, ProjectBoardColumnEntity initialColumn, TenantEntity tenant, DateTime? dueDate = null)
            : base(tenant)
        {
            if (string.IsNullOrWhiteSpace(title))
                throw new ArgumentException("Task title cannot be empty.");

            Title = title;
            Description = description;
            Content = content;
            Order = order;
            BoardColumn = initialColumn;
            BoardColumnId = initialColumn.Id;
            DueDate = dueDate;
            CreatedAt = DateTime.UtcNow;
        }

        #region Task Management Methods



        public void ChangeOrder(int newOrder)
        {
            if (newOrder < 0)
                throw new ArgumentException("Order must be a positive number.");

            Order = newOrder;
            UpdatedAt = DateTime.UtcNow;
        }

        public void MoveToColumn(ProjectBoardColumnEntity newColumnEntity)
        {
            if (newColumnEntity == null)
                throw new ArgumentException("Target column cannot be null.");

            BoardColumn = newColumnEntity;
            BoardColumnId = newColumnEntity.Id;
            UpdatedAt = DateTime.UtcNow;
        }

        public void SetDueDate(DateTime dueDate)
        {
            if (dueDate < DateTime.UtcNow)
                throw new ArgumentException("Due date cannot be in the past.");

            DueDate = dueDate;
            UpdatedAt = DateTime.UtcNow;
        }

        public void RemoveDueDate()
        {
            DueDate = null;
            UpdatedAt = DateTime.UtcNow;
        }

        public void UpdateContent(string newContent)
        {
            if (string.IsNullOrWhiteSpace(newContent))
                throw new ArgumentException("Task content cannot be empty.");

            Content = newContent;
            UpdatedAt = DateTime.UtcNow;
        }

        #endregion

        #region Label Management Methods

        public void AddLabel(TaskLabelEntity labelEntity)
        {
            if (labelEntity == null || _labels.Contains(labelEntity))
                return;

            _labels.Add(labelEntity);
            UpdatedAt = DateTime.UtcNow;
        }

        public void RemoveLabel(TaskLabelEntity labelEntity)
        {
            if (labelEntity == null || !_labels.Contains(labelEntity))
                return;

            _labels.Remove(labelEntity);
            UpdatedAt = DateTime.UtcNow;
        }

        #endregion

        #region Assignee Management Methods

        public void AssignUser(TenantMemberEntity user)
        {
            if (user == null || _assigneeMembers.Contains(user))
                return;

            _assigneeMembers.Add(user);
            UpdatedAt = DateTime.UtcNow;
        }

        public void UnassignUser(TenantMemberEntity user)
        {
            if (user == null || !_assigneeMembers.Contains(user))
                return;

            _assigneeMembers.Remove(user);
            UpdatedAt = DateTime.UtcNow;
        }

        #endregion

        #region Comment Management Methods

        public void AddComment(Guid userId, string commentContent)
        {
            if (string.IsNullOrWhiteSpace(commentContent))
                throw new ArgumentException("Comment content cannot be empty.");

            var comment = new TaskCommentEntity(userId, commentContent);
            _comments.Add(comment);
            UpdatedAt = DateTime.UtcNow;
        }

        public void UpdateComment(Guid commentId, string newContent)
        {
            var comment = _comments.FirstOrDefault(c => c.Id == commentId);
            if (comment == null)
                throw new InvalidOperationException("Comment not found.");

            comment.UpdateContent(newContent);
            UpdatedAt = DateTime.UtcNow;
        }

        public void DeleteComment(Guid commentId)
        {
            var comment = _comments.FirstOrDefault(c => c.Id == commentId);
            if (comment == null)
                throw new InvalidOperationException("Comment not found.");

            _comments.Remove(comment);
            UpdatedAt = DateTime.UtcNow;
        }

        #endregion

        #region Attachment Management Methods

        public void AddAttachment(string fileName, string filePath)
        {
            if (string.IsNullOrWhiteSpace(fileName) || string.IsNullOrWhiteSpace(filePath))
                throw new ArgumentException("File name and path cannot be empty.");

            var attachment = new TaskAttachmentEntity(fileName, filePath);
            _attachments.Add(attachment);
            UpdatedAt = DateTime.UtcNow;
        }

        public void RemoveAttachment(Guid attachmentId)
        {
            var attachment = _attachments.FirstOrDefault(a => a.Id == attachmentId);
            if (attachment == null)
                throw new InvalidOperationException("Attachment not found.");

            _attachments.Remove(attachment);
            UpdatedAt = DateTime.UtcNow;
        }

        #endregion
    }
}
