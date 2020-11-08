using Microsoft.EntityFrameworkCore.Migrations;

namespace TennisApplication.Migrations
{
    public partial class ChangedNameOfColumnInTournaments : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PlayersNumber",
                table: "Tournaments");

            migrationBuilder.AddColumn<int>(
                name: "DrawSize",
                table: "Tournaments",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DrawSize",
                table: "Tournaments");

            migrationBuilder.AddColumn<int>(
                name: "PlayersNumber",
                table: "Tournaments",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
