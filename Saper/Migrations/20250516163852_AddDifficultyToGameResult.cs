using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Saper.Migrations
{
    /// <inheritdoc />
    public partial class AddDifficultyToGameResult : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Difficulty",
                table: "GameResults",
                type: "TEXT",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Difficulty",
                table: "GameResults");
        }
    }
}
