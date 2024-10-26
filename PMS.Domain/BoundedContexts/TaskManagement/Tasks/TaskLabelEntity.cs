using SharedKernel.DomainDrivenDesign.Domain;

namespace PMS.Domain.BoundedContexts.TaskManagement.Tasks
{
    public class TaskLabelEntity : Entity<Guid>
    {
        public string Name { get; private set; }
        public Guid SprintTaskId { get; private set; } // اضافه کردن این ویژگی

        protected TaskLabelEntity() { }

        public TaskLabelEntity(string name)
        {
            Name = name;
        }
    }
}
