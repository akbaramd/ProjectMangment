using SharedKernel.DomainDrivenDesign.Domain;

namespace PMS.Domain.Entities
{
    public class TaskAttachment : Entity<Guid>
    {
        public string FileName { get; private set; }
        public string FilePath { get; private set; }
        public Guid SprintTaskId { get; private set; }

        protected TaskAttachment() { }

        public TaskAttachment(string fileName, string filePath)
        {
            FileName = fileName;
            FilePath = filePath;
        }
    }
}
