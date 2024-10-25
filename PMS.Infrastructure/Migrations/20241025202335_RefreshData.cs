using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PMS.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class RefreshData : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TaskAttachments_SprintTasks_SprintTaskId",
                table: "TaskAttachments");

            migrationBuilder.DropForeignKey(
                name: "FK_TaskComments_SprintTasks_SprintTaskId",
                table: "TaskComments");

            migrationBuilder.DropForeignKey(
                name: "FK_TaskComments_Users_UserId",
                table: "TaskComments");

            migrationBuilder.DropTable(
                name: "SprintTaskAssignee");

            migrationBuilder.DropPrimaryKey(
                name: "PK_TaskAttachments",
                table: "TaskAttachments");

            migrationBuilder.DropColumn(
                name: "FileName",
                table: "TaskAttachments");

            migrationBuilder.DropColumn(
                name: "FilePath",
                table: "TaskAttachments");

            migrationBuilder.RenameTable(
                name: "TaskAttachments",
                newName: "TasksAttachments");

            migrationBuilder.RenameColumn(
                name: "UserId",
                table: "TaskComments",
                newName: "TaskId");

            migrationBuilder.RenameColumn(
                name: "SprintTaskId",
                table: "TaskComments",
                newName: "ProjectMemberId");

            migrationBuilder.RenameIndex(
                name: "IX_TaskComments_UserId",
                table: "TaskComments",
                newName: "IX_TaskComments_TaskId");

            migrationBuilder.RenameIndex(
                name: "IX_TaskComments_SprintTaskId",
                table: "TaskComments",
                newName: "IX_TaskComments_ProjectMemberId");

            migrationBuilder.RenameColumn(
                name: "SprintTaskId",
                table: "TasksAttachments",
                newName: "TaskId");

            migrationBuilder.RenameIndex(
                name: "IX_TaskAttachments_SprintTaskId",
                table: "TasksAttachments",
                newName: "IX_TasksAttachments_TaskId");

            migrationBuilder.AddColumn<Guid>(
                name: "TenantMemberEntityId",
                table: "SprintTasks",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "AttachmentId",
                table: "TasksAttachments",
                type: "TEXT",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<Guid>(
                name: "ProjectMemberId",
                table: "TasksAttachments",
                type: "TEXT",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddPrimaryKey(
                name: "PK_TasksAttachments",
                table: "TasksAttachments",
                column: "Id");

            migrationBuilder.CreateTable(
                name: "AttachmentCategories",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    Title = table.Column<string>(type: "TEXT", nullable: false),
                    TenantId = table.Column<Guid>(type: "TEXT", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "TEXT", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AttachmentCategories", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AttachmentCategories_Tenants_TenantId",
                        column: x => x.TenantId,
                        principalTable: "Tenants",
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
                name: "Attachments",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    FileName = table.Column<string>(type: "TEXT", maxLength: 255, nullable: false),
                    FilePath = table.Column<string>(type: "TEXT", maxLength: 1000, nullable: false),
                    Description = table.Column<string>(type: "TEXT", nullable: false),
                    TenantMemberId = table.Column<Guid>(type: "TEXT", nullable: false),
                    CategoryId = table.Column<Guid>(type: "TEXT", nullable: true),
                    TenantId = table.Column<Guid>(type: "TEXT", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "TEXT", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "TEXT", nullable: true)
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
                    table.ForeignKey(
                        name: "FK_Attachments_Tenants_TenantId",
                        column: x => x.TenantId,
                        principalTable: "Tenants",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_SprintTasks_TenantMemberEntityId",
                table: "SprintTasks",
                column: "TenantMemberEntityId");

            migrationBuilder.CreateIndex(
                name: "IX_TasksAttachments_AttachmentId",
                table: "TasksAttachments",
                column: "AttachmentId");

            migrationBuilder.CreateIndex(
                name: "IX_TasksAttachments_ProjectMemberId",
                table: "TasksAttachments",
                column: "ProjectMemberId");

            migrationBuilder.CreateIndex(
                name: "IX_AttachmentCategories_TenantId",
                table: "AttachmentCategories",
                column: "TenantId");

            migrationBuilder.CreateIndex(
                name: "IX_Attachments_CategoryId",
                table: "Attachments",
                column: "CategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_Attachments_TenantId",
                table: "Attachments",
                column: "TenantId");

            migrationBuilder.CreateIndex(
                name: "IX_Attachments_TenantMemberId",
                table: "Attachments",
                column: "TenantMemberId");

            migrationBuilder.CreateIndex(
                name: "IX_TasksAssignee_TasksId",
                table: "TasksAssignee",
                column: "TasksId");

            migrationBuilder.AddForeignKey(
                name: "FK_SprintTasks_TenantMember_TenantMemberEntityId",
                table: "SprintTasks",
                column: "TenantMemberEntityId",
                principalTable: "TenantMember",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_TaskComments_ProjectsMembers_ProjectMemberId",
                table: "TaskComments",
                column: "ProjectMemberId",
                principalTable: "ProjectsMembers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_TaskComments_SprintTasks_TaskId",
                table: "TaskComments",
                column: "TaskId",
                principalTable: "SprintTasks",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_TasksAttachments_Attachments_AttachmentId",
                table: "TasksAttachments",
                column: "AttachmentId",
                principalTable: "Attachments",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_TasksAttachments_ProjectsMembers_ProjectMemberId",
                table: "TasksAttachments",
                column: "ProjectMemberId",
                principalTable: "ProjectsMembers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_TasksAttachments_SprintTasks_TaskId",
                table: "TasksAttachments",
                column: "TaskId",
                principalTable: "SprintTasks",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SprintTasks_TenantMember_TenantMemberEntityId",
                table: "SprintTasks");

            migrationBuilder.DropForeignKey(
                name: "FK_TaskComments_ProjectsMembers_ProjectMemberId",
                table: "TaskComments");

            migrationBuilder.DropForeignKey(
                name: "FK_TaskComments_SprintTasks_TaskId",
                table: "TaskComments");

            migrationBuilder.DropForeignKey(
                name: "FK_TasksAttachments_Attachments_AttachmentId",
                table: "TasksAttachments");

            migrationBuilder.DropForeignKey(
                name: "FK_TasksAttachments_ProjectsMembers_ProjectMemberId",
                table: "TasksAttachments");

            migrationBuilder.DropForeignKey(
                name: "FK_TasksAttachments_SprintTasks_TaskId",
                table: "TasksAttachments");

            migrationBuilder.DropTable(
                name: "Attachments");

            migrationBuilder.DropTable(
                name: "TasksAssignee");

            migrationBuilder.DropTable(
                name: "AttachmentCategories");

            migrationBuilder.DropIndex(
                name: "IX_SprintTasks_TenantMemberEntityId",
                table: "SprintTasks");

            migrationBuilder.DropPrimaryKey(
                name: "PK_TasksAttachments",
                table: "TasksAttachments");

            migrationBuilder.DropIndex(
                name: "IX_TasksAttachments_AttachmentId",
                table: "TasksAttachments");

            migrationBuilder.DropIndex(
                name: "IX_TasksAttachments_ProjectMemberId",
                table: "TasksAttachments");

            migrationBuilder.DropColumn(
                name: "TenantMemberEntityId",
                table: "SprintTasks");

            migrationBuilder.DropColumn(
                name: "AttachmentId",
                table: "TasksAttachments");

            migrationBuilder.DropColumn(
                name: "ProjectMemberId",
                table: "TasksAttachments");

            migrationBuilder.RenameTable(
                name: "TasksAttachments",
                newName: "TaskAttachments");

            migrationBuilder.RenameColumn(
                name: "TaskId",
                table: "TaskComments",
                newName: "UserId");

            migrationBuilder.RenameColumn(
                name: "ProjectMemberId",
                table: "TaskComments",
                newName: "SprintTaskId");

            migrationBuilder.RenameIndex(
                name: "IX_TaskComments_TaskId",
                table: "TaskComments",
                newName: "IX_TaskComments_UserId");

            migrationBuilder.RenameIndex(
                name: "IX_TaskComments_ProjectMemberId",
                table: "TaskComments",
                newName: "IX_TaskComments_SprintTaskId");

            migrationBuilder.RenameColumn(
                name: "TaskId",
                table: "TaskAttachments",
                newName: "SprintTaskId");

            migrationBuilder.RenameIndex(
                name: "IX_TasksAttachments_TaskId",
                table: "TaskAttachments",
                newName: "IX_TaskAttachments_SprintTaskId");

            migrationBuilder.AddColumn<string>(
                name: "FileName",
                table: "TaskAttachments",
                type: "TEXT",
                maxLength: 255,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "FilePath",
                table: "TaskAttachments",
                type: "TEXT",
                maxLength: 1000,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddPrimaryKey(
                name: "PK_TaskAttachments",
                table: "TaskAttachments",
                column: "Id");

            migrationBuilder.CreateTable(
                name: "SprintTaskAssignee",
                columns: table => new
                {
                    AssigneeMembersId = table.Column<Guid>(type: "TEXT", nullable: false),
                    TasksId = table.Column<Guid>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SprintTaskAssignee", x => new { x.AssigneeMembersId, x.TasksId });
                    table.ForeignKey(
                        name: "FK_SprintTaskAssignee_SprintTasks_TasksId",
                        column: x => x.TasksId,
                        principalTable: "SprintTasks",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_SprintTaskAssignee_TenantMember_AssigneeMembersId",
                        column: x => x.AssigneeMembersId,
                        principalTable: "TenantMember",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_SprintTaskAssignee_TasksId",
                table: "SprintTaskAssignee",
                column: "TasksId");

            migrationBuilder.AddForeignKey(
                name: "FK_TaskAttachments_SprintTasks_SprintTaskId",
                table: "TaskAttachments",
                column: "SprintTaskId",
                principalTable: "SprintTasks",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_TaskComments_SprintTasks_SprintTaskId",
                table: "TaskComments",
                column: "SprintTaskId",
                principalTable: "SprintTasks",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_TaskComments_Users_UserId",
                table: "TaskComments",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
