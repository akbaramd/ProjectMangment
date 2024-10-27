using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PMS.Domain.BoundedContexts.TaskManagement.Tasks;

namespace PMS.Infrastructure.Data.Configurations;

public class TaskLabelConfiguration : IEntityTypeConfiguration<TaskLabelEntity>
{
    public void Configure(EntityTypeBuilder<TaskLabelEntity> builder)
    {
        builder.HasKey(tl => tl.Id);
        builder.Property(tl => tl.Name).IsRequired().HasMaxLength(200);
        builder.ToTable("TaskLabels");
    }
}
