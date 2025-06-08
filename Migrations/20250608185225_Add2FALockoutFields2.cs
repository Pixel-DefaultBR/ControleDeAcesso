using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ControleDeAcesso.Migrations
{
    /// <inheritdoc />
    public partial class Add2FALockoutFields2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Failed2FAAttempts",
                table: "AuthModels",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<DateTime>(
                name: "TwoFABlockedUntil",
                table: "AuthModels",
                type: "datetime(6)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Failed2FAAttempts",
                table: "AuthModels");

            migrationBuilder.DropColumn(
                name: "TwoFABlockedUntil",
                table: "AuthModels");
        }
    }
}
