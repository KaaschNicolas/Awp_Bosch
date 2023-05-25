using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace App.Core.Migrations
{
    public partial class AddExplicitPcbFKToTransfer : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Transfers_Pcbs_PcbId",
                table: "Transfers");

            migrationBuilder.AlterColumn<int>(
                name: "PcbId",
                table: "Transfers",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Transfers_Pcbs_PcbId",
                table: "Transfers",
                column: "PcbId",
                principalTable: "Pcbs",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Transfers_Pcbs_PcbId",
                table: "Transfers");

            migrationBuilder.AlterColumn<int>(
                name: "PcbId",
                table: "Transfers",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddForeignKey(
                name: "FK_Transfers_Pcbs_PcbId",
                table: "Transfers",
                column: "PcbId",
                principalTable: "Pcbs",
                principalColumn: "Id");
        }
    }
}
