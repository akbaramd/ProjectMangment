using PMS.Domain.BoundedContexts.TenantManagment;
using PMS.Domain.Core;
using SharedKernel.DomainDrivenDesign.Domain;

namespace PMS.Domain.BoundedContexts.AttachmentManagement
{
    public class AttachmentEntity : TenantAggregateRootBase
    {
        public string FileName { get; private set; }
        public string FilePath { get; private set; }
        public string Description { get; private set; }

        public virtual  Guid TenantMemberId { get; set; }
        public virtual TenantMemberEntity TenantMember { get; set; }
        
        public virtual AttachmentCategoryEntity? Category { get; private set; }
        public Guid? CategoryId { get; private set; }

        protected AttachmentEntity() { }

        public AttachmentEntity(TenantEntity entity,string fileName, string filePath ,AttachmentCategoryEntity? categoryy = null):base(entity)
        {
            FileName = fileName;
            FilePath = filePath;
            Category = categoryy;
        }
    }
    
   
}
