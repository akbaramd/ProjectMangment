using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using PMS.Domain.Entities;

namespace PMS.Infrastructure.Data;

public class ApplicationDbContext : IdentityDbContext<ApplicationUser, ApplicationRole, Guid>
{
    public DbSet<ApplicationPermission> Permissions { get; set; }
    public DbSet<ApplicationPermissionGroup> PermissionGroups { get; set; }
    public DbSet<Tenant> Tenants { get; set; }
    public DbSet<TenantMember> TenantMember { get; set; }
    public DbSet<Invitation> Invitations { get; set; }

    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        builder.Entity<IdentityUserRole<Guid>>(entity =>
        {
            entity.ToTable("UserRoles"); // Renaming AspNetUserRoles to UserRoles
        });

        builder.Entity<ApplicationRole>(entity =>
        {
            entity.Property(x => x.Name).HasColumnName("Key");
            entity.HasMany(x => x.Permissions).WithMany(c => c.Roles);
            entity.HasOne(x => x.Tenant).WithMany().HasForeignKey(x=>x.TenantId).IsRequired(false);
            entity.ToTable("Roles"); // Renaming AspNetRoles to Roles
        });

        builder.Entity<IdentityUserClaim<Guid>>(entity =>
        {
            entity.ToTable("UserClaims"); // Renaming AspNetUserClaims to UserClaims
        });

        builder.Entity<IdentityUserLogin<Guid>>(entity =>
        {
            entity.ToTable("UserLogins"); // Renaming AspNetUserLogins to UserLogins
        });

        builder.Entity<IdentityRoleClaim<Guid>>(entity =>
        {
            entity.ToTable("RoleClaims"); // Renaming AspNetRoleClaims to RoleClaims
        });

        builder.Entity<IdentityUserToken<Guid>>(entity =>
        {
            entity.ToTable("UserTokens"); // Renaming AspNetUserTokens to UserTokens
        });
        
        builder.Entity<ApplicationPermission>(entity =>
        {
            entity.HasKey(x => x.Key);
            entity.ToTable("Permissions"); // Renaming AspNetUserTokens to UserTokens
            entity.HasMany(x => x.Roles).WithMany(c => c.Permissions);
        });
        
        builder.Entity<ApplicationPermissionGroup>(entity =>
        {
            entity.HasKey(x => x.Key);
            entity.ToTable("PermissionGroup"); // Renaming AspNetUserTokens to UserTokens
            entity.HasMany(x => x.Permissions).WithOne().HasForeignKey(x => x.GroupKey);
        });

        // Apply all entity configurations
        builder.ApplyConfigurationsFromAssembly(typeof(ApplicationDbContext).Assembly);
    }
}