using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ControleDeAcesso.Migrations
{
    /// <inheritdoc />
    public partial class PreAuthTokenExpirationCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "PreAuthTokenExpiration",
                table: "AuthModels",
                type: "datetime(6)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "VerificationCodeExpiration",
                table: "AuthModels",
                type: "datetime(6)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PreAuthTokenExpiration",
                table: "AuthModels");

            migrationBuilder.DropColumn(
                name: "VerificationCodeExpiration",
                table: "AuthModels");
        }
    }
}
