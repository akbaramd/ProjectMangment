using PMS.Domain.Core;

namespace PMS.Domain.BoundedContexts.AttachmentManagement
{
    public class AttachmentCategoryEntity : TenantEntityBase
    {
        public string Title { get; private set; }

        public virtual ICollection<AttachmentEntity> Attachments { get; set; } = new List<AttachmentEntity>();
        protected AttachmentCategoryEntity() { }

        public AttachmentCategoryEntity(string title)
        {
            Title = title;
        }
    }
}
