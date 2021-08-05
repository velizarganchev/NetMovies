using Microsoft.EntityFrameworkCore.Migrations;

namespace NetMovies.Migrations
{
    public partial class AddWatchProperty : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "WatchUrl",
                table: "Movies",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "WatchUrl",
                table: "Movies");
        }
    }
}
