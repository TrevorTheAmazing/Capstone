using Microsoft.EntityFrameworkCore.Migrations;

namespace test.Data.Migrations
{
    public partial class labelledGenre : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "labelledGenre",
                table: "Submissions",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "labelledGenre",
                table: "Submissions");
        }
    }
}
