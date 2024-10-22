using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PMS.Domain.Entities;

namespace PMS.Infrastructure.Data.Configurations;

public class TenantMemberConfiguration : IEntityTypeConfiguration<TenantMember>
{
    public void Configure(EntityTypeBuilder<TenantMember> builder)
    {
        builder.Property(ut => ut.MemberStatus)
            .HasConversion<string>()
            .IsRequired();

        // Relationship with Tenant
        builder.HasOne(ut => ut.Tenant)
            .WithMany(t => t.Users)
            .HasForeignKey(ut => ut.TenantId)
            .OnDelete(DeleteBehavior.Cascade);

        // Relationship with User
        builder.HasOne(ut => ut.User)
            .WithMany(u => u.UserTenants)
            .HasForeignKey(ut => ut.UserId)
            .OnDelete(DeleteBehavior.Cascade);

        // One-to-Many relationship: Tenant has many UserTenants
        builder.HasMany(t => t.Roles)
            .WithMany(ut => ut.Members)
            .UsingEntity("TenantMemberRoles");
        builder.HasMany(t => t.ProjectMembers)
            .WithOne(ut => ut.TenantMember)
            .HasForeignKey(x => x.TenantMemberId);
        builder.ToTable("TenantMembers");
    }
}