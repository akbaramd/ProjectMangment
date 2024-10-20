using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PMS.Domain.Entities;

namespace PMS.Infrastructure.Data.Configurations;

public class TaskAttachmentConfiguration : IEntityTypeConfiguration<TaskAttachment>
{
    public void Configure(EntityTypeBuilder<TaskAttachment> builder)
    {
        builder.HasKey(ta => ta.Id);
        builder.Property(ta => ta.FileName).IsRequired().HasMaxLength(255);
        builder.Property(ta => ta.FilePath).IsRequired().HasMaxLength(1000);
        builder.ToTable("TaskAttachments");
    }
}
