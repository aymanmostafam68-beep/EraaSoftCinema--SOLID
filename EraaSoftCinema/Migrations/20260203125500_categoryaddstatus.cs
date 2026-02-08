using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EraaSoftCinema.Migrations
{
    /// <inheritdoc />
    public partial class categoryaddstatus : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "status",
                table: "MoviesCategories",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "status",
                table: "MoviesCategories");
        }
    }
}
