using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Pokerino.Server.Migrations
{
    /// <inheritdoc />
    public partial class Init : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Username = table.Column<string>(type: "text", nullable: false),
                    Email = table.Column<string>(type: "text", nullable: false),
                    Password = table.Column<string>(type: "text", nullable: false),
                    PasswordSalt = table.Column<byte[]>(type: "bytea", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                });

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
                });

            migrationBuilder.CreateTable(
                name: "Rooms",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    PublicId = table.Column<string>(type: "text", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false),
                    Status = table.Column<int>(type: "integer", nullable: false),
                    ActiveTopicId = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Rooms", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "RoomTopic",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "text", nullable: false),
                    Description = table.Column<string>(type: "text", nullable: false),
                    ShowVotes = table.Column<bool>(type: "boolean", nullable: false),
                    Estimate = table.Column<double>(type: "double precision", nullable: true),
                    RoomId = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RoomTopic", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RoomTopic_Rooms_RoomId",
                        column: x => x.RoomId,
                        principalTable: "Rooms",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "RoomUser",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    UserId = table.Column<int>(type: "integer", nullable: true),
                    Name = table.Column<string>(type: "text", nullable: true),
                    Role = table.Column<int>(type: "integer", nullable: false),
                    RoomId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RoomUser", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RoomUser_Rooms_RoomId",
                        column: x => x.RoomId,
                        principalTable: "Rooms",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_RoomUser_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_EstimateVote_RoomTopicId",
                table: "EstimateVote",
                column: "RoomTopicId");

            migrationBuilder.CreateIndex(
                name: "IX_Rooms_ActiveTopicId",
                table: "Rooms",
                column: "ActiveTopicId");

            migrationBuilder.CreateIndex(
                name: "IX_RoomTopic_RoomId",
                table: "RoomTopic",
                column: "RoomId");

            migrationBuilder.CreateIndex(
                name: "IX_RoomUser_RoomId",
                table: "RoomUser",
                column: "RoomId");

            migrationBuilder.CreateIndex(
                name: "IX_RoomUser_UserId",
                table: "RoomUser",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_EstimateVote_RoomTopic_RoomTopicId",
                table: "EstimateVote",
                column: "RoomTopicId",
                principalTable: "RoomTopic",
                principalColumn: "Id");

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

            migrationBuilder.DropTable(
                name: "RoomUser");

            migrationBuilder.DropTable(
                name: "Users");

            migrationBuilder.DropTable(
                name: "RoomTopic");

            migrationBuilder.DropTable(
                name: "Rooms");
        }
    }
}
