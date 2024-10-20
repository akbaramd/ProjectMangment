using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PMS.Domain.Entities;

namespace PMS.Infrastructure.Data.Configurations
{
    public class ApplicationPermissionConfiguration : IEntityTypeConfiguration<ApplicationPermission>
    {
        public void Configure(EntityTypeBuilder<ApplicationPermission> builder)
        {
            builder.HasKey(x => x.Key);
            builder.ToTable("TenantPermissions");
            builder.HasMany(x => x.Roles)
                   .WithMany(c => c.Permissions)
                   .UsingEntity("RolePermissions");
        }
    }
}
