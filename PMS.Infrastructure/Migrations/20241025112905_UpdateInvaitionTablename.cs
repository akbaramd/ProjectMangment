using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PMS.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class UpdateInvaitionTablename : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Invitations_Tenants_TenantId",
                table: "Invitations");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Invitations",
                table: "Invitations");

            migrationBuilder.RenameTable(
                name: "Invitations",
                newName: "TenantInvitations");

            migrationBuilder.RenameIndex(
                name: "IX_Invitations_TenantId",
                table: "TenantInvitations",
                newName: "IX_TenantInvitations_TenantId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_TenantInvitations",
                table: "TenantInvitations",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_TenantInvitations_Tenants_TenantId",
                table: "TenantInvitations",
                column: "TenantId",
                principalTable: "Tenants",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TenantInvitations_Tenants_TenantId",
                table: "TenantInvitations");

            migrationBuilder.DropPrimaryKey(
                name: "PK_TenantInvitations",
                table: "TenantInvitations");

            migrationBuilder.RenameTable(
                name: "TenantInvitations",
                newName: "Invitations");

            migrationBuilder.RenameIndex(
                name: "IX_TenantInvitations_TenantId",
                table: "Invitations",
                newName: "IX_Invitations_TenantId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Invitations",
                table: "Invitations",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Invitations_Tenants_TenantId",
                table: "Invitations",
                column: "TenantId",
                principalTable: "Tenants",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
