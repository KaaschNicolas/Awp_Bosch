using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace App.Core.Migrations
{
    public partial class UpdatedErrorType : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ErrorTypePcb");

            migrationBuilder.AddColumn<int>(
                name: "PcbId",
                table: "ErrorTypes",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_ErrorTypes_PcbId",
                table: "ErrorTypes",
                column: "PcbId");

            migrationBuilder.AddForeignKey(
                name: "FK_ErrorTypes_Pcbs_PcbId",
                table: "ErrorTypes",
                column: "PcbId",
                principalTable: "Pcbs",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ErrorTypes_Pcbs_PcbId",
                table: "ErrorTypes");

            migrationBuilder.DropIndex(
                name: "IX_ErrorTypes_PcbId",
                table: "ErrorTypes");

            migrationBuilder.DropColumn(
                name: "PcbId",
                table: "ErrorTypes");

            migrationBuilder.CreateTable(
                name: "ErrorTypePcb",
                columns: table => new
                {
                    ErrorTypesId = table.Column<int>(type: "int", nullable: false),
                    PcbsId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ErrorTypePcb", x => new { x.ErrorTypesId, x.PcbsId });
                    table.ForeignKey(
                        name: "FK_ErrorTypePcb_ErrorTypes_ErrorTypesId",
                        column: x => x.ErrorTypesId,
                        principalTable: "ErrorTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ErrorTypePcb_Pcbs_PcbsId",
                        column: x => x.PcbsId,
                        principalTable: "Pcbs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ErrorTypePcb_PcbsId",
                table: "ErrorTypePcb",
                column: "PcbsId");
        }
    }
}
