using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace VumbaSoft.ErmanApp.Migrations
{
    /// <inheritdoc />
    public partial class Added_Locality_ExtraFields : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "DistrictCityCode",
                table: "AppLocalities",
                type: "nvarchar(3)",
                maxLength: 3,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<decimal>(
                name: "Latitude",
                table: "AppLocalities",
                type: "decimal(11,9)",
                precision: 11,
                scale: 9,
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<string>(
                name: "LocalityCode",
                table: "AppLocalities",
                type: "nvarchar(3)",
                maxLength: 3,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<decimal>(
                name: "Longitude",
                table: "AppLocalities",
                type: "decimal(12,9)",
                precision: 12,
                scale: 9,
                nullable: false,
                defaultValue: 0m);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DistrictCityCode",
                table: "AppLocalities");

            migrationBuilder.DropColumn(
                name: "Latitude",
                table: "AppLocalities");

            migrationBuilder.DropColumn(
                name: "LocalityCode",
                table: "AppLocalities");

            migrationBuilder.DropColumn(
                name: "Longitude",
                table: "AppLocalities");
        }
    }
}
