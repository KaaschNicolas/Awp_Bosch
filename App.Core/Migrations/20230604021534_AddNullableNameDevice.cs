using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace App.Core.Migrations
{
    public partial class AddNullableNameDevice : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "Diagnoses",
                type: "nvarchar(650)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(650)");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "Diagnoses",
                type: "nvarchar(650)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(650)",
                oldNullable: true);
        }
    }
}
