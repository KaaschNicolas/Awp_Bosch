using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Backend.Migrations
{
    public partial class KeyToBaseEntity : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "VerweildauerRot",
                table: "StorageLocations",
                newName: "DwellTimeYellow");

            migrationBuilder.RenameColumn(
                name: "VerweildauerGelb",
                table: "StorageLocations",
                newName: "DwellTimeRed");

            migrationBuilder.RenameColumn(
                name: "LagerName",
                table: "StorageLocations",
                newName: "StorageName");

            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "PcbTypes",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Description",
                table: "PcbTypes");

            migrationBuilder.RenameColumn(
                name: "StorageName",
                table: "StorageLocations",
                newName: "LagerName");

            migrationBuilder.RenameColumn(
                name: "DwellTimeYellow",
                table: "StorageLocations",
                newName: "VerweildauerRot");

            migrationBuilder.RenameColumn(
                name: "DwellTimeRed",
                table: "StorageLocations",
                newName: "VerweildauerGelb");
        }
    }
}
