using PMS.Domain.Core;
using SharedKernel.DomainDrivenDesign.Domain;
using System;
using System.Collections.Generic;

namespace PMS.Domain.Entities
{
    public class SprintTask : Entity<Guid>
    {
        public string Title { get; private set; }
        public string Description { get; private set; }
        public string Content { get; private set; }
        public SprintTaskStatus Status { get; private set; }
        public int Order { get; private set; }
        public DateTime? DueDate { get; private set; }

        // Relations
        public Guid BoardColumnId { get; private set; }
        public BoardColumn BoardColumn { get; private set; }

        public ICollection<TaskLabel> Labels { get; private set; } = new List<TaskLabel>();
        public ICollection<TenantMember> AssigneeMembers { get; private set; } = new List<TenantMember>();
        public ICollection<TaskComment> Comments { get; private set; } = new List<TaskComment>();
        public ICollection<TaskAttachment> Attachments { get; private set; } = new List<TaskAttachment>();

        protected SprintTask() { }

        public SprintTask(string title, string description, string content, int order, BoardColumn initialColumn, DateTime? dueDate = null)
        {
            Title = title;
            Description = description;
            Content = content;
            Status = SprintTaskStatus.ToDo;
            Order = order;
            BoardColumn = initialColumn;
            DueDate = dueDate;
        }

        #region Task Management Methods

        public void ChangeStatus(SprintTaskStatus newStatus)
        {
            Status = newStatus;
        }

        public void ChangeOrder(int newOrder)
        {
            Order = newOrder;
        }

        public void MoveToColumn(BoardColumn newColumn)
        {
            BoardColumn = newColumn;
            BoardColumnId = newColumn.Id;
        }

        public void SetDueDate(DateTime dueDate)
        {
            DueDate = dueDate;
        }

        public void RemoveDueDate()
        {
            DueDate = null;
        }

        #endregion

        #region Label Management Methods

        public void AddLabel(TaskLabel label)
        {
            if (!Labels.Contains(label))
            {
                Labels.Add(label);
            }
        }

        public void RemoveLabel(TaskLabel label)
        {
            if (Labels.Contains(label))
            {
                Labels.Remove(label);
            }
        }

        #endregion

        #region Assignee Management Methods

        public void AssignUser(TenantMember userId)
        {
            if (!AssigneeMembers.Contains(userId))
            {
                AssigneeMembers.Add(userId);
            }
        }

        public void UnassignUser(TenantMember userId)
        {
            if (AssigneeMembers.Contains(userId))
            {
                AssigneeMembers.Remove(userId);
            }
        }

        #endregion

        #region Comment Management Methods

        public void AddComment(Guid userId, string commentContent)
        {
            var comment = new TaskComment(userId, commentContent);
            Comments.Add(comment);
        }

        public void UpdateComment(Guid commentId, string newContent)
        {
            var comment = Comments.FirstOrDefault(c => c.Id == commentId);
            if (comment == null)
            {
                throw new InvalidOperationException("Comment not found.");
            }

            comment.UpdateContent(newContent);
        }

        public void DeleteComment(Guid commentId)
        {
            var comment = Comments.FirstOrDefault(c => c.Id == commentId);
            if (comment == null)
            {
                throw new InvalidOperationException("Comment not found.");
            }

            Comments.Remove(comment);
        }

        #endregion

        #region Attachment Management Methods

        public void AddAttachment(string fileName, string filePath)
        {
            var attachment = new TaskAttachment(fileName, filePath);
            Attachments.Add(attachment);
        }

        public void RemoveAttachment(Guid attachmentId)
        {
            var attachment = Attachments.FirstOrDefault(a => a.Id == attachmentId);
            if (attachment == null)
            {
                throw new InvalidOperationException("Attachment not found.");
            }

            Attachments.Remove(attachment);
        }

        #endregion
    }
}
