using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Backend.Migrations
{
    /// <inheritdoc />
    public partial class Init : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Diagnosen",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(650)", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DeletedDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Diagnosen", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Fehlertypen",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Code = table.Column<string>(type: "nvarchar(5)", maxLength: 5, nullable: false),
                    Fehlerbeschreibung = table.Column<string>(type: "nvarchar(650)", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DeletedDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Fehlertypen", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Geraete",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(50)", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DeletedDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Geraete", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "LagerOrte",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    LagerName = table.Column<string>(type: "nvarchar(50)", nullable: false),
                    VerweildauerGelb = table.Column<int>(type: "int", nullable: false),
                    VerweildauerRot = table.Column<int>(type: "int", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DeletedDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LagerOrte", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Leiterplattentypen",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    LpSachnummer = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    MaxWeitergaben = table.Column<int>(type: "int", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DeletedDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Leiterplattentypen", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Nutzende",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    AdUsername = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DeletedDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Nutzende", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Leiterplatten",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SerienNummer = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    EinschraenkungId = table.Column<int>(type: "int", nullable: false),
                    Fehlerbeschreibung = table.Column<string>(type: "nvarchar(650)", nullable: false),
                    Abgeschlossen = table.Column<bool>(type: "bit", nullable: false),
                    LeiterplattentypId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    EnddiagnoseId = table.Column<int>(type: "int", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DeletedDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Leiterplatten", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Leiterplatten_Diagnosen_EnddiagnoseId",
                        column: x => x.EnddiagnoseId,
                        principalTable: "Diagnosen",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Leiterplatten_Geraete_EinschraenkungId",
                        column: x => x.EinschraenkungId,
                        principalTable: "Geraete",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Leiterplatten_Leiterplattentypen_LeiterplattentypId",
                        column: x => x.LeiterplattentypId,
                        principalTable: "Leiterplattentypen",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Anmerkungen",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(650)", nullable: false),
                    VermerktVonId = table.Column<int>(type: "int", nullable: false),
                    Anmerkung = table.Column<int>(type: "int", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DeletedDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Anmerkungen", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Anmerkungen_Leiterplatten_Anmerkung",
                        column: x => x.Anmerkung,
                        principalTable: "Leiterplatten",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Anmerkungen_Nutzende_VermerktVonId",
                        column: x => x.VermerktVonId,
                        principalTable: "Nutzende",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "FehlertypLeiterplatte",
                columns: table => new
                {
                    FehlertypenId = table.Column<int>(type: "int", nullable: false),
                    LeiterplattenId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FehlertypLeiterplatte", x => new { x.FehlertypenId, x.LeiterplattenId });
                    table.ForeignKey(
                        name: "FK_FehlertypLeiterplatte_Fehlertypen_FehlertypenId",
                        column: x => x.FehlertypenId,
                        principalTable: "Fehlertypen",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_FehlertypLeiterplatte_Leiterplatten_LeiterplattenId",
                        column: x => x.LeiterplattenId,
                        principalTable: "Leiterplatten",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Umbuchungen",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Anmerkung = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    NachId = table.Column<int>(type: "int", nullable: false),
                    VerbuchtVonId = table.Column<int>(type: "int", nullable: false),
                    LeiterplatteId = table.Column<int>(type: "int", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DeletedDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Umbuchungen", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Umbuchungen_LagerOrte_NachId",
                        column: x => x.NachId,
                        principalTable: "LagerOrte",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Umbuchungen_Leiterplatten_LeiterplatteId",
                        column: x => x.LeiterplatteId,
                        principalTable: "Leiterplatten",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Umbuchungen_Nutzende_VerbuchtVonId",
                        column: x => x.VerbuchtVonId,
                        principalTable: "Nutzende",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Anmerkungen_Anmerkung",
                table: "Anmerkungen",
                column: "Anmerkung",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Anmerkungen_VermerktVonId",
                table: "Anmerkungen",
                column: "VermerktVonId");

            migrationBuilder.CreateIndex(
                name: "IX_FehlertypLeiterplatte_LeiterplattenId",
                table: "FehlertypLeiterplatte",
                column: "LeiterplattenId");

            migrationBuilder.CreateIndex(
                name: "IX_Leiterplatten_EinschraenkungId",
                table: "Leiterplatten",
                column: "EinschraenkungId");

            migrationBuilder.CreateIndex(
                name: "IX_Leiterplatten_EnddiagnoseId",
                table: "Leiterplatten",
                column: "EnddiagnoseId");

            migrationBuilder.CreateIndex(
                name: "IX_Leiterplatten_LeiterplattentypId",
                table: "Leiterplatten",
                column: "LeiterplattentypId");

            migrationBuilder.CreateIndex(
                name: "IX_Umbuchungen_LeiterplatteId",
                table: "Umbuchungen",
                column: "LeiterplatteId");

            migrationBuilder.CreateIndex(
                name: "IX_Umbuchungen_NachId",
                table: "Umbuchungen",
                column: "NachId");

            migrationBuilder.CreateIndex(
                name: "IX_Umbuchungen_VerbuchtVonId",
                table: "Umbuchungen",
                column: "VerbuchtVonId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Anmerkungen");

            migrationBuilder.DropTable(
                name: "FehlertypLeiterplatte");

            migrationBuilder.DropTable(
                name: "Umbuchungen");

            migrationBuilder.DropTable(
                name: "Fehlertypen");

            migrationBuilder.DropTable(
                name: "LagerOrte");

            migrationBuilder.DropTable(
                name: "Leiterplatten");

            migrationBuilder.DropTable(
                name: "Nutzende");

            migrationBuilder.DropTable(
                name: "Diagnosen");

            migrationBuilder.DropTable(
                name: "Geraete");

            migrationBuilder.DropTable(
                name: "Leiterplattentypen");
        }
    }
}
