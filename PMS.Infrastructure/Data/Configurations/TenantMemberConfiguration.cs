using Bonyan.Layer.Domain.Enumerations;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PMS.Domain.BoundedContexts.TenantManagement;

namespace PMS.Infrastructure.Data.Configurations;

public class TenantMemberConfiguration : IEntityTypeConfiguration<TenantMemberEntity>
{
    public void Configure(EntityTypeBuilder<TenantMemberEntity> builder)
    {
        builder.Property(ut => ut.Status)
            .HasConversion<string>()
            .IsRequired();


        // Relationship with UserEntity
        builder.HasOne(ut => ut.UserEntity)
            .WithMany(u => u.UserTenants)
            .HasForeignKey(ut => ut.UserId)
            .OnDelete(DeleteBehavior.Cascade);

        // One-to-Many relationship: Tenants has many UserTenants
        builder.HasMany(t => t.Roles)
            .WithMany(ut => ut.Members)
            .UsingEntity("TenantMemberRoles");
        builder.HasMany(t => t.ProjectMembers)
            .WithOne(ut => ut.TenantMember)
            .HasForeignKey(x => x.TenantMemberId);

        builder.Property(x => x.Status)
            .HasConversion(x => x.Name, c => Enumeration.FromName<TenantMemberStatus>(c));
    }
}