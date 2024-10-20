using SharedKernel.DomainDrivenDesign.Domain;

namespace PMS.Domain.Entities
{
    public class TaskLabel : Entity<Guid>
    {
        public string Name { get; private set; }
        public Guid SprintTaskId { get; private set; } // اضافه کردن این ویژگی

        protected TaskLabel() { }

        public TaskLabel(string name)
        {
            Name = name;
        }
    }
}
