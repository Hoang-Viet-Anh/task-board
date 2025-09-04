using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TaskBoard.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class FixUserTask : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TaskBoards_Tasks_TaskId",
                table: "TaskBoards");

            migrationBuilder.DropForeignKey(
                name: "FK_TaskBoards_Users_UserId",
                table: "TaskBoards");

            migrationBuilder.DropPrimaryKey(
                name: "PK_TaskBoards",
                table: "TaskBoards");

            migrationBuilder.RenameTable(
                name: "TaskBoards",
                newName: "UserTasks");

            migrationBuilder.RenameIndex(
                name: "IX_TaskBoards_UserId",
                table: "UserTasks",
                newName: "IX_UserTasks_UserId");

            migrationBuilder.RenameIndex(
                name: "IX_TaskBoards_TaskId",
                table: "UserTasks",
                newName: "IX_UserTasks_TaskId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_UserTasks",
                table: "UserTasks",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_UserTasks_Tasks_TaskId",
                table: "UserTasks",
                column: "TaskId",
                principalTable: "Tasks",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_UserTasks_Users_UserId",
                table: "UserTasks",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserTasks_Tasks_TaskId",
                table: "UserTasks");

            migrationBuilder.DropForeignKey(
                name: "FK_UserTasks_Users_UserId",
                table: "UserTasks");

            migrationBuilder.DropPrimaryKey(
                name: "PK_UserTasks",
                table: "UserTasks");

            migrationBuilder.RenameTable(
                name: "UserTasks",
                newName: "TaskBoards");

            migrationBuilder.RenameIndex(
                name: "IX_UserTasks_UserId",
                table: "TaskBoards",
                newName: "IX_TaskBoards_UserId");

            migrationBuilder.RenameIndex(
                name: "IX_UserTasks_TaskId",
                table: "TaskBoards",
                newName: "IX_TaskBoards_TaskId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_TaskBoards",
                table: "TaskBoards",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_TaskBoards_Tasks_TaskId",
                table: "TaskBoards",
                column: "TaskId",
                principalTable: "Tasks",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_TaskBoards_Users_UserId",
                table: "TaskBoards",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
