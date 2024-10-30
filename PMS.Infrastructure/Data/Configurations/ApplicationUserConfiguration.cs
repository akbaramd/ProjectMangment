using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PMS.Domain.BoundedContexts.UserManagment;

namespace PMS.Infrastructure.Data.Configurations;

public class ApplicationUserConfiguration : IEntityTypeConfiguration<UserEntity>
{
    public void Configure(EntityTypeBuilder<UserEntity> builder)
    {
        builder.HasKey(u => u.Id);

        builder.Property(u => u.FullName)
            .IsRequired()
            .HasMaxLength(150);

        builder.Property(u => u.Status)
            .HasConversion<string>()
            .IsRequired();

        // One-to-Many relationship: UserEntity can belong to many UserTenants
        builder.HasMany(u => u.UserTenants)
            .WithOne(ut => ut.UserEntity)
            .HasForeignKey(ut => ut.UserId)
            .OnDelete(DeleteBehavior.Cascade); // Cascade delete when UserEntity is deleted

        builder.ToTable("Users");
    }
}