using Microsoft.EntityFrameworkCore.Migrations;

namespace survey_polling.api.Data.Migrations
{
    public partial class RemoveLobbySeed : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Lobby",
                keyColumn: "Id",
                keyValue: 1);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Lobby",
                columns: new[] { "Id", "ActiveQuestionId", "HasStarted", "Pin", "PollId" },
                values: new object[] { 1, null, false, "241573", 1 });
        }
    }
}
