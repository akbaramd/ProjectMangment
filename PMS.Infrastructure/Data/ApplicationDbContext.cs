using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using PMS.Domain.BoundedContexts.AttachmentManagement;
using PMS.Domain.BoundedContexts.ProjectManagement;
using PMS.Domain.BoundedContexts.ProjectManagement.Projects;
using PMS.Domain.BoundedContexts.ProjectManagement.Projects.Enums;
using PMS.Domain.BoundedContexts.TaskManagement.Kanban;
using PMS.Domain.BoundedContexts.TaskManagement.Tasks;
using PMS.Domain.BoundedContexts.TaskManagement;
using PMS.Domain.BoundedContexts.TaskManagement.Kanban.DomainEvents;
using PMS.Domain.BoundedContexts.TenantManagment;
using PMS.Domain.BoundedContexts.UserManagment;
using SharedKernel.DomainDrivenDesign.Domain;

namespace PMS.Infrastructure.Data;

public class ApplicationDbContext : IdentityDbContext<ApplicationUser, IdentityRole<Guid>, Guid>
{
    public DbSet<AttachmentEntity> Attachments { get; set; }
    public DbSet<AttachmentCategoryEntity> AttachmentCategories { get; set; }
    public DbSet<TenantRoleEntity> TenantRole { get; set; }
    public DbSet<TenantMemberEntity> TenantMember { get; set; }
    public DbSet<ProjectInvitationEntity> TenantInvitations { get; set; }
    public DbSet<ProjectEntity> Projects { get; set; }
    public DbSet<ProjectMemberEntity> ProjectsMembers { get; set; }
    public DbSet<ProjectSprintEntity> ProjectSprints { get; set; }
    public DbSet<KanbanBoardColumnEntity> KanbanBoardColumns { get; set; }
    public DbSet<KanbanBoardEntity> KanbanBoards { get; set; }
    public DbSet<TaskEntity> Tasks { get; set; }
    public DbSet<TaskAttachmentEntity> TasksAttachments { get; set; }
    public DbSet<TenantPermissionEntity> Permissions { get; set; }
    public DbSet<TenantPermissionGroupEntity> PermissionGroups { get; set; }
    public DbSet<TenantEntity> Tenants { get; set; }
    public DbSet<TaskCommentEntity> TaskComments { get; set; }
    public DbSet<TaskLabelEntity> TaskLabels { get; set; }

    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
    {
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseLazyLoadingProxies();
        base.OnConfiguring(optionsBuilder);
    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        ConfigureIdentityTables(builder);
        
        builder.Entity<ProjectMemberEntity>(entity =>
        {
            entity.HasOne(x => x.Project).WithMany(x=>x.Members).HasForeignKey(x => x.ProjectId);
            entity.Property(x => x.AccessEnum)
                .HasConversion(c => c.Name.ToString(),
                    v => Enumeration.FromName<ProjectMemberAccessEnum>(v)); // Renaming AspNetUserTokens to UserTokensl
        });
     
    
        builder.Entity<KanbanBoardEntity>(entity =>
        {
            entity.HasMany(x => x.Columns).WithOne().HasForeignKey(x => x.BoardId);
            
        });
        builder.Entity<ProjectEntity>(entity =>
        {
            entity.HasMany(x => x.Members).WithOne(x=>x.Project).HasForeignKey(x => x.ProjectId);
            
        });
        
        builder.ApplyConfigurationsFromAssembly(typeof(ApplicationDbContext).Assembly);
    } 
    private void ConfigureIdentityTables(ModelBuilder builder)
         {
             builder.Entity<IdentityRole<Guid>>(entity =>
             {
                 entity.ToTable("Roles"); // Renaming AspNetUserRoles to UserRoles
             });
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
             
             
         }
}
