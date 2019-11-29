using Microsoft.EntityFrameworkCore.Migrations;

namespace test.Data.Migrations
{
    public partial class AddedGuidToSubmissions : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "guid",
                table: "Submissions",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "originalFilename",
                table: "Submissions",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "guid",
                table: "Submissions");

            migrationBuilder.DropColumn(
                name: "originalFilename",
                table: "Submissions");
        }
    }
}
