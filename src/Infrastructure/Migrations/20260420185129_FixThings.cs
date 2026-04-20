using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace UrlShortener.Migrations
{
    /// <inheritdoc />
    public partial class FixThings : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "DateModified",
                table: "Urls",
                newName: "LastModified"
            );

            migrationBuilder.RenameColumn(name: "DateCreated", table: "Urls", newName: "Created");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "LastModified",
                table: "Urls",
                newName: "DateModified"
            );

            migrationBuilder.RenameColumn(name: "Created", table: "Urls", newName: "DateCreated");
        }
    }
}
