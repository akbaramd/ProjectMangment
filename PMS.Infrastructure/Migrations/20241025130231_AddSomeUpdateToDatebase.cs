using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PMS.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddSomeUpdateToDatebase : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ProjectBoardColumns_ProjectBoards_ProjectBoardId",
                table: "ProjectBoardColumns");

            migrationBuilder.DropForeignKey(
                name: "FK_ProjectBoards_ProjectSprints_ProjectSprintId",
                table: "ProjectBoards");

            migrationBuilder.DropIndex(
                name: "IX_ProjectBoards_ProjectSprintId",
                table: "ProjectBoards");

            migrationBuilder.DropIndex(
                name: "IX_ProjectBoardColumns_ProjectBoardId",
                table: "ProjectBoardColumns");

            migrationBuilder.DropColumn(
                name: "ProjectSprintId",
                table: "ProjectBoards");

            migrationBuilder.DropColumn(
                name: "ProjectBoardId",
                table: "ProjectBoardColumns");

            migrationBuilder.AlterColumn<Guid>(
                name: "TenantId",
                table: "TenantRoles",
                type: "TEXT",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "TEXT",
                oldNullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedAt",
                table: "TenantRoles",
                type: "TEXT",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdatedAt",
                table: "TenantRoles",
                type: "TEXT",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_ProjectBoards_SprintId",
                table: "ProjectBoards",
                column: "SprintId");

            migrationBuilder.CreateIndex(
                name: "IX_ProjectBoardColumns_BoardId",
                table: "ProjectBoardColumns",
                column: "BoardId");

            migrationBuilder.AddForeignKey(
                name: "FK_ProjectBoardColumns_ProjectBoards_BoardId",
                table: "ProjectBoardColumns",
                column: "BoardId",
                principalTable: "ProjectBoards",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ProjectBoards_ProjectSprints_SprintId",
                table: "ProjectBoards",
                column: "SprintId",
                principalTable: "ProjectSprints",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ProjectBoardColumns_ProjectBoards_BoardId",
                table: "ProjectBoardColumns");

            migrationBuilder.DropForeignKey(
                name: "FK_ProjectBoards_ProjectSprints_SprintId",
                table: "ProjectBoards");

            migrationBuilder.DropIndex(
                name: "IX_ProjectBoards_SprintId",
                table: "ProjectBoards");

            migrationBuilder.DropIndex(
                name: "IX_ProjectBoardColumns_BoardId",
                table: "ProjectBoardColumns");

            migrationBuilder.DropColumn(
                name: "CreatedAt",
                table: "TenantRoles");

            migrationBuilder.DropColumn(
                name: "UpdatedAt",
                table: "TenantRoles");

            migrationBuilder.AlterColumn<Guid>(
                name: "TenantId",
                table: "TenantRoles",
                type: "TEXT",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "TEXT");

            migrationBuilder.AddColumn<Guid>(
                name: "ProjectSprintId",
                table: "ProjectBoards",
                type: "TEXT",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<Guid>(
                name: "ProjectBoardId",
                table: "ProjectBoardColumns",
                type: "TEXT",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "IX_ProjectBoards_ProjectSprintId",
                table: "ProjectBoards",
                column: "ProjectSprintId");

            migrationBuilder.CreateIndex(
                name: "IX_ProjectBoardColumns_ProjectBoardId",
                table: "ProjectBoardColumns",
                column: "ProjectBoardId");

            migrationBuilder.AddForeignKey(
                name: "FK_ProjectBoardColumns_ProjectBoards_ProjectBoardId",
                table: "ProjectBoardColumns",
                column: "ProjectBoardId",
                principalTable: "ProjectBoards",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ProjectBoards_ProjectSprints_ProjectSprintId",
                table: "ProjectBoards",
                column: "ProjectSprintId",
                principalTable: "ProjectSprints",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
