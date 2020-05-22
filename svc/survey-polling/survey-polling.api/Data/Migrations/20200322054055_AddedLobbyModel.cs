using Microsoft.EntityFrameworkCore.Migrations;

namespace survey_polling.api.Data.Migrations
{
    public partial class AddedLobbyModel : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsActive",
                table: "Poll");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "User",
                maxLength: 40,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AddColumn<int>(
                name: "LobbyId",
                table: "User",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Lobby",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PollId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Lobby", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Lobby_Poll_PollId",
                        column: x => x.PollId,
                        principalTable: "Poll",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_User_LobbyId",
                table: "User",
                column: "LobbyId");

            migrationBuilder.CreateIndex(
                name: "IX_Lobby_PollId",
                table: "Lobby",
                column: "PollId");

            migrationBuilder.AddForeignKey(
                name: "FK_User_Lobby_LobbyId",
                table: "User",
                column: "LobbyId",
                principalTable: "Lobby",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_User_Lobby_LobbyId",
                table: "User");

            migrationBuilder.DropTable(
                name: "Lobby");

            migrationBuilder.DropIndex(
                name: "IX_User_LobbyId",
                table: "User");

            migrationBuilder.DropColumn(
                name: "LobbyId",
                table: "User");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "User",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldMaxLength: 40);

            migrationBuilder.AddColumn<bool>(
                name: "IsActive",
                table: "Poll",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }
    }
}
