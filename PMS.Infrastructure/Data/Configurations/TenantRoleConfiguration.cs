using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PMS.Domain.BoundedContexts.TenantManagement;

namespace PMS.Infrastructure.Data.Configurations
{
    public class TenantRoleConfiguration : IEntityTypeConfiguration<TenantRoleEntity>
    {
        public void Configure(EntityTypeBuilder<TenantRoleEntity> builder)
        {
            builder.Property(x => x.Key).HasColumnName("Key");
            builder.HasOne(x => x.Tenant)
                   .WithMany(c => c.Roles)
                   .HasForeignKey(x => x.TenantId)
                   .IsRequired(false);
            builder.ToTable("TenantRoles");
        }
    }
}
