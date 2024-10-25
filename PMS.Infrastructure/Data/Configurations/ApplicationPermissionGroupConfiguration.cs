using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PMS.Domain.BoundedContexts.TenantManagment;

namespace PMS.Infrastructure.Data.Configurations
{
    public class ApplicationPermissionGroupConfiguration : IEntityTypeConfiguration<TenantPermissionGroupEntity>
    {
        public void Configure(EntityTypeBuilder<TenantPermissionGroupEntity> builder)
        {
            builder.HasKey(x => x.Key);
            builder.ToTable("TenantPermissionGroup");
            builder.HasMany(x => x.Permissions)
                   .WithOne(x => x.Group)
                   .HasForeignKey(x => x.GroupKey);
        }
    }
}
