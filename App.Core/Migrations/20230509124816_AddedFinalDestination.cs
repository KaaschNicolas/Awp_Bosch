using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace App.Core.Migrations
{
    public partial class AddedFinalDestination : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "ErrorDescribtion",
                table: "ErrorTypes",
                newName: "ErrorDescription");

            migrationBuilder.RenameColumn(
                name: "Name",
                table: "Comments",
                newName: "Content");

            migrationBuilder.AddColumn<bool>(
                name: "IsFinalDestination",
                table: "StorageLocations",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsFinalDestination",
                table: "StorageLocations");

            migrationBuilder.RenameColumn(
                name: "ErrorDescription",
                table: "ErrorTypes",
                newName: "ErrorDescribtion");

            migrationBuilder.RenameColumn(
                name: "Content",
                table: "Comments",
                newName: "Name");
        }
    }
}
