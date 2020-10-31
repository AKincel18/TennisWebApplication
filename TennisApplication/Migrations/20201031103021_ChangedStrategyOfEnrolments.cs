using Microsoft.EntityFrameworkCore.Migrations;

namespace TennisApplication.Migrations
{
    public partial class ChangedStrategyOfEnrolments : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_Enrolments_UserId",
                table: "Enrolments",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Enrolments_Tournaments_TournamentId",
                table: "Enrolments",
                column: "TournamentId",
                principalTable: "Tournaments",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Enrolments_Users_UserId",
                table: "Enrolments",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Enrolments_Tournaments_TournamentId",
                table: "Enrolments");

            migrationBuilder.DropForeignKey(
                name: "FK_Enrolments_Users_UserId",
                table: "Enrolments");

            migrationBuilder.DropIndex(
                name: "IX_Enrolments_UserId",
                table: "Enrolments");
        }
    }
}
