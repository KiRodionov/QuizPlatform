using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace QuizPlatform.Migrations
{
    /// <inheritdoc />
    public partial class AddPlayerAccessToken : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "AccessToken",
                table: "Players",
                type: "text",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AccessToken",
                table: "Players");
        }
    }
}
