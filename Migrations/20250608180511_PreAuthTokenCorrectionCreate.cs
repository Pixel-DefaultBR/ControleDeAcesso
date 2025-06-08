using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ControleDeAcesso.Migrations
{
    /// <inheritdoc />
    public partial class PreAuthTokenCorrectionCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "PreAuthToken",
                table: "AuthModels",
                type: "longtext",
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PreAuthToken",
                table: "AuthModels");
        }
    }
}
