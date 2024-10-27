using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PMS.Domain.BoundedContexts.TaskManagement.Tasks;

namespace PMS.Infrastructure.Data.Configurations;

public class TaskCommentConfiguration : IEntityTypeConfiguration<TaskCommentEntity>
{
    public void Configure(EntityTypeBuilder<TaskCommentEntity> builder)
    {
        builder.HasKey(tc => tc.Id);
        builder.Property(tc => tc.Content).IsRequired().HasMaxLength(1000);
        builder.Property(tc => tc.CreatedAt).IsRequired();
        builder.HasOne(x=>x.ProjectMember).WithMany(x=>x.TaskComments).HasForeignKey(tc => tc.ProjectMemberId);
    }
}
