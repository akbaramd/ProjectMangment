using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PMS.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class Init : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Projects",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    Name = table.Column<string>(type: "TEXT", nullable: false),
                    Description = table.Column<string>(type: "TEXT", nullable: false),
                    StartDate = table.Column<DateTime>(type: "TEXT", nullable: false),
                    EndDate = table.Column<DateTime>(type: "TEXT", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "TEXT", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "TEXT", nullable: true),
                    TenantId = table.Column<Guid>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Projects", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "TenantPermissionGroup",
                columns: table => new
                {
                    Key = table.Column<string>(type: "TEXT", nullable: false),
                    Name = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TenantPermissionGroup", x => x.Key);
                });

            migrationBuilder.CreateTable(
                name: "Tenants",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    Key = table.Column<string>(type: "TEXT", nullable: false),
                    Name = table.Column<string>(type: "TEXT", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "TEXT", nullable: false),
                    ModifiedDate = table.Column<DateTime>(type: "TEXT", nullable: true),
                    IsDeleted = table.Column<bool>(type: "INTEGER", nullable: false, defaultValue: false),
                    DeletedDate = table.Column<DateTime>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tenants", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    FullName = table.Column<string>(type: "TEXT", maxLength: 150, nullable: false),
                    Deletable = table.Column<bool>(type: "INTEGER", nullable: false),
                    IsPhoneNumberConfirmed = table.Column<bool>(type: "INTEGER", nullable: false),
                    Status = table.Column<string>(type: "TEXT", nullable: false),
                    RefreshToken = table.Column<string>(type: "TEXT", nullable: false),
                    RefreshTokenExpiryTime = table.Column<DateTime>(type: "TEXT", nullable: false),
                    FailedLoginAttempts = table.Column<int>(type: "INTEGER", nullable: false),
                    LockoutEndTime = table.Column<DateTime>(type: "TEXT", nullable: true),
                    UserName = table.Column<string>(type: "TEXT", nullable: true),
                    NormalizedUserName = table.Column<string>(type: "TEXT", nullable: true),
                    Email = table.Column<string>(type: "TEXT", nullable: true),
                    NormalizedEmail = table.Column<string>(type: "TEXT", nullable: true),
                    EmailConfirmed = table.Column<bool>(type: "INTEGER", nullable: false),
                    PasswordHash = table.Column<string>(type: "TEXT", nullable: true),
                    SecurityStamp = table.Column<string>(type: "TEXT", nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "TEXT", nullable: true),
                    PhoneNumber = table.Column<string>(type: "TEXT", nullable: true),
                    PhoneNumberConfirmed = table.Column<bool>(type: "INTEGER", nullable: false),
                    TwoFactorEnabled = table.Column<bool>(type: "INTEGER", nullable: false),
                    LockoutEnd = table.Column<DateTimeOffset>(type: "TEXT", nullable: true),
                    LockoutEnabled = table.Column<bool>(type: "INTEGER", nullable: false),
                    AccessFailedCount = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ProjectSprints",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    Name = table.Column<string>(type: "TEXT", nullable: false),
                    StartDate = table.Column<DateTime>(type: "TEXT", nullable: false),
                    EndDate = table.Column<DateTime>(type: "TEXT", nullable: false),
                    ProjectId = table.Column<Guid>(type: "TEXT", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "TEXT", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "TEXT", nullable: true),
                    TenantId = table.Column<Guid>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProjectSprints", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ProjectSprints_Projects_ProjectId",
                        column: x => x.ProjectId,
                        principalTable: "Projects",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TenantPermissions",
                columns: table => new
                {
                    Key = table.Column<string>(type: "TEXT", nullable: false),
                    Name = table.Column<string>(type: "TEXT", nullable: false),
                    GroupKey = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TenantPermissions", x => x.Key);
                    table.ForeignKey(
                        name: "FK_TenantPermissions_TenantPermissionGroup_GroupKey",
                        column: x => x.GroupKey,
                        principalTable: "TenantPermissionGroup",
                        principalColumn: "Key",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AttachmentCategories",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    Title = table.Column<string>(type: "TEXT", nullable: false),
                    TenantId = table.Column<Guid>(type: "TEXT", nullable: true),
                    TenantId1 = table.Column<Guid>(type: "TEXT", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "TEXT", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AttachmentCategories", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AttachmentCategories_Tenants_TenantId1",
                        column: x => x.TenantId1,
                        principalTable: "Tenants",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TenantInfoEntity",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    Name = table.Column<string>(type: "TEXT", nullable: false),
                    Description = table.Column<string>(type: "TEXT", nullable: true),
                    TenantId = table.Column<Guid>(type: "TEXT", nullable: true),
                    TenantId1 = table.Column<Guid>(type: "TEXT", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "TEXT", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TenantInfoEntity", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TenantInfoEntity_Tenants_TenantId1",
                        column: x => x.TenantId1,
                        principalTable: "Tenants",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TenantInvitations",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    PhoneNumber = table.Column<string>(type: "TEXT", maxLength: 200, nullable: false),
                    SentAt = table.Column<DateTime>(type: "TEXT", nullable: false),
                    AcceptedAt = table.Column<DateTime>(type: "TEXT", nullable: true),
                    ExpirationDate = table.Column<DateTime>(type: "TEXT", nullable: false),
                    CanceledAt = table.Column<DateTime>(type: "TEXT", nullable: true),
                    Status = table.Column<string>(type: "TEXT", nullable: false),
                    TenantId = table.Column<Guid>(type: "TEXT", nullable: true),
                    TenantId1 = table.Column<Guid>(type: "TEXT", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "TEXT", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TenantInvitations", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TenantInvitations_Tenants_TenantId1",
                        column: x => x.TenantId1,
                        principalTable: "Tenants",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TenantRoles",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    Title = table.Column<string>(type: "TEXT", nullable: false),
                    Key = table.Column<string>(type: "TEXT", nullable: false),
                    Deletable = table.Column<bool>(type: "INTEGER", nullable: false),
                    IsSystemRole = table.Column<bool>(type: "INTEGER", nullable: false),
                    TenantId = table.Column<Guid>(type: "TEXT", nullable: true),
                    TenantId1 = table.Column<Guid>(type: "TEXT", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "TEXT", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TenantRoles", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TenantRoles_Tenants_TenantId1",
                        column: x => x.TenantId1,
                        principalTable: "Tenants",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TenantMember",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    UserId = table.Column<Guid>(type: "TEXT", nullable: false),
                    Status = table.Column<string>(type: "TEXT", nullable: false),
                    SprintTaskId = table.Column<Guid>(type: "TEXT", nullable: false),
                    TenantId = table.Column<Guid>(type: "TEXT", nullable: true),
                    TenantId1 = table.Column<Guid>(type: "TEXT", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "TEXT", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TenantMember", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TenantMember_Tenants_TenantId1",
                        column: x => x.TenantId1,
                        principalTable: "Tenants",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_TenantMember_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "KanbanBoards",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    Name = table.Column<string>(type: "TEXT", nullable: false),
                    SprintId = table.Column<Guid>(type: "TEXT", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "TEXT", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "TEXT", nullable: true),
                    TenantId = table.Column<Guid>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_KanbanBoards", x => x.Id);
                    table.ForeignKey(
                        name: "FK_KanbanBoards_ProjectSprints_SprintId",
                        column: x => x.SprintId,
                        principalTable: "ProjectSprints",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "RolePermissions",
                columns: table => new
                {
                    PermissionsKey = table.Column<string>(type: "TEXT", nullable: false),
                    RolesId = table.Column<Guid>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RolePermissions", x => new { x.PermissionsKey, x.RolesId });
                    table.ForeignKey(
                        name: "FK_RolePermissions_TenantPermissions_PermissionsKey",
                        column: x => x.PermissionsKey,
                        principalTable: "TenantPermissions",
                        principalColumn: "Key",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_RolePermissions_TenantRoles_RolesId",
                        column: x => x.RolesId,
                        principalTable: "TenantRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Attachments",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    FileName = table.Column<string>(type: "TEXT", maxLength: 255, nullable: false),
                    FilePath = table.Column<string>(type: "TEXT", maxLength: 1000, nullable: false),
                    Description = table.Column<string>(type: "TEXT", nullable: false),
                    TenantMemberId = table.Column<Guid>(type: "TEXT", nullable: false),
                    CategoryId = table.Column<Guid>(type: "TEXT", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "TEXT", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "TEXT", nullable: true),
                    TenantId = table.Column<Guid>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Attachments", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Attachments_AttachmentCategories_CategoryId",
                        column: x => x.CategoryId,
                        principalTable: "AttachmentCategories",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Attachments_TenantMember_TenantMemberId",
                        column: x => x.TenantMemberId,
                        principalTable: "TenantMember",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ProjectsMembers",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    TenantMemberId = table.Column<Guid>(type: "TEXT", nullable: false),
                    ProjectId = table.Column<Guid>(type: "TEXT", nullable: false),
                    AccessEnum = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProjectsMembers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ProjectsMembers_Projects_ProjectId",
                        column: x => x.ProjectId,
                        principalTable: "Projects",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ProjectsMembers_TenantMember_TenantMemberId",
                        column: x => x.TenantMemberId,
                        principalTable: "TenantMember",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TenantMemberRoles",
                columns: table => new
                {
                    MembersId = table.Column<Guid>(type: "TEXT", nullable: false),
                    RolesId = table.Column<Guid>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TenantMemberRoles", x => new { x.MembersId, x.RolesId });
                    table.ForeignKey(
                        name: "FK_TenantMemberRoles_TenantMember_MembersId",
                        column: x => x.MembersId,
                        principalTable: "TenantMember",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_TenantMemberRoles_TenantRoles_RolesId",
                        column: x => x.RolesId,
                        principalTable: "TenantRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "KanbanBoardColumns",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    Name = table.Column<string>(type: "TEXT", nullable: false),
                    Order = table.Column<int>(type: "INTEGER", nullable: false),
                    BoardId = table.Column<Guid>(type: "TEXT", nullable: false),
                    TenantId = table.Column<Guid>(type: "TEXT", nullable: true),
                    TenantId1 = table.Column<Guid>(type: "TEXT", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "TEXT", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_KanbanBoardColumns", x => x.Id);
                    table.ForeignKey(
                        name: "FK_KanbanBoardColumns_KanbanBoards_BoardId",
                        column: x => x.BoardId,
                        principalTable: "KanbanBoards",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_KanbanBoardColumns_Tenants_TenantId1",
                        column: x => x.TenantId1,
                        principalTable: "Tenants",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "SprintTasks",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    Title = table.Column<string>(type: "TEXT", maxLength: 200, nullable: false),
                    Description = table.Column<string>(type: "TEXT", maxLength: 1000, nullable: false),
                    Order = table.Column<int>(type: "INTEGER", nullable: false),
                    DueDate = table.Column<DateTime>(type: "TEXT", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "TEXT", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "TEXT", nullable: true),
                    BoardColumnId = table.Column<Guid>(type: "TEXT", nullable: false),
                    TenantMemberEntityId = table.Column<Guid>(type: "TEXT", nullable: true),
                    TenantId = table.Column<Guid>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SprintTasks", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SprintTasks_KanbanBoardColumns_BoardColumnId",
                        column: x => x.BoardColumnId,
                        principalTable: "KanbanBoardColumns",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_SprintTasks_TenantMember_TenantMemberEntityId",
                        column: x => x.TenantMemberEntityId,
                        principalTable: "TenantMember",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "TaskComments",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    ProjectMemberId = table.Column<Guid>(type: "TEXT", nullable: false),
                    Content = table.Column<string>(type: "TEXT", maxLength: 1000, nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "TEXT", nullable: false),
                    TaskId = table.Column<Guid>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TaskComments", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TaskComments_ProjectsMembers_ProjectMemberId",
                        column: x => x.ProjectMemberId,
                        principalTable: "ProjectsMembers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_TaskComments_SprintTasks_TaskId",
                        column: x => x.TaskId,
                        principalTable: "SprintTasks",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TaskLabels",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    Name = table.Column<string>(type: "TEXT", maxLength: 200, nullable: false),
                    SprintTaskId = table.Column<Guid>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TaskLabels", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TaskLabels_SprintTasks_SprintTaskId",
                        column: x => x.SprintTaskId,
                        principalTable: "SprintTasks",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TasksAssignee",
                columns: table => new
                {
                    AssigneeMembersId = table.Column<Guid>(type: "TEXT", nullable: false),
                    TasksId = table.Column<Guid>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TasksAssignee", x => new { x.AssigneeMembersId, x.TasksId });
                    table.ForeignKey(
                        name: "FK_TasksAssignee_ProjectsMembers_AssigneeMembersId",
                        column: x => x.AssigneeMembersId,
                        principalTable: "ProjectsMembers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_TasksAssignee_SprintTasks_TasksId",
                        column: x => x.TasksId,
                        principalTable: "SprintTasks",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TasksAttachments",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    ProjectMemberId = table.Column<Guid>(type: "TEXT", nullable: false),
                    TaskId = table.Column<Guid>(type: "TEXT", nullable: false),
                    AttachmentId = table.Column<Guid>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TasksAttachments", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TasksAttachments_Attachments_AttachmentId",
                        column: x => x.AttachmentId,
                        principalTable: "Attachments",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_TasksAttachments_ProjectsMembers_ProjectMemberId",
                        column: x => x.ProjectMemberId,
                        principalTable: "ProjectsMembers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_TasksAttachments_SprintTasks_TaskId",
                        column: x => x.TaskId,
                        principalTable: "SprintTasks",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AttachmentCategories_TenantId1",
                table: "AttachmentCategories",
                column: "TenantId1");

            migrationBuilder.CreateIndex(
                name: "IX_Attachments_CategoryId",
                table: "Attachments",
                column: "CategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_Attachments_TenantMemberId",
                table: "Attachments",
                column: "TenantMemberId");

            migrationBuilder.CreateIndex(
                name: "IX_KanbanBoardColumns_BoardId",
                table: "KanbanBoardColumns",
                column: "BoardId");

            migrationBuilder.CreateIndex(
                name: "IX_KanbanBoardColumns_TenantId1",
                table: "KanbanBoardColumns",
                column: "TenantId1");

            migrationBuilder.CreateIndex(
                name: "IX_KanbanBoards_SprintId",
                table: "KanbanBoards",
                column: "SprintId");

            migrationBuilder.CreateIndex(
                name: "IX_ProjectsMembers_ProjectId",
                table: "ProjectsMembers",
                column: "ProjectId");

            migrationBuilder.CreateIndex(
                name: "IX_ProjectsMembers_TenantMemberId",
                table: "ProjectsMembers",
                column: "TenantMemberId");

            migrationBuilder.CreateIndex(
                name: "IX_ProjectSprints_ProjectId",
                table: "ProjectSprints",
                column: "ProjectId");

            migrationBuilder.CreateIndex(
                name: "IX_RolePermissions_RolesId",
                table: "RolePermissions",
                column: "RolesId");

            migrationBuilder.CreateIndex(
                name: "IX_SprintTasks_BoardColumnId",
                table: "SprintTasks",
                column: "BoardColumnId");

            migrationBuilder.CreateIndex(
                name: "IX_SprintTasks_TenantMemberEntityId",
                table: "SprintTasks",
                column: "TenantMemberEntityId");

            migrationBuilder.CreateIndex(
                name: "IX_TaskComments_ProjectMemberId",
                table: "TaskComments",
                column: "ProjectMemberId");

            migrationBuilder.CreateIndex(
                name: "IX_TaskComments_TaskId",
                table: "TaskComments",
                column: "TaskId");

            migrationBuilder.CreateIndex(
                name: "IX_TaskLabels_SprintTaskId",
                table: "TaskLabels",
                column: "SprintTaskId");

            migrationBuilder.CreateIndex(
                name: "IX_TasksAssignee_TasksId",
                table: "TasksAssignee",
                column: "TasksId");

            migrationBuilder.CreateIndex(
                name: "IX_TasksAttachments_AttachmentId",
                table: "TasksAttachments",
                column: "AttachmentId");

            migrationBuilder.CreateIndex(
                name: "IX_TasksAttachments_ProjectMemberId",
                table: "TasksAttachments",
                column: "ProjectMemberId");

            migrationBuilder.CreateIndex(
                name: "IX_TasksAttachments_TaskId",
                table: "TasksAttachments",
                column: "TaskId");

            migrationBuilder.CreateIndex(
                name: "IX_TenantInfoEntity_TenantId1",
                table: "TenantInfoEntity",
                column: "TenantId1");

            migrationBuilder.CreateIndex(
                name: "IX_TenantInvitations_TenantId1",
                table: "TenantInvitations",
                column: "TenantId1");

            migrationBuilder.CreateIndex(
                name: "IX_TenantMember_TenantId1",
                table: "TenantMember",
                column: "TenantId1");

            migrationBuilder.CreateIndex(
                name: "IX_TenantMember_UserId",
                table: "TenantMember",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_TenantMemberRoles_RolesId",
                table: "TenantMemberRoles",
                column: "RolesId");

            migrationBuilder.CreateIndex(
                name: "IX_TenantPermissions_GroupKey",
                table: "TenantPermissions",
                column: "GroupKey");

            migrationBuilder.CreateIndex(
                name: "IX_TenantRoles_TenantId1",
                table: "TenantRoles",
                column: "TenantId1");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "RolePermissions");

            migrationBuilder.DropTable(
                name: "TaskComments");

            migrationBuilder.DropTable(
                name: "TaskLabels");

            migrationBuilder.DropTable(
                name: "TasksAssignee");

            migrationBuilder.DropTable(
                name: "TasksAttachments");

            migrationBuilder.DropTable(
                name: "TenantInfoEntity");

            migrationBuilder.DropTable(
                name: "TenantInvitations");

            migrationBuilder.DropTable(
                name: "TenantMemberRoles");

            migrationBuilder.DropTable(
                name: "TenantPermissions");

            migrationBuilder.DropTable(
                name: "Attachments");

            migrationBuilder.DropTable(
                name: "ProjectsMembers");

            migrationBuilder.DropTable(
                name: "SprintTasks");

            migrationBuilder.DropTable(
                name: "TenantRoles");

            migrationBuilder.DropTable(
                name: "TenantPermissionGroup");

            migrationBuilder.DropTable(
                name: "AttachmentCategories");

            migrationBuilder.DropTable(
                name: "KanbanBoardColumns");

            migrationBuilder.DropTable(
                name: "TenantMember");

            migrationBuilder.DropTable(
                name: "KanbanBoards");

            migrationBuilder.DropTable(
                name: "Tenants");

            migrationBuilder.DropTable(
                name: "Users");

            migrationBuilder.DropTable(
                name: "ProjectSprints");

            migrationBuilder.DropTable(
                name: "Projects");
        }
    }
}
