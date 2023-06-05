using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace App.Core.Migrations
{
    public partial class RemoveRequiredCodeErrorDescription : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "ErrorDescription",
                table: "ErrorTypes",
                type: "nvarchar(650)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(650)");

            migrationBuilder.AlterColumn<string>(
                name: "Code",
                table: "ErrorTypes",
                type: "nvarchar(5)",
                maxLength: 5,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(5)",
                oldMaxLength: 5);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "ErrorDescription",
                table: "ErrorTypes",
                type: "nvarchar(650)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(650)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Code",
                table: "ErrorTypes",
                type: "nvarchar(5)",
                maxLength: 5,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(5)",
                oldMaxLength: 5,
                oldNullable: true);
        }
    }
}
