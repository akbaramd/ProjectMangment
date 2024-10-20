using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PMS.Domain.Entities;

namespace PMS.Infrastructure.Data.Configurations
{
    public class ApplicationPermissionGroupConfiguration : IEntityTypeConfiguration<ApplicationPermissionGroup>
    {
        public void Configure(EntityTypeBuilder<ApplicationPermissionGroup> builder)
        {
            builder.HasKey(x => x.Key);
            builder.ToTable("TenantPermissionGroup");
            builder.HasMany(x => x.Permissions)
                   .WithOne(x => x.Group)
                   .HasForeignKey(x => x.GroupKey);
        }
    }
}
