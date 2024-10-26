using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PMS.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class UpdateDomain : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_KanbanBoardColumns_KanbanBoards_BoardId",
                table: "KanbanBoardColumns");

            migrationBuilder.DropIndex(
                name: "IX_KanbanBoardColumns_BoardId",
                table: "KanbanBoardColumns");

            migrationBuilder.DropColumn(
                name: "Content",
                table: "SprintTasks");

            migrationBuilder.RenameColumn(
                name: "Access",
                table: "ProjectsMembers",
                newName: "AccessEnum");

            migrationBuilder.AddColumn<Guid>(
                name: "KanbanBoardEntityId",
                table: "KanbanBoardColumns",
                type: "TEXT",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_KanbanBoardColumns_KanbanBoardEntityId",
                table: "KanbanBoardColumns",
                column: "KanbanBoardEntityId");

            migrationBuilder.AddForeignKey(
                name: "FK_KanbanBoardColumns_KanbanBoards_KanbanBoardEntityId",
                table: "KanbanBoardColumns",
                column: "KanbanBoardEntityId",
                principalTable: "KanbanBoards",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_KanbanBoardColumns_KanbanBoards_KanbanBoardEntityId",
                table: "KanbanBoardColumns");

            migrationBuilder.DropIndex(
                name: "IX_KanbanBoardColumns_KanbanBoardEntityId",
                table: "KanbanBoardColumns");

            migrationBuilder.DropColumn(
                name: "KanbanBoardEntityId",
                table: "KanbanBoardColumns");

            migrationBuilder.RenameColumn(
                name: "AccessEnum",
                table: "ProjectsMembers",
                newName: "Access");

            migrationBuilder.AddColumn<string>(
                name: "Content",
                table: "SprintTasks",
                type: "TEXT",
                maxLength: 2000,
                nullable: false,
                defaultValue: "");

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
        }
    }
}
