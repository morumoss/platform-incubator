using Microsoft.EntityFrameworkCore.Migrations;

namespace survey_polling.api.Data.Migrations
{
    public partial class AddLobbyHasStartedBool : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "HasStarted",
                table: "Lobby",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "HasStarted",
                table: "Lobby");
        }
    }
}
