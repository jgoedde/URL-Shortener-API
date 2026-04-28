using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace UrlShortener.Migrations
{
    /// <inheritdoc />
    public partial class AddUserFieldToUrl : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "CreatedById",
                table: "Urls",
                type: "text",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Urls_CreatedById",
                table: "Urls",
                column: "CreatedById");

            migrationBuilder.AddForeignKey(
                name: "FK_Urls_AspNetUsers_CreatedById",
                table: "Urls",
                column: "CreatedById",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Urls_AspNetUsers_CreatedById",
                table: "Urls");

            migrationBuilder.DropIndex(
                name: "IX_Urls_CreatedById",
                table: "Urls");

            migrationBuilder.DropColumn(
                name: "CreatedById",
                table: "Urls");
        }
    }
}
