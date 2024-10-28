using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PMS.Domain.BoundedContexts.TenantManagement;

namespace PMS.Infrastructure.Data.Configurations;

public class TenantConfiguration : IEntityTypeConfiguration<TenantInfoEntity>
{
    public void Configure(EntityTypeBuilder<TenantInfoEntity> builder)
    {
       
    }
}