using Bonyan.Layer.Domain.Enumerations;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PMS.Domain.BoundedContexts.TenantManagement;

namespace PMS.Infrastructure.Data.Configurations;

public class InvitationConfiguration : IEntityTypeConfiguration<ProjectInvitationEntity>
{
    public void Configure(EntityTypeBuilder<ProjectInvitationEntity> builder)
    {
        builder.HasKey(i => i.Id);

        builder.Property(i => i.PhoneNumber)
            .IsRequired()
            .HasMaxLength(200);

        builder.Property(i => i.Status)
            .HasConversion<string>()
            .IsRequired();

        builder.Property(i => i.SentAt)
            .IsRequired();

       
        builder.Property(x => x.Status)
            .HasConversion(x => x.Name, c => Enumeration.FromName<InvitationStatus>(c));
    }
}