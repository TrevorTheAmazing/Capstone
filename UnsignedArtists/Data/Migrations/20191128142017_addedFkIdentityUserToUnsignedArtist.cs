using Microsoft.EntityFrameworkCore.Migrations;

namespace UnsignedArtists.Data.Migrations
{
    public partial class addedFkIdentityUserToUnsignedArtist : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "UserId",
                table: "UnsignedArtists",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_UnsignedArtists_UserId",
                table: "UnsignedArtists",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_UnsignedArtists_AspNetUsers_UserId",
                table: "UnsignedArtists",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UnsignedArtists_AspNetUsers_UserId",
                table: "UnsignedArtists");

            migrationBuilder.DropIndex(
                name: "IX_UnsignedArtists_UserId",
                table: "UnsignedArtists");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "UnsignedArtists");
        }
    }
}
