using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PMS.Domain.Entities;

public class InvitationConfiguration : IEntityTypeConfiguration<Invitation>
{
    public void Configure(EntityTypeBuilder<Invitation> builder)
    {
        builder.HasKey(i => i.Id);

        builder.Property(i => i.Email)
            .IsRequired()
            .HasMaxLength(200);

        builder.Property(i => i.Status)
            .HasConversion<string>()
            .IsRequired();

        builder.Property(i => i.SentAt)
            .IsRequired();

        // Relationship with Tenant
        builder.HasOne(i => i.Tenant)
            .WithMany(t => t.Invitations)
            .HasForeignKey(i => i.TenantId)
            .OnDelete(DeleteBehavior.Cascade); // Delete invitations if the tenant is deleted

        builder.ToTable("Invitations");
    }
}
