using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace VumbaSoft.ErmanApp.Migrations
{
    /// <inheritdoc />
    public partial class Added_StateProvince_ExtraFields : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "RegionCode",
                table: "AppStateProvinces",
                type: "nvarchar(6)",
                maxLength: 6,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "StateProvinceCode",
                table: "AppStateProvinces",
                type: "nvarchar(6)",
                maxLength: 6,
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "RegionCode",
                table: "AppStateProvinces");

            migrationBuilder.DropColumn(
                name: "StateProvinceCode",
                table: "AppStateProvinces");
        }
    }
}
