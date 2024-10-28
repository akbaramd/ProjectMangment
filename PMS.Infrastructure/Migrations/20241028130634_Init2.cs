using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PMS.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class Init2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AttachmentCategories_Tenants_TenantId1",
                table: "AttachmentCategories");

            migrationBuilder.DropForeignKey(
                name: "FK_KanbanBoardColumns_Tenants_TenantId1",
                table: "KanbanBoardColumns");

            migrationBuilder.DropForeignKey(
                name: "FK_TenantInfoEntity_Tenants_TenantId1",
                table: "TenantInfoEntity");

            migrationBuilder.DropForeignKey(
                name: "FK_TenantInvitations_Tenants_TenantId1",
                table: "TenantInvitations");

            migrationBuilder.DropForeignKey(
                name: "FK_TenantMember_Tenants_TenantId1",
                table: "TenantMember");

            migrationBuilder.DropForeignKey(
                name: "FK_TenantRoles_Tenants_TenantId1",
                table: "TenantRoles");

            migrationBuilder.DropIndex(
                name: "IX_TenantRoles_TenantId1",
                table: "TenantRoles");

            migrationBuilder.DropIndex(
                name: "IX_TenantMember_TenantId1",
                table: "TenantMember");

            migrationBuilder.DropIndex(
                name: "IX_TenantInvitations_TenantId1",
                table: "TenantInvitations");

            migrationBuilder.DropIndex(
                name: "IX_TenantInfoEntity_TenantId1",
                table: "TenantInfoEntity");

            migrationBuilder.DropIndex(
                name: "IX_KanbanBoardColumns_TenantId1",
                table: "KanbanBoardColumns");

            migrationBuilder.DropIndex(
                name: "IX_AttachmentCategories_TenantId1",
                table: "AttachmentCategories");

            migrationBuilder.DropColumn(
                name: "TenantId1",
                table: "TenantRoles");

            migrationBuilder.DropColumn(
                name: "TenantId1",
                table: "TenantMember");

            migrationBuilder.DropColumn(
                name: "TenantId1",
                table: "TenantInvitations");

            migrationBuilder.DropColumn(
                name: "TenantId1",
                table: "TenantInfoEntity");

            migrationBuilder.DropColumn(
                name: "TenantId1",
                table: "KanbanBoardColumns");

            migrationBuilder.DropColumn(
                name: "TenantId1",
                table: "AttachmentCategories");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "TenantId1",
                table: "TenantRoles",
                type: "TEXT",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<Guid>(
                name: "TenantId1",
                table: "TenantMember",
                type: "TEXT",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<Guid>(
                name: "TenantId1",
                table: "TenantInvitations",
                type: "TEXT",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<Guid>(
                name: "TenantId1",
                table: "TenantInfoEntity",
                type: "TEXT",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<Guid>(
                name: "TenantId1",
                table: "KanbanBoardColumns",
                type: "TEXT",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<Guid>(
                name: "TenantId1",
                table: "AttachmentCategories",
                type: "TEXT",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "IX_TenantRoles_TenantId1",
                table: "TenantRoles",
                column: "TenantId1");

            migrationBuilder.CreateIndex(
                name: "IX_TenantMember_TenantId1",
                table: "TenantMember",
                column: "TenantId1");

            migrationBuilder.CreateIndex(
                name: "IX_TenantInvitations_TenantId1",
                table: "TenantInvitations",
                column: "TenantId1");

            migrationBuilder.CreateIndex(
                name: "IX_TenantInfoEntity_TenantId1",
                table: "TenantInfoEntity",
                column: "TenantId1");

            migrationBuilder.CreateIndex(
                name: "IX_KanbanBoardColumns_TenantId1",
                table: "KanbanBoardColumns",
                column: "TenantId1");

            migrationBuilder.CreateIndex(
                name: "IX_AttachmentCategories_TenantId1",
                table: "AttachmentCategories",
                column: "TenantId1");

            migrationBuilder.AddForeignKey(
                name: "FK_AttachmentCategories_Tenants_TenantId1",
                table: "AttachmentCategories",
                column: "TenantId1",
                principalTable: "Tenants",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_KanbanBoardColumns_Tenants_TenantId1",
                table: "KanbanBoardColumns",
                column: "TenantId1",
                principalTable: "Tenants",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_TenantInfoEntity_Tenants_TenantId1",
                table: "TenantInfoEntity",
                column: "TenantId1",
                principalTable: "Tenants",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_TenantInvitations_Tenants_TenantId1",
                table: "TenantInvitations",
                column: "TenantId1",
                principalTable: "Tenants",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_TenantMember_Tenants_TenantId1",
                table: "TenantMember",
                column: "TenantId1",
                principalTable: "Tenants",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_TenantRoles_Tenants_TenantId1",
                table: "TenantRoles",
                column: "TenantId1",
                principalTable: "Tenants",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
