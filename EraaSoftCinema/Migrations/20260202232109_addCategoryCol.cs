using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EraaSoftCinema.Migrations
{
    /// <inheritdoc />
    public partial class addCategoryCol : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "imgUrl",
                table: "MoviesCategories",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "imgUrl",
                table: "MoviesCategories");
        }
    }
}
