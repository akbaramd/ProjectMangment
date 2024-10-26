using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PMS.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddKanbanBoardColumnUpdate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
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

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_KanbanBoardColumns_KanbanBoards_BoardId",
                table: "KanbanBoardColumns");

            migrationBuilder.DropIndex(
                name: "IX_KanbanBoardColumns_BoardId",
                table: "KanbanBoardColumns");

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
    }
}
