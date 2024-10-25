using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PMS.Domain.BoundedContexts.TaskManagment;

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

        builder.Property(st => st.Content)
            .HasMaxLength(2000);



        builder.Property(st => st.Order)
            .IsRequired();

        builder.HasOne(st => st.BoardColumn)
            .WithMany(bc => bc.Tasks)
            .HasForeignKey(st => st.BoardColumnId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasMany(st => st.Labels)
            .WithOne()
            .HasForeignKey(l => l.SprintTaskId);

        builder.HasMany(st => st.Comments)
            .WithOne()
            .HasForeignKey(c => c.SprintTaskId);

        builder.HasMany(st => st.Attachments)
            .WithOne()
            .HasForeignKey(a => a.SprintTaskId);

        builder.HasMany(st => st.AssigneeMembers)
            .WithMany(am => am.Tasks)
            .UsingEntity(
                "SprintTaskAssignee"
            );

        builder.ToTable("SprintTasks");
    }
}
