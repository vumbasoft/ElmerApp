using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace VumbaSoft.ErmanApp.Migrations
{
    /// <inheritdoc />
    public partial class Added_Country_ExtraFields : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "CCN3",
                table: "AppCountries",
                type: "nvarchar(6)",
                maxLength: 6,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Capital",
                table: "AppCountries",
                type: "nvarchar(128)",
                maxLength: 128,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Currency",
                table: "AppCountries",
                type: "nvarchar(12)",
                maxLength: 12,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Emoji",
                table: "AppCountries",
                type: "nvarchar(16)",
                maxLength: 16,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "EmojiU",
                table: "AppCountries",
                type: "nvarchar(16)",
                maxLength: 16,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "FormalName",
                table: "AppCountries",
                type: "nvarchar(128)",
                maxLength: 128,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "ISO2",
                table: "AppCountries",
                type: "nvarchar(4)",
                maxLength: 4,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "ISO3",
                table: "AppCountries",
                type: "nvarchar(6)",
                maxLength: 6,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "NativeName",
                table: "AppCountries",
                type: "nvarchar(128)",
                maxLength: 128,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "PhoneCode",
                table: "AppCountries",
                type: "nvarchar(16)",
                maxLength: 16,
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CCN3",
                table: "AppCountries");

            migrationBuilder.DropColumn(
                name: "Capital",
                table: "AppCountries");

            migrationBuilder.DropColumn(
                name: "Currency",
                table: "AppCountries");

            migrationBuilder.DropColumn(
                name: "Emoji",
                table: "AppCountries");

            migrationBuilder.DropColumn(
                name: "EmojiU",
                table: "AppCountries");

            migrationBuilder.DropColumn(
                name: "FormalName",
                table: "AppCountries");

            migrationBuilder.DropColumn(
                name: "ISO2",
                table: "AppCountries");

            migrationBuilder.DropColumn(
                name: "ISO3",
                table: "AppCountries");

            migrationBuilder.DropColumn(
                name: "NativeName",
                table: "AppCountries");

            migrationBuilder.DropColumn(
                name: "PhoneCode",
                table: "AppCountries");
        }
    }
}
