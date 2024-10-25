using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PMS.Domain.BoundedContexts.AttachmentManagement;

namespace PMS.Infrastructure.Data.Configurations;

public class AttachmentConfiguration : IEntityTypeConfiguration<AttachmentEntity>
{
    public void Configure(EntityTypeBuilder<AttachmentEntity> builder)
    {
        builder.HasKey(ta => ta.Id);
        builder.Property(ta => ta.FileName).IsRequired().HasMaxLength(255);
        builder.Property(ta => ta.FilePath).IsRequired().HasMaxLength(1000);
    }
}