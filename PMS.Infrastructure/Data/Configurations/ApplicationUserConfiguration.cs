using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PMS.Domain.BoundedContexts.UserManagment;

namespace PMS.Infrastructure.Data.Configurations;

public class ApplicationUserConfiguration : IEntityTypeConfiguration<ApplicationUser>
{
    public void Configure(EntityTypeBuilder<ApplicationUser> builder)
    {
        builder.HasKey(u => u.Id);

        builder.Property(u => u.FullName)
            .IsRequired()
            .HasMaxLength(150);

        builder.Property(u => u.Status)
            .HasConversion<string>()
            .IsRequired();

        // One-to-Many relationship: User can belong to many UserTenants
        builder.HasMany(u => u.UserTenants)
            .WithOne(ut => ut.User)
            .HasForeignKey(ut => ut.UserId)
            .OnDelete(DeleteBehavior.Cascade); // Cascade delete when User is deleted

        builder.ToTable("Users");
    }
}