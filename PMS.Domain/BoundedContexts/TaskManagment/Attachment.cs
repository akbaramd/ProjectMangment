using SharedKernel.DomainDrivenDesign.Domain;

namespace PMS.Domain.BoundedContexts.TaskManagment
{
    public class TaskAttachmentEntity : Entity<Guid>
    {
        public string FileName { get; private set; }
        public string FilePath { get; private set; }
        public Guid SprintTaskId { get; private set; }

        protected TaskAttachmentEntity() { }

        public TaskAttachmentEntity(string fileName, string filePath)
        {
            FileName = fileName;
            FilePath = filePath;
        }
    }
}
