using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PMS.Domain.BoundedContexts.TaskManagement.Tasks;

namespace PMS.Infrastructure.Data.Configurations;

public class SprintTaskConfiguration : IEntityTypeConfiguration<TaskEntity>
{
    public void Configure(EntityTypeBuilder<TaskEntity> builder)
    {
        builder.HasKey(st => st.Id);

        builder.Property(st => st.Title)
            .IsRequired()
            .HasMaxLength(200);

        builder.Property(st => st.Description)
            .HasMaxLength(1000);


        builder.Property(st => st.Order)
            .IsRequired();

        builder.HasOne(st => st.BoardColumn)
            .WithMany()
            .HasForeignKey(st => st.BoardColumnId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasMany(st => st.Labels)
            .WithOne()
            .HasForeignKey(l => l.SprintTaskId);

        builder.HasMany(st => st.Comments)
            .WithOne(x=>x.Task)
            .HasForeignKey(c => c.TaskId);

        builder.HasMany(st => st.Attachments)
            .WithOne(c=>c.Task)
            .HasForeignKey(a => a.TaskId);

        builder.HasMany(st => st.AssigneeMembers)
            .WithMany(x=>x.Tasks)
            .UsingEntity(
                "TasksAssignee"
            );

        builder.ToTable("SprintTasks");
    }
}
