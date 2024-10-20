using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PMS.Domain.Entities;

namespace PMS.Infrastructure.Data.Configurations
{
    public class TenantRoleConfiguration : IEntityTypeConfiguration<TenantRole>
    {
        public void Configure(EntityTypeBuilder<TenantRole> builder)
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
