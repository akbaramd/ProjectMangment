using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PMS.Domain.BoundedContexts.TaskManagment;

namespace PMS.Infrastructure.Data.Configurations;

public class TaskAttachmentConfiguration : IEntityTypeConfiguration<TaskAttachmentEntity>
{
    public void Configure(EntityTypeBuilder<TaskAttachmentEntity> builder)
    {
        builder.HasKey(ta => ta.Id);
    }
}