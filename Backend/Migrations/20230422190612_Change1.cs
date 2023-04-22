using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Backend.Migrations
{
    /// <inheritdoc />
    public partial class Change1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Anmerkungen_Leiterplatten_Anmerkung",
                table: "Anmerkungen");

            migrationBuilder.DropColumn(
                name: "Anmerkung",
                table: "Anmerkungen");

            migrationBuilder.AddColumn<int>(
                name: "AnmerkungId",
                table: "Leiterplatten",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddForeignKey(
                name: "FK_Leiterplatten_Anmerkungen_Leiterplatte",
                table: "Leiterplatten",
                column: "AnmerkungId",
                principalTable: "Anmerkungen",
                principalColumn: "Id",
                onDelete: ReferentialAction.NoAction);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Leiterplatten_Anmerkungen_Leiterplatte",
                table: "Leiterplatten");

            migrationBuilder.DropIndex(
                name: "IX_Leiterplatten_Leiterplatte",
                table: "Leiterplatten");

            migrationBuilder.DropColumn(
                name: "Leiterplatte",
                table: "Leiterplatten");

            migrationBuilder.AddColumn<int>(
                name: "Anmerkung",
                table: "Anmerkungen",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Anmerkungen_Anmerkung",
                table: "Anmerkungen",
                column: "Anmerkung",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Anmerkungen_Leiterplatten_Anmerkung",
                table: "Anmerkungen",
                column: "Anmerkung",
                principalTable: "Leiterplatten",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
