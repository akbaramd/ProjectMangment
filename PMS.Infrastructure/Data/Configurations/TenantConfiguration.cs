using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PMS.Domain.Entities;

namespace PMS.Infrastructure.Data.Configurations;

public class TenantConfiguration : IEntityTypeConfiguration<Tenant>
{
    public void Configure(EntityTypeBuilder<Tenant> builder)
    {
        builder.HasKey(t => t.Id);

        builder.Property(t => t.Name)
            .IsRequired()
            .HasMaxLength(200);

        builder.Property(t => t.Subdomain)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(t => t.Status)
            .HasConversion<string>()
            .IsRequired();

        // One-to-Many relationship: Tenant has many UserTenants
        builder.HasMany(t => t.Users)
            .WithOne(ut => ut.Tenant)
            .HasForeignKey(ut => ut.TenantId)
            .OnDelete(DeleteBehavior.Cascade); // Cascade delete when Tenant is deleted

        builder.ToTable("Tenants");
    }
}