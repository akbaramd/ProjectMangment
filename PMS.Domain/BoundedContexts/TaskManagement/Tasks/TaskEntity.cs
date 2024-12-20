using PMS.Domain.BoundedContexts.AttachmentManagement;
using PMS.Domain.BoundedContexts.ProjectManagement.Projects;
using PMS.Domain.BoundedContexts.TaskManagement.Kanban;
using PMS.Domain.Core;

namespace PMS.Domain.BoundedContexts.TaskManagement.Tasks
{
    public class TaskEntity : TenantAggregateRootBase
    {
        public string Title { get; private set; }
        public string Description { get; private set; }

        public int Order { get; private set; }
        public DateTime? DueDate { get; private set; }
        public DateTime CreatedAt { get; private set; }
        public DateTime? UpdatedAt { get; private set; }

        // Relations
        
        public Guid BoardColumnId { get; private set; }
        public virtual KanbanBoardColumnEntity BoardColumn { get; private set; }

        private readonly List<TaskLabelEntity> _labels = new List<TaskLabelEntity>();
        public virtual ICollection<TaskLabelEntity> Labels => _labels.AsReadOnly();

        private readonly List<ProjectMemberEntity> _assigneeMembers = new List<ProjectMemberEntity>();
        public virtual ICollection<ProjectMemberEntity> AssigneeMembers => _assigneeMembers.AsReadOnly();

        private readonly List<TaskCommentEntity> _comments = new List<TaskCommentEntity>();
        public virtual ICollection<TaskCommentEntity> Comments => _comments.AsReadOnly();

        private readonly List<TaskAttachmentEntity> _attachments = new List<TaskAttachmentEntity>();
        public virtual ICollection<TaskAttachmentEntity> Attachments => _attachments.AsReadOnly();

        protected TaskEntity() { }

        public TaskEntity(string title, string description, int order, KanbanBoardColumnEntity initialColumn,  DateTime? dueDate = null)
           
        {
            if (string.IsNullOrWhiteSpace(title))
                throw new ArgumentException("Task title cannot be empty.");

            Title = title;
            Description = description;
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

        public void MoveToColumn(KanbanBoardColumnEntity newColumnEntity)
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

        public void AssignUser(ProjectMemberEntity user)
        {
            if (user == null || _assigneeMembers.Contains(user))
                return;

            _assigneeMembers.Add(user);
            UpdatedAt = DateTime.UtcNow;
        }

        public void UnassignUser(ProjectMemberEntity user)
        {
            if (user == null || !_assigneeMembers.Contains(user))
                return;

            _assigneeMembers.Remove(user);
            UpdatedAt = DateTime.UtcNow;
        }

        #endregion

        #region Comment Management Methods

        public void AddComment(ProjectMemberEntity member, string commentContent)
        {
            if (string.IsNullOrWhiteSpace(commentContent))
                throw new ArgumentException("Comment content cannot be empty.");

            var comment = new TaskCommentEntity(this,member, commentContent);
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

        public void AddAttachment(ProjectMemberEntity member,AttachmentEntity attachment)
        {
            var taskAttachmentEntity = new TaskAttachmentEntity(this,member,attachment);
            _attachments.Add(taskAttachmentEntity);
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

        public void UpdateTitle(string title)
        {
            Title = title;
            UpdatedAt = DateTime.UtcNow;
        }

        public void UpdateDescription(string description)
        {
            Description = description;
            UpdatedAt = DateTime.UtcNow;
        }

        public void UpdateDueDate(DateTime value)
        {
            DueDate = value;
            UpdatedAt = DateTime.UtcNow;
        }

        public void UpdateOrder(int value)
        {
            Order = value;
            UpdatedAt = DateTime.UtcNow;
        }


        #endregion
    }
}
