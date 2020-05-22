using Microsoft.EntityFrameworkCore.Migrations;

namespace survey_polling.api.Data.Migrations
{
    public partial class AddConnectionIdToUser : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ConnectionId",
                table: "User",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ConnectionId",
                table: "User");
        }
    }
}
