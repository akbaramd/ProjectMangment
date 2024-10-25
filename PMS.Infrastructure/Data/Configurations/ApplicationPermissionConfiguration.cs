using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PMS.Domain.BoundedContexts.TenantManagment;

namespace PMS.Infrastructure.Data.Configurations
{
    public class ApplicationPermissionConfiguration : IEntityTypeConfiguration<TenantPermissionEntity>
    {
        public void Configure(EntityTypeBuilder<TenantPermissionEntity> builder)
        {
            builder.HasKey(x => x.Key);
            builder.ToTable("TenantPermissions");
            builder.HasMany(x => x.Roles)
                   .WithMany(c => c.Permissions)
                   .UsingEntity("RolePermissions");
        }
    }
}
