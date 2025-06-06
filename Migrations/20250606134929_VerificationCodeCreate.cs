using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ControleDeAcesso.Migrations
{
    /// <inheritdoc />
    public partial class VerificationCodeCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "RefreshToken",
                table: "AuthModels",
                type: "longtext",
                nullable: false)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<string>(
                name: "VerificationCode",
                table: "AuthModels",
                type: "longtext",
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "RefreshToken",
                table: "AuthModels");

            migrationBuilder.DropColumn(
                name: "VerificationCode",
                table: "AuthModels");
        }
    }
}
