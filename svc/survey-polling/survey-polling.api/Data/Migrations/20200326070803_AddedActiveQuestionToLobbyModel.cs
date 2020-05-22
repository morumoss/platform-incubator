using Microsoft.EntityFrameworkCore.Migrations;

namespace survey_polling.api.Data.Migrations
{
    public partial class AddedActiveQuestionToLobbyModel : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ActiveQuestionId",
                table: "Lobby",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Lobby_ActiveQuestionId",
                table: "Lobby",
                column: "ActiveQuestionId",
                unique: true,
                filter: "[ActiveQuestionId] IS NOT NULL");

            migrationBuilder.AddForeignKey(
                name: "FK_Lobby_Question_ActiveQuestionId",
                table: "Lobby",
                column: "ActiveQuestionId",
                principalTable: "Question",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Lobby_Question_ActiveQuestionId",
                table: "Lobby");

            migrationBuilder.DropIndex(
                name: "IX_Lobby_ActiveQuestionId",
                table: "Lobby");

            migrationBuilder.DropColumn(
                name: "ActiveQuestionId",
                table: "Lobby");
        }
    }
}
