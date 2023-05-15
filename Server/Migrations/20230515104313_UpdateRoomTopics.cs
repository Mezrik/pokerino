using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Pokerino.Server.Migrations
{
    /// <inheritdoc />
    public partial class UpdateRoomTopics : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "ShowVotes",
                table: "RoomTopic",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<int>(
                name: "ActiveTopicId",
                table: "Rooms",
                type: "integer",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "EstimateVote",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Username = table.Column<string>(type: "text", nullable: false),
                    Estimate = table.Column<int>(type: "integer", nullable: false),
                    RoomTopicId = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EstimateVote", x => x.Id);
                    table.ForeignKey(
                        name: "FK_EstimateVote_RoomTopic_RoomTopicId",
                        column: x => x.RoomTopicId,
                        principalTable: "RoomTopic",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Rooms_ActiveTopicId",
                table: "Rooms",
                column: "ActiveTopicId");

            migrationBuilder.CreateIndex(
                name: "IX_EstimateVote_RoomTopicId",
                table: "EstimateVote",
                column: "RoomTopicId");

            migrationBuilder.AddForeignKey(
                name: "FK_Rooms_RoomTopic_ActiveTopicId",
                table: "Rooms",
                column: "ActiveTopicId",
                principalTable: "RoomTopic",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Rooms_RoomTopic_ActiveTopicId",
                table: "Rooms");

            migrationBuilder.DropTable(
                name: "EstimateVote");

            migrationBuilder.DropIndex(
                name: "IX_Rooms_ActiveTopicId",
                table: "Rooms");

            migrationBuilder.DropColumn(
                name: "ShowVotes",
                table: "RoomTopic");

            migrationBuilder.DropColumn(
                name: "ActiveTopicId",
                table: "Rooms");
        }
    }
}
