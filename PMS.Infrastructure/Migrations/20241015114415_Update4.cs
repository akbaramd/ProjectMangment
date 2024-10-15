using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PMS.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class Update4 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserTenants_Roles_RoleId",
                table: "UserTenants");

            migrationBuilder.DropIndex(
                name: "IX_UserTenants_RoleId",
                table: "UserTenants");

            migrationBuilder.DropColumn(
                name: "RoleId",
                table: "UserTenants");

            migrationBuilder.AddColumn<int>(
                name: "Role",
                table: "UserTenants",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<bool>(
                name: "Deletable",
                table: "Users",
                type: "INTEGER",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "Deletable",
                table: "Roles",
                type: "INTEGER",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Role",
                table: "UserTenants");

            migrationBuilder.DropColumn(
                name: "Deletable",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "Deletable",
                table: "Roles");

            migrationBuilder.AddColumn<Guid>(
                name: "RoleId",
                table: "UserTenants",
                type: "TEXT",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "IX_UserTenants_RoleId",
                table: "UserTenants",
                column: "RoleId");

            migrationBuilder.AddForeignKey(
                name: "FK_UserTenants_Roles_RoleId",
                table: "UserTenants",
                column: "RoleId",
                principalTable: "Roles",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
