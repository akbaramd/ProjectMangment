using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PMS.Domain.BoundedContexts.TenantManagment;
using SharedKernel.DomainDrivenDesign.Domain;

namespace PMS.Infrastructure.Data.Configurations;

public class TenantConfiguration : IEntityTypeConfiguration<TenantEntity>
{
    public void Configure(EntityTypeBuilder<TenantEntity> builder)
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
        
        // One-to-Many relationship: Tenant has many UserTenants
        builder.HasMany(t => t.Roles)
            .WithOne(ut => ut.Tenant)
            .HasForeignKey(ut => ut.TenantId)
            .OnDelete(DeleteBehavior.Cascade); // Cascade delete when Tenant is deleted

        builder.Property(x => x.Status)
            .HasConversion(x => x.Name, c => Enumeration.ParseFromName<TenantrStatus>(c));
    }
}