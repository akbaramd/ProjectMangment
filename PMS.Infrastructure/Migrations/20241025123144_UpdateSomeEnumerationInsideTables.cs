using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PMS.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class UpdateSomeEnumerationInsideTables : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ProjectsMembers_TenantMembers_TenantMemberId",
                table: "ProjectsMembers");

            migrationBuilder.DropForeignKey(
                name: "FK_SprintTaskAssignee_TenantMembers_AssigneeMembersId",
                table: "SprintTaskAssignee");

            migrationBuilder.DropForeignKey(
                name: "FK_TenantMemberRoles_TenantMembers_MembersId",
                table: "TenantMemberRoles");

            migrationBuilder.DropForeignKey(
                name: "FK_TenantMembers_Tenants_TenantId",
                table: "TenantMembers");

            migrationBuilder.DropForeignKey(
                name: "FK_TenantMembers_Users_UserId",
                table: "TenantMembers");

            migrationBuilder.DropPrimaryKey(
                name: "PK_TenantMembers",
                table: "TenantMembers");

            migrationBuilder.DropColumn(
                name: "Status",
                table: "SprintTasks");

            migrationBuilder.RenameTable(
                name: "TenantMembers",
                newName: "TenantMember");

            migrationBuilder.RenameColumn(
                name: "MemberStatus",
                table: "TenantMember",
                newName: "Status");

            migrationBuilder.RenameIndex(
                name: "IX_TenantMembers_UserId",
                table: "TenantMember",
                newName: "IX_TenantMember_UserId");

            migrationBuilder.RenameIndex(
                name: "IX_TenantMembers_TenantId",
                table: "TenantMember",
                newName: "IX_TenantMember_TenantId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_TenantMember",
                table: "TenantMember",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ProjectsMembers_TenantMember_TenantMemberId",
                table: "ProjectsMembers",
                column: "TenantMemberId",
                principalTable: "TenantMember",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_SprintTaskAssignee_TenantMember_AssigneeMembersId",
                table: "SprintTaskAssignee",
                column: "AssigneeMembersId",
                principalTable: "TenantMember",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_TenantMember_Tenants_TenantId",
                table: "TenantMember",
                column: "TenantId",
                principalTable: "Tenants",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_TenantMember_Users_UserId",
                table: "TenantMember",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_TenantMemberRoles_TenantMember_MembersId",
                table: "TenantMemberRoles",
                column: "MembersId",
                principalTable: "TenantMember",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ProjectsMembers_TenantMember_TenantMemberId",
                table: "ProjectsMembers");

            migrationBuilder.DropForeignKey(
                name: "FK_SprintTaskAssignee_TenantMember_AssigneeMembersId",
                table: "SprintTaskAssignee");

            migrationBuilder.DropForeignKey(
                name: "FK_TenantMember_Tenants_TenantId",
                table: "TenantMember");

            migrationBuilder.DropForeignKey(
                name: "FK_TenantMember_Users_UserId",
                table: "TenantMember");

            migrationBuilder.DropForeignKey(
                name: "FK_TenantMemberRoles_TenantMember_MembersId",
                table: "TenantMemberRoles");

            migrationBuilder.DropPrimaryKey(
                name: "PK_TenantMember",
                table: "TenantMember");

            migrationBuilder.RenameTable(
                name: "TenantMember",
                newName: "TenantMembers");

            migrationBuilder.RenameColumn(
                name: "Status",
                table: "TenantMembers",
                newName: "MemberStatus");

            migrationBuilder.RenameIndex(
                name: "IX_TenantMember_UserId",
                table: "TenantMembers",
                newName: "IX_TenantMembers_UserId");

            migrationBuilder.RenameIndex(
                name: "IX_TenantMember_TenantId",
                table: "TenantMembers",
                newName: "IX_TenantMembers_TenantId");

            migrationBuilder.AddColumn<int>(
                name: "Status",
                table: "SprintTasks",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddPrimaryKey(
                name: "PK_TenantMembers",
                table: "TenantMembers",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ProjectsMembers_TenantMembers_TenantMemberId",
                table: "ProjectsMembers",
                column: "TenantMemberId",
                principalTable: "TenantMembers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_SprintTaskAssignee_TenantMembers_AssigneeMembersId",
                table: "SprintTaskAssignee",
                column: "AssigneeMembersId",
                principalTable: "TenantMembers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_TenantMemberRoles_TenantMembers_MembersId",
                table: "TenantMemberRoles",
                column: "MembersId",
                principalTable: "TenantMembers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_TenantMembers_Tenants_TenantId",
                table: "TenantMembers",
                column: "TenantId",
                principalTable: "Tenants",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_TenantMembers_Users_UserId",
                table: "TenantMembers",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
