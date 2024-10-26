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
                name: "FK_KanbanBoardColumns_KanbanBoards_KanbanBoardId",
                table: "KanbanBoardColumns");

            migrationBuilder.DropForeignKey(
                name: "FK_KanbanBoards_ProjectSprints_ProjectSprintId",
                table: "KanbanBoards");

            migrationBuilder.DropIndex(
                name: "IX_KanbanBoards_ProjectSprintId",
                table: "KanbanBoards");

            migrationBuilder.DropIndex(
                name: "IX_KanbanBoardColumns_KanbanBoardId",
                table: "KanbanBoardColumns");

            migrationBuilder.DropColumn(
                name: "ProjectSprintId",
                table: "KanbanBoards");

            migrationBuilder.DropColumn(
                name: "KanbanBoardId",
                table: "KanbanBoardColumns");

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
                name: "IX_KanbanBoards_SprintId",
                table: "KanbanBoards",
                column: "SprintId");

            migrationBuilder.CreateIndex(
                name: "IX_KanbanBoardColumns_BoardId",
                table: "KanbanBoardColumns",
                column: "BoardId");

            migrationBuilder.AddForeignKey(
                name: "FK_KanbanBoardColumns_KanbanBoards_BoardId",
                table: "KanbanBoardColumns",
                column: "BoardId",
                principalTable: "KanbanBoards",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_KanbanBoards_ProjectSprints_SprintId",
                table: "KanbanBoards",
                column: "SprintId",
                principalTable: "ProjectSprints",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_KanbanBoardColumns_KanbanBoards_BoardId",
                table: "KanbanBoardColumns");

            migrationBuilder.DropForeignKey(
                name: "FK_KanbanBoards_ProjectSprints_SprintId",
                table: "KanbanBoards");

            migrationBuilder.DropIndex(
                name: "IX_KanbanBoards_SprintId",
                table: "KanbanBoards");

            migrationBuilder.DropIndex(
                name: "IX_KanbanBoardColumns_BoardId",
                table: "KanbanBoardColumns");

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
                table: "KanbanBoards",
                type: "TEXT",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<Guid>(
                name: "KanbanBoardId",
                table: "KanbanBoardColumns",
                type: "TEXT",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "IX_KanbanBoards_ProjectSprintId",
                table: "KanbanBoards",
                column: "ProjectSprintId");

            migrationBuilder.CreateIndex(
                name: "IX_KanbanBoardColumns_KanbanBoardId",
                table: "KanbanBoardColumns",
                column: "KanbanBoardId");

            migrationBuilder.AddForeignKey(
                name: "FK_KanbanBoardColumns_KanbanBoards_KanbanBoardId",
                table: "KanbanBoardColumns",
                column: "KanbanBoardId",
                principalTable: "KanbanBoards",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_KanbanBoards_ProjectSprints_ProjectSprintId",
                table: "KanbanBoards",
                column: "ProjectSprintId",
                principalTable: "ProjectSprints",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
