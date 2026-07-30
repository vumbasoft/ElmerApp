using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace VumbaSoft.ErmanApp.Migrations
{
    /// <inheritdoc />
    public partial class Added_DistrictCity_ExtraFields : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "CountryCode",
                table: "AppDistrictCities",
                type: "nvarchar(3)",
                maxLength: 3,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<decimal>(
                name: "Latitude",
                table: "AppDistrictCities",
                type: "decimal(11,9)",
                precision: 11,
                scale: 9,
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "Longitude",
                table: "AppDistrictCities",
                type: "decimal(11,9)",
                precision: 11,
                scale: 9,
                nullable: false,
                defaultValue: 0m);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CountryCode",
                table: "AppDistrictCities");

            migrationBuilder.DropColumn(
                name: "Latitude",
                table: "AppDistrictCities");

            migrationBuilder.DropColumn(
                name: "Longitude",
                table: "AppDistrictCities");
        }
    }
}
