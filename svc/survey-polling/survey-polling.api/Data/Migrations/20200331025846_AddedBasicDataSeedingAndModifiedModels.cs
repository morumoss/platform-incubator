using Microsoft.EntityFrameworkCore.Migrations;

namespace survey_polling.api.Data.Migrations
{
    public partial class AddedBasicDataSeedingAndModifiedModels : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Option_Poll_PollId",
                table: "Option");

            migrationBuilder.DropForeignKey(
                name: "FK_Option_Question_QuestionId",
                table: "Option");

            migrationBuilder.DropForeignKey(
                name: "FK_Poll_User_HostId",
                table: "Poll");

            migrationBuilder.DropForeignKey(
                name: "FK_Question_Poll_PollId",
                table: "Question");

            migrationBuilder.DropForeignKey(
                name: "FK_Vote_Option_OptionId",
                table: "Vote");

            migrationBuilder.DropColumn(
                name: "Content",
                table: "Question");

            migrationBuilder.AddColumn<string>(
                name: "Title",
                table: "Question",
                maxLength: 100,
                nullable: false,
                defaultValue: "");

            migrationBuilder.InsertData(
                table: "User",
                columns: new[] { "Id", "IsHost", "LobbyId" },
                values: new object[] { 1, true, null });

            migrationBuilder.InsertData(
                table: "User",
                columns: new[] { "Id", "LobbyId" },
                values: new object[] { 2, null });

            migrationBuilder.InsertData(
                table: "Poll",
                columns: new[] { "Id", "HostId", "Title" },
                values: new object[] { 1, 1, "Test poll #1" });

            migrationBuilder.InsertData(
                table: "Poll",
                columns: new[] { "Id", "HostId", "Title" },
                values: new object[] { 2, 1, "Test poll #2" });

            migrationBuilder.InsertData(
                table: "Question",
                columns: new[] { "Id", "PollId", "Title" },
                values: new object[] { 1, 1, "What is this symbol: =>" });

            migrationBuilder.InsertData(
                table: "Question",
                columns: new[] { "Id", "PollId", "Title" },
                values: new object[] { 2, 1, "What is 4/2?" });

            migrationBuilder.InsertData(
                table: "Question",
                columns: new[] { "Id", "PollId", "Title" },
                values: new object[] { 3, 2, "What is 1+1?" });

            migrationBuilder.InsertData(
                table: "Option",
                columns: new[] { "Id", "Content", "PollId", "QuestionId" },
                values: new object[,]
                {
                    { 1, "Nothing", 1, 1 },
                    { 2, "Greater than or equal to", 1, 1 },
                    { 3, "Lambda", 1, 1 },
                    { 4, "1", 1, 2 },
                    { 5, "2", 1, 2 }
                });

            migrationBuilder.AddForeignKey(
                name: "FK_Option_Poll_PollId",
                table: "Option",
                column: "PollId",
                principalTable: "Poll",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Option_Question_QuestionId",
                table: "Option",
                column: "QuestionId",
                principalTable: "Question",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Poll_User_HostId",
                table: "Poll",
                column: "HostId",
                principalTable: "User",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Question_Poll_PollId",
                table: "Question",
                column: "PollId",
                principalTable: "Poll",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Vote_Option_OptionId",
                table: "Vote",
                column: "OptionId",
                principalTable: "Option",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Option_Poll_PollId",
                table: "Option");

            migrationBuilder.DropForeignKey(
                name: "FK_Option_Question_QuestionId",
                table: "Option");

            migrationBuilder.DropForeignKey(
                name: "FK_Poll_User_HostId",
                table: "Poll");

            migrationBuilder.DropForeignKey(
                name: "FK_Question_Poll_PollId",
                table: "Question");

            migrationBuilder.DropForeignKey(
                name: "FK_Vote_Option_OptionId",
                table: "Vote");

            migrationBuilder.DeleteData(
                table: "Option",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Option",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Option",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Option",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "Option",
                keyColumn: "Id",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "Question",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "User",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Poll",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Question",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Question",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Poll",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "User",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DropColumn(
                name: "Title",
                table: "Question");

            migrationBuilder.AddColumn<string>(
                name: "Content",
                table: "Question",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddForeignKey(
                name: "FK_Option_Poll_PollId",
                table: "Option",
                column: "PollId",
                principalTable: "Poll",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Option_Question_QuestionId",
                table: "Option",
                column: "QuestionId",
                principalTable: "Question",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Poll_User_HostId",
                table: "Poll",
                column: "HostId",
                principalTable: "User",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Question_Poll_PollId",
                table: "Question",
                column: "PollId",
                principalTable: "Poll",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Vote_Option_OptionId",
                table: "Vote",
                column: "OptionId",
                principalTable: "Option",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
