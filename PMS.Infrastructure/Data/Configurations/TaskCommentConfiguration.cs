using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PMS.Domain.Entities;

namespace PMS.Infrastructure.Data.Configurations;

public class TaskCommentConfiguration : IEntityTypeConfiguration<TaskComment>
{
    public void Configure(EntityTypeBuilder<TaskComment> builder)
    {
        builder.HasKey(tc => tc.Id);
        builder.Property(tc => tc.Content).IsRequired().HasMaxLength(1000);
        builder.Property(tc => tc.CreatedAt).IsRequired();
        builder.HasOne<ApplicationUser>().WithMany().HasForeignKey(tc => tc.UserId);
        builder.ToTable("TaskComments");
    }
}
