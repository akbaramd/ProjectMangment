using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using PMS.Domain.Entities;
using SharedKernel.DomainDrivenDesign.Domain;

namespace PMS.Infrastructure.Data;

public class ApplicationDbContext : IdentityDbContext<ApplicationUser, IdentityRole<Guid>, Guid>
{
    public DbSet<TenantRole> TenantRole { get; set; }
    public DbSet<Project> Projects { get; set; }
    public DbSet<ProjectMember> ProjectsMembers { get; set; }
    public DbSet<Sprint> Sprints { get; set; }
    public DbSet<BoardColumn> BoardColumns { get; set; }
    public DbSet<SprintTask> SprintTasks { get; set; }
    public DbSet<Board> Boards { get; set; }
    public DbSet<ApplicationPermission> Permissions { get; set; }
    public DbSet<ApplicationPermissionGroup> PermissionGroups { get; set; }
    public DbSet<Tenant> Tenants { get; set; }
    public DbSet<TenantMember> TenantMember { get; set; }
    public DbSet<Invitation> Invitations { get; set; }
    public DbSet<TaskComment> TaskComments { get; set; }
    public DbSet<TaskLabel> TaskLabels { get; set; }
    public DbSet<TaskAttachment> TaskAttachments { get; set; }

    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
    {
    }

    private void ConfigureIdentityTables(ModelBuilder builder)
    {
        builder.Entity<IdentityUserRole<Guid>>(entity =>
        {
            entity.ToTable("UserRoles"); // Renaming AspNetUserRoles to UserRoles
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
        
        builder.Entity<ProjectMember>(entity =>
        {
            entity.ToTable("ProjectMembers");
            entity.HasOne(x => x.Project).WithMany(x=>x.Members).HasForeignKey(x => x.ProjectId);
            entity.Property(x => x.Access)
                .HasConversion(c => c.Name.ToString(),
                    v => Enumeration.FromName<ProjectMemberAccess>(v)); // Renaming AspNetUserTokens to UserTokensl
        });
        
        
        builder.Entity<Project>(entity =>
        {
            entity.HasMany(x => x.Members).WithOne(x=>x.Project).HasForeignKey(x => x.ProjectId);
        
        });
    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        ConfigureIdentityTables(builder);
        // سایر تنظیمات
        builder.ApplyConfigurationsFromAssembly(typeof(ApplicationDbContext).Assembly);
    }
}
