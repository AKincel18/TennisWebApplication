using Microsoft.EntityFrameworkCore.Migrations;

namespace TennisApplication.Migrations
{
    public partial class AddEnrolmentMigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Enrolments",
                columns: table => new
                {
                    TournamentId = table.Column<int>(nullable: false),
                    UserId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Enrolments", x => new { x.TournamentId, x.UserId });
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Enrolments");
        }
    }
}
