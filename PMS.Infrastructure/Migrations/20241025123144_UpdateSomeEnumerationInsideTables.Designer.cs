﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using PMS.Infrastructure.Data;

#nullable disable

namespace PMS.Infrastructure.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    [Migration("20241025123144_UpdateSomeEnumerationInsideTables")]
    partial class UpdateSomeEnumerationInsideTables
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder.HasAnnotation("ProductVersion", "8.0.10");

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRole<System.Guid>", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("TEXT");

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken()
                        .HasColumnType("TEXT");

                    b.Property<string>("Name")
                        .HasMaxLength(256)
                        .HasColumnType("TEXT");

                    b.Property<string>("NormalizedName")
                        .HasMaxLength(256)
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.HasIndex("NormalizedName")
                        .IsUnique()
                        .HasDatabaseName("RoleNameIndex");

                    b.ToTable("Roles", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<System.Guid>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("ClaimType")
                        .HasColumnType("TEXT");

                    b.Property<string>("ClaimValue")
                        .HasColumnType("TEXT");

                    b.Property<Guid>("RoleId")
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.HasIndex("RoleId");

                    b.ToTable("RoleClaims", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<System.Guid>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("ClaimType")
                        .HasColumnType("TEXT");

                    b.Property<string>("ClaimValue")
                        .HasColumnType("TEXT");

                    b.Property<Guid>("UserId")
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("UserClaims", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<System.Guid>", b =>
                {
                    b.Property<string>("LoginProvider")
                        .HasColumnType("TEXT");

                    b.Property<string>("ProviderKey")
                        .HasColumnType("TEXT");

                    b.Property<string>("ProviderDisplayName")
                        .HasColumnType("TEXT");

                    b.Property<Guid>("UserId")
                        .HasColumnType("TEXT");

                    b.HasKey("LoginProvider", "ProviderKey");

                    b.HasIndex("UserId");

                    b.ToTable("UserLogins", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<System.Guid>", b =>
                {
                    b.Property<Guid>("UserId")
                        .HasColumnType("TEXT");

                    b.Property<Guid>("RoleId")
                        .HasColumnType("TEXT");

                    b.HasKey("UserId", "RoleId");

                    b.HasIndex("RoleId");

                    b.ToTable("UserRoles", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<System.Guid>", b =>
                {
                    b.Property<Guid>("UserId")
                        .HasColumnType("TEXT");

                    b.Property<string>("LoginProvider")
                        .HasColumnType("TEXT");

                    b.Property<string>("Name")
                        .HasColumnType("TEXT");

                    b.Property<string>("Value")
                        .HasColumnType("TEXT");

                    b.HasKey("UserId", "LoginProvider", "Name");

                    b.ToTable("UserTokens", (string)null);
                });

            modelBuilder.Entity("PMS.Domain.BoundedContexts.ProjectManagement.KanbanBoardColumnEntity", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("TEXT");

                    b.Property<Guid>("BoardId")
                        .HasColumnType("TEXT");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("TEXT");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<int>("Order")
                        .HasColumnType("INTEGER");

                    b.Property<Guid>("KanbanBoardId")
                        .HasColumnType("TEXT");

                    b.Property<Guid>("TenantId")
                        .HasColumnType("TEXT");

                    b.Property<DateTime?>("UpdatedAt")
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.HasIndex("KanbanBoardId");

                    b.HasIndex("TenantId");

                    b.ToTable("KanbanBoardColumns");
                });

            modelBuilder.Entity("PMS.Domain.BoundedContexts.ProjectManagement.KanbanBoardEntity", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("TEXT");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("TEXT");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<Guid>("ProjectSprintId")
                        .HasColumnType("TEXT");

                    b.Property<Guid>("SprintId")
                        .HasColumnType("TEXT");

                    b.Property<Guid>("TenantId")
                        .HasColumnType("TEXT");

                    b.Property<DateTime?>("UpdatedAt")
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.HasIndex("ProjectSprintId");

                    b.HasIndex("TenantId");

                    b.ToTable("KanbanBoards");
                });

            modelBuilder.Entity("PMS.Domain.BoundedContexts.ProjectManagement.ProjectEntity", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("TEXT");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("TEXT");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<DateTime?>("EndDate")
                        .HasColumnType("TEXT");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<DateTime>("StartDate")
                        .HasColumnType("TEXT");

                    b.Property<Guid>("TenantId")
                        .HasColumnType("TEXT");

                    b.Property<DateTime?>("UpdatedAt")
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.HasIndex("TenantId");

                    b.ToTable("Projects");
                });

            modelBuilder.Entity("PMS.Domain.BoundedContexts.ProjectManagement.ProjectMemberEntity", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("TEXT");

                    b.Property<string>("Access")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<Guid>("ProjectId")
                        .HasColumnType("TEXT");

                    b.Property<Guid>("TenantMemberId")
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.HasIndex("ProjectId");

                    b.HasIndex("TenantMemberId");

                    b.ToTable("ProjectsMembers");
                });

            modelBuilder.Entity("PMS.Domain.BoundedContexts.ProjectManagement.ProjectSprintEntity", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("TEXT");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("TEXT");

                    b.Property<DateTime>("EndDate")
                        .HasColumnType("TEXT");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<Guid>("ProjectId")
                        .HasColumnType("TEXT");

                    b.Property<DateTime>("StartDate")
                        .HasColumnType("TEXT");

                    b.Property<Guid>("TenantId")
                        .HasColumnType("TEXT");

                    b.Property<DateTime?>("UpdatedAt")
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.HasIndex("ProjectId");

                    b.HasIndex("TenantId");

                    b.ToTable("ProjectSprints");
                });

            modelBuilder.Entity("PMS.Domain.BoundedContexts.TaskManagement.TaskAttachmentEntity", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("TEXT");

                    b.Property<string>("FileName")
                        .IsRequired()
                        .HasMaxLength(255)
                        .HasColumnType("TEXT");

                    b.Property<string>("FilePath")
                        .IsRequired()
                        .HasMaxLength(1000)
                        .HasColumnType("TEXT");

                    b.Property<Guid>("SprintTaskId")
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.HasIndex("SprintTaskId");

                    b.ToTable("TaskAttachments", (string)null);
                });

            modelBuilder.Entity("PMS.Domain.BoundedContexts.TaskManagement.TaskCommentEntity", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("TEXT");

                    b.Property<string>("Content")
                        .IsRequired()
                        .HasMaxLength(1000)
                        .HasColumnType("TEXT");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("TEXT");

                    b.Property<Guid>("SprintTaskId")
                        .HasColumnType("TEXT");

                    b.Property<Guid>("UserId")
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.HasIndex("SprintTaskId");

                    b.HasIndex("UserId");

                    b.ToTable("TaskComments", (string)null);
                });

            modelBuilder.Entity("PMS.Domain.BoundedContexts.TaskManagement.TaskEntity", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("TEXT");

                    b.Property<Guid>("BoardColumnId")
                        .HasColumnType("TEXT");

                    b.Property<string>("Content")
                        .IsRequired()
                        .HasMaxLength(2000)
                        .HasColumnType("TEXT");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("TEXT");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasMaxLength(1000)
                        .HasColumnType("TEXT");

                    b.Property<DateTime?>("DueDate")
                        .HasColumnType("TEXT");

                    b.Property<int>("Order")
                        .HasColumnType("INTEGER");

                    b.Property<Guid>("TenantId")
                        .HasColumnType("TEXT");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasMaxLength(200)
                        .HasColumnType("TEXT");

                    b.Property<DateTime?>("UpdatedAt")
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.HasIndex("BoardColumnId");

                    b.HasIndex("TenantId");

                    b.ToTable("SprintTasks", (string)null);
                });

            modelBuilder.Entity("PMS.Domain.BoundedContexts.TaskManagement.TaskLabelEntity", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("TEXT");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(200)
                        .HasColumnType("TEXT");

                    b.Property<Guid>("SprintTaskId")
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.HasIndex("SprintTaskId");

                    b.ToTable("TaskLabels", (string)null);
                });

            modelBuilder.Entity("PMS.Domain.BoundedContexts.TenantManagment.ProjectInvitationEntity", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("TEXT");

                    b.Property<DateTime?>("AcceptedAt")
                        .HasColumnType("TEXT");

                    b.Property<DateTime?>("CanceledAt")
                        .HasColumnType("TEXT");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("TEXT");

                    b.Property<DateTime>("ExpirationDate")
                        .HasColumnType("TEXT");

                    b.Property<string>("PhoneNumber")
                        .IsRequired()
                        .HasMaxLength(200)
                        .HasColumnType("TEXT");

                    b.Property<DateTime>("SentAt")
                        .HasColumnType("TEXT");

                    b.Property<string>("Status")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<Guid>("TenantId")
                        .HasColumnType("TEXT");

                    b.Property<DateTime?>("UpdatedAt")
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.HasIndex("TenantId");

                    b.ToTable("TenantInvitations");
                });

            modelBuilder.Entity("PMS.Domain.BoundedContexts.TenantManagment.TenantEntity", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("TEXT");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(200)
                        .HasColumnType("TEXT");

                    b.Property<string>("Status")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("Subdomain")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.ToTable("Tenants");
                });

            modelBuilder.Entity("PMS.Domain.BoundedContexts.TenantManagment.TenantMemberEntity", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("TEXT");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("TEXT");

                    b.Property<Guid>("SprintTaskId")
                        .HasColumnType("TEXT");

                    b.Property<string>("Status")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<Guid>("TenantId")
                        .HasColumnType("TEXT");

                    b.Property<DateTime?>("UpdatedAt")
                        .HasColumnType("TEXT");

                    b.Property<Guid>("UserId")
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.HasIndex("TenantId");

                    b.HasIndex("UserId");

                    b.ToTable("TenantMember");
                });

            modelBuilder.Entity("PMS.Domain.BoundedContexts.TenantManagment.TenantPermissionEntity", b =>
                {
                    b.Property<string>("Key")
                        .HasColumnType("TEXT");

                    b.Property<string>("GroupKey")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("Key");

                    b.HasIndex("GroupKey");

                    b.ToTable("TenantPermissions", (string)null);
                });

            modelBuilder.Entity("PMS.Domain.BoundedContexts.TenantManagment.TenantPermissionGroupEntity", b =>
                {
                    b.Property<string>("Key")
                        .HasColumnType("TEXT");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("Key");

                    b.ToTable("TenantPermissionGroup", (string)null);
                });

            modelBuilder.Entity("PMS.Domain.BoundedContexts.TenantManagment.TenantRoleEntity", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("TEXT");

                    b.Property<bool>("Deletable")
                        .HasColumnType("INTEGER");

                    b.Property<bool>("IsSystemRole")
                        .HasColumnType("INTEGER");

                    b.Property<string>("Key")
                        .IsRequired()
                        .HasColumnType("TEXT")
                        .HasColumnName("Key");

                    b.Property<Guid?>("TenantId")
                        .HasColumnType("TEXT");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.HasIndex("TenantId");

                    b.ToTable("TenantRoles", (string)null);
                });

            modelBuilder.Entity("PMS.Domain.BoundedContexts.UserManagment.ApplicationUser", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("TEXT");

                    b.Property<int>("AccessFailedCount")
                        .HasColumnType("INTEGER");

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken()
                        .HasColumnType("TEXT");

                    b.Property<bool>("Deletable")
                        .HasColumnType("INTEGER");

                    b.Property<string>("Email")
                        .HasMaxLength(256)
                        .HasColumnType("TEXT");

                    b.Property<bool>("EmailConfirmed")
                        .HasColumnType("INTEGER");

                    b.Property<int>("FailedLoginAttempts")
                        .HasColumnType("INTEGER");

                    b.Property<string>("FullName")
                        .IsRequired()
                        .HasMaxLength(150)
                        .HasColumnType("TEXT");

                    b.Property<bool>("IsPhoneNumberConfirmed")
                        .HasColumnType("INTEGER");

                    b.Property<bool>("LockoutEnabled")
                        .HasColumnType("INTEGER");

                    b.Property<DateTimeOffset?>("LockoutEnd")
                        .HasColumnType("TEXT");

                    b.Property<DateTime?>("LockoutEndTime")
                        .HasColumnType("TEXT");

                    b.Property<string>("NormalizedEmail")
                        .HasMaxLength(256)
                        .HasColumnType("TEXT");

                    b.Property<string>("NormalizedUserName")
                        .HasMaxLength(256)
                        .HasColumnType("TEXT");

                    b.Property<string>("PasswordHash")
                        .HasColumnType("TEXT");

                    b.Property<string>("PhoneNumber")
                        .HasColumnType("TEXT");

                    b.Property<bool>("PhoneNumberConfirmed")
                        .HasColumnType("INTEGER");

                    b.Property<string>("RefreshToken")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<DateTime>("RefreshTokenExpiryTime")
                        .HasColumnType("TEXT");

                    b.Property<string>("SecurityStamp")
                        .HasColumnType("TEXT");

                    b.Property<string>("Status")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<bool>("TwoFactorEnabled")
                        .HasColumnType("INTEGER");

                    b.Property<string>("UserName")
                        .HasMaxLength(256)
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.HasIndex("NormalizedEmail")
                        .HasDatabaseName("EmailIndex");

                    b.HasIndex("NormalizedUserName")
                        .IsUnique()
                        .HasDatabaseName("UserNameIndex");

                    b.ToTable("Users", (string)null);
                });

            modelBuilder.Entity("RolePermissions", b =>
                {
                    b.Property<string>("PermissionsKey")
                        .HasColumnType("TEXT");

                    b.Property<Guid>("RolesId")
                        .HasColumnType("TEXT");

                    b.HasKey("PermissionsKey", "RolesId");

                    b.HasIndex("RolesId");

                    b.ToTable("RolePermissions");
                });

            modelBuilder.Entity("SprintTaskAssignee", b =>
                {
                    b.Property<Guid>("AssigneeMembersId")
                        .HasColumnType("TEXT");

                    b.Property<Guid>("TasksId")
                        .HasColumnType("TEXT");

                    b.HasKey("AssigneeMembersId", "TasksId");

                    b.HasIndex("TasksId");

                    b.ToTable("SprintTaskAssignee");
                });

            modelBuilder.Entity("TenantMemberRoles", b =>
                {
                    b.Property<Guid>("MembersId")
                        .HasColumnType("TEXT");

                    b.Property<Guid>("RolesId")
                        .HasColumnType("TEXT");

                    b.HasKey("MembersId", "RolesId");

                    b.HasIndex("RolesId");

                    b.ToTable("TenantMemberRoles");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<System.Guid>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityRole<System.Guid>", null)
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<System.Guid>", b =>
                {
                    b.HasOne("PMS.Domain.BoundedContexts.UserManagment.ApplicationUser", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<System.Guid>", b =>
                {
                    b.HasOne("PMS.Domain.BoundedContexts.UserManagment.ApplicationUser", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<System.Guid>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityRole<System.Guid>", null)
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("PMS.Domain.BoundedContexts.UserManagment.ApplicationUser", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<System.Guid>", b =>
                {
                    b.HasOne("PMS.Domain.BoundedContexts.UserManagment.ApplicationUser", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("PMS.Domain.BoundedContexts.ProjectManagement.KanbanBoardColumnEntity", b =>
                {
                    b.HasOne("PMS.Domain.BoundedContexts.ProjectManagement.KanbanBoardEntity", "KanbanBoard")
                        .WithMany("Columns")
                        .HasForeignKey("KanbanBoardId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("PMS.Domain.BoundedContexts.TenantManagment.TenantEntity", "Tenant")
                        .WithMany()
                        .HasForeignKey("TenantId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("KanbanBoard");

                    b.Navigation("Tenant");
                });

            modelBuilder.Entity("PMS.Domain.BoundedContexts.ProjectManagement.KanbanBoardEntity", b =>
                {
                    b.HasOne("PMS.Domain.BoundedContexts.ProjectManagement.ProjectSprintEntity", "ProjectSprint")
                        .WithMany()
                        .HasForeignKey("ProjectSprintId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("PMS.Domain.BoundedContexts.TenantManagment.TenantEntity", "Tenant")
                        .WithMany()
                        .HasForeignKey("TenantId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("ProjectSprint");

                    b.Navigation("Tenant");
                });

            modelBuilder.Entity("PMS.Domain.BoundedContexts.ProjectManagement.ProjectEntity", b =>
                {
                    b.HasOne("PMS.Domain.BoundedContexts.TenantManagment.TenantEntity", "Tenant")
                        .WithMany()
                        .HasForeignKey("TenantId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Tenant");
                });

            modelBuilder.Entity("PMS.Domain.BoundedContexts.ProjectManagement.ProjectMemberEntity", b =>
                {
                    b.HasOne("PMS.Domain.BoundedContexts.ProjectManagement.ProjectEntity", "Project")
                        .WithMany("Members")
                        .HasForeignKey("ProjectId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("PMS.Domain.BoundedContexts.TenantManagment.TenantMemberEntity", "TenantMember")
                        .WithMany("ProjectMembers")
                        .HasForeignKey("TenantMemberId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Project");

                    b.Navigation("TenantMember");
                });

            modelBuilder.Entity("PMS.Domain.BoundedContexts.ProjectManagement.ProjectSprintEntity", b =>
                {
                    b.HasOne("PMS.Domain.BoundedContexts.ProjectManagement.ProjectEntity", "Project")
                        .WithMany()
                        .HasForeignKey("ProjectId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("PMS.Domain.BoundedContexts.TenantManagment.TenantEntity", "Tenant")
                        .WithMany()
                        .HasForeignKey("TenantId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Project");

                    b.Navigation("Tenant");
                });

            modelBuilder.Entity("PMS.Domain.BoundedContexts.TaskManagement.TaskAttachmentEntity", b =>
                {
                    b.HasOne("PMS.Domain.BoundedContexts.TaskManagement.TaskEntity", null)
                        .WithMany("Attachments")
                        .HasForeignKey("SprintTaskId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("PMS.Domain.BoundedContexts.TaskManagement.TaskCommentEntity", b =>
                {
                    b.HasOne("PMS.Domain.BoundedContexts.TaskManagement.TaskEntity", null)
                        .WithMany("Comments")
                        .HasForeignKey("SprintTaskId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("PMS.Domain.BoundedContexts.UserManagment.ApplicationUser", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("PMS.Domain.BoundedContexts.TaskManagement.TaskEntity", b =>
                {
                    b.HasOne("PMS.Domain.BoundedContexts.ProjectManagement.KanbanBoardColumnEntity", "KanbanBoardColumn")
                        .WithMany("Tasks")
                        .HasForeignKey("BoardColumnId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("PMS.Domain.BoundedContexts.TenantManagment.TenantEntity", "Tenant")
                        .WithMany()
                        .HasForeignKey("TenantId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("KanbanBoardColumn");

                    b.Navigation("Tenant");
                });

            modelBuilder.Entity("PMS.Domain.BoundedContexts.TaskManagement.TaskLabelEntity", b =>
                {
                    b.HasOne("PMS.Domain.BoundedContexts.TaskManagement.TaskEntity", null)
                        .WithMany("Labels")
                        .HasForeignKey("SprintTaskId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("PMS.Domain.BoundedContexts.TenantManagment.ProjectInvitationEntity", b =>
                {
                    b.HasOne("PMS.Domain.BoundedContexts.TenantManagment.TenantEntity", "Tenant")
                        .WithMany("Invitations")
                        .HasForeignKey("TenantId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Tenant");
                });

            modelBuilder.Entity("PMS.Domain.BoundedContexts.TenantManagment.TenantMemberEntity", b =>
                {
                    b.HasOne("PMS.Domain.BoundedContexts.TenantManagment.TenantEntity", "Tenant")
                        .WithMany("Users")
                        .HasForeignKey("TenantId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("PMS.Domain.BoundedContexts.UserManagment.ApplicationUser", "User")
                        .WithMany("UserTenants")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Tenant");

                    b.Navigation("User");
                });

            modelBuilder.Entity("PMS.Domain.BoundedContexts.TenantManagment.TenantPermissionEntity", b =>
                {
                    b.HasOne("PMS.Domain.BoundedContexts.TenantManagment.TenantPermissionGroupEntity", "Group")
                        .WithMany("Permissions")
                        .HasForeignKey("GroupKey")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Group");
                });

            modelBuilder.Entity("PMS.Domain.BoundedContexts.TenantManagment.TenantRoleEntity", b =>
                {
                    b.HasOne("PMS.Domain.BoundedContexts.TenantManagment.TenantEntity", "Tenant")
                        .WithMany("Roles")
                        .HasForeignKey("TenantId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.Navigation("Tenant");
                });

            modelBuilder.Entity("RolePermissions", b =>
                {
                    b.HasOne("PMS.Domain.BoundedContexts.TenantManagment.TenantPermissionEntity", null)
                        .WithMany()
                        .HasForeignKey("PermissionsKey")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("PMS.Domain.BoundedContexts.TenantManagment.TenantRoleEntity", null)
                        .WithMany()
                        .HasForeignKey("RolesId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("SprintTaskAssignee", b =>
                {
                    b.HasOne("PMS.Domain.BoundedContexts.TenantManagment.TenantMemberEntity", null)
                        .WithMany()
                        .HasForeignKey("AssigneeMembersId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("PMS.Domain.BoundedContexts.TaskManagement.TaskEntity", null)
                        .WithMany()
                        .HasForeignKey("TasksId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("TenantMemberRoles", b =>
                {
                    b.HasOne("PMS.Domain.BoundedContexts.TenantManagment.TenantMemberEntity", null)
                        .WithMany()
                        .HasForeignKey("MembersId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("PMS.Domain.BoundedContexts.TenantManagment.TenantRoleEntity", null)
                        .WithMany()
                        .HasForeignKey("RolesId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("PMS.Domain.BoundedContexts.ProjectManagement.KanbanBoardColumnEntity", b =>
                {
                    b.Navigation("Tasks");
                });

            modelBuilder.Entity("PMS.Domain.BoundedContexts.ProjectManagement.KanbanBoardEntity", b =>
                {
                    b.Navigation("Columns");
                });

            modelBuilder.Entity("PMS.Domain.BoundedContexts.ProjectManagement.ProjectEntity", b =>
                {
                    b.Navigation("Members");
                });

            modelBuilder.Entity("PMS.Domain.BoundedContexts.TaskManagement.TaskEntity", b =>
                {
                    b.Navigation("Attachments");

                    b.Navigation("Comments");

                    b.Navigation("Labels");
                });

            modelBuilder.Entity("PMS.Domain.BoundedContexts.TenantManagment.TenantEntity", b =>
                {
                    b.Navigation("Invitations");

                    b.Navigation("Roles");

                    b.Navigation("Users");
                });

            modelBuilder.Entity("PMS.Domain.BoundedContexts.TenantManagment.TenantMemberEntity", b =>
                {
                    b.Navigation("ProjectMembers");
                });

            modelBuilder.Entity("PMS.Domain.BoundedContexts.TenantManagment.TenantPermissionGroupEntity", b =>
                {
                    b.Navigation("Permissions");
                });

            modelBuilder.Entity("PMS.Domain.BoundedContexts.UserManagment.ApplicationUser", b =>
                {
                    b.Navigation("UserTenants");
                });
#pragma warning restore 612, 618
        }
    }
}
