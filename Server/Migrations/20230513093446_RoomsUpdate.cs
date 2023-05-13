using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Pokerino.Server.Migrations
{
    /// <inheritdoc />
    public partial class RoomsUpdate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_RoomUser_Users_UserId",
                table: "RoomUser");

            migrationBuilder.AlterColumn<int>(
                name: "UserId",
                table: "RoomUser",
                type: "integer",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "RoomUser",
                type: "text",
                nullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_RoomUser_Users_UserId",
                table: "RoomUser",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_RoomUser_Users_UserId",
                table: "RoomUser");

            migrationBuilder.DropColumn(
                name: "Name",
                table: "RoomUser");

            migrationBuilder.AlterColumn<int>(
                name: "UserId",
                table: "RoomUser",
                type: "integer",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "integer",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_RoomUser_Users_UserId",
                table: "RoomUser",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
