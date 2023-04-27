using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Backend.Migrations
{
    public partial class ChangeToEng : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
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
                name: "Anmerkungen");

            migrationBuilder.DropTable(
                name: "Diagnosen");

            migrationBuilder.DropTable(
                name: "Geraete");

            migrationBuilder.DropTable(
                name: "Leiterplattentypen");

            migrationBuilder.DropTable(
                name: "Nutzende");

            migrationBuilder.CreateTable(
                name: "AuditEntries",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Message = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: true),
                    Level = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    Exception = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DeletedDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AuditEntries", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Devices",
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
                    table.PrimaryKey("PK_Devices", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Diagnoses",
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
                    table.PrimaryKey("PK_Diagnoses", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ErrorTypes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Code = table.Column<string>(type: "nvarchar(5)", maxLength: 5, nullable: false),
                    ErrorDescribtion = table.Column<string>(type: "nvarchar(650)", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DeletedDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ErrorTypes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "PcbTypes",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    PcbPartNumber = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    MaxTransfer = table.Column<int>(type: "int", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DeletedDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PcbTypes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "StorageLocations",
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
                    table.PrimaryKey("PK_StorageLocations", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    AdUsername = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DeletedDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Comments",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(650)", nullable: false),
                    NotedById = table.Column<int>(type: "int", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DeletedDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Comments", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Comments_Users_NotedById",
                        column: x => x.NotedById,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Pcbs",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SerialNumber = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: true),
                    RestrictionId = table.Column<int>(type: "int", nullable: true),
                    ErrorDescription = table.Column<string>(type: "nvarchar(650)", nullable: true),
                    Finalized = table.Column<bool>(type: "bit", nullable: false),
                    PcbTypeId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    CommentId = table.Column<int>(type: "int", nullable: true),
                    EnddiagnoseId = table.Column<int>(type: "int", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DeletedDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Pcbs", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Pcbs_Comments_CommentId",
                        column: x => x.CommentId,
                        principalTable: "Comments",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Pcbs_Devices_RestrictionId",
                        column: x => x.RestrictionId,
                        principalTable: "Devices",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Pcbs_Diagnoses_EnddiagnoseId",
                        column: x => x.EnddiagnoseId,
                        principalTable: "Diagnoses",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Pcbs_PcbTypes_PcbTypeId",
                        column: x => x.PcbTypeId,
                        principalTable: "PcbTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

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

            migrationBuilder.CreateTable(
                name: "Transfers",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Anmerkung = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    NachId = table.Column<int>(type: "int", nullable: false),
                    VerbuchtVonId = table.Column<int>(type: "int", nullable: false),
                    LeiterplatteId = table.Column<int>(type: "int", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DeletedDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Transfers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Transfers_Pcbs_LeiterplatteId",
                        column: x => x.LeiterplatteId,
                        principalTable: "Pcbs",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Transfers_StorageLocations_NachId",
                        column: x => x.NachId,
                        principalTable: "StorageLocations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Transfers_Users_VerbuchtVonId",
                        column: x => x.VerbuchtVonId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Comments_NotedById",
                table: "Comments",
                column: "NotedById");

            migrationBuilder.CreateIndex(
                name: "IX_ErrorTypePcb_PcbsId",
                table: "ErrorTypePcb",
                column: "PcbsId");

            migrationBuilder.CreateIndex(
                name: "IX_Pcbs_CommentId",
                table: "Pcbs",
                column: "CommentId",
                unique: true,
                filter: "[CommentId] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Pcbs_EnddiagnoseId",
                table: "Pcbs",
                column: "EnddiagnoseId");

            migrationBuilder.CreateIndex(
                name: "IX_Pcbs_PcbTypeId",
                table: "Pcbs",
                column: "PcbTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_Pcbs_RestrictionId",
                table: "Pcbs",
                column: "RestrictionId");

            migrationBuilder.CreateIndex(
                name: "IX_Transfers_LeiterplatteId",
                table: "Transfers",
                column: "LeiterplatteId");

            migrationBuilder.CreateIndex(
                name: "IX_Transfers_NachId",
                table: "Transfers",
                column: "NachId");

            migrationBuilder.CreateIndex(
                name: "IX_Transfers_VerbuchtVonId",
                table: "Transfers",
                column: "VerbuchtVonId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AuditEntries");

            migrationBuilder.DropTable(
                name: "ErrorTypePcb");

            migrationBuilder.DropTable(
                name: "Transfers");

            migrationBuilder.DropTable(
                name: "ErrorTypes");

            migrationBuilder.DropTable(
                name: "Pcbs");

            migrationBuilder.DropTable(
                name: "StorageLocations");

            migrationBuilder.DropTable(
                name: "Comments");

            migrationBuilder.DropTable(
                name: "Devices");

            migrationBuilder.DropTable(
                name: "Diagnoses");

            migrationBuilder.DropTable(
                name: "PcbTypes");

            migrationBuilder.DropTable(
                name: "Users");

            migrationBuilder.CreateTable(
                name: "Diagnosen",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DeletedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(650)", nullable: false)
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
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DeletedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Fehlerbeschreibung = table.Column<string>(type: "nvarchar(650)", nullable: false)
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
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DeletedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(50)", nullable: false)
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
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DeletedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LagerName = table.Column<string>(type: "nvarchar(50)", nullable: false),
                    VerweildauerGelb = table.Column<int>(type: "int", nullable: false),
                    VerweildauerRot = table.Column<int>(type: "int", nullable: false)
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
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DeletedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LpSachnummer = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    MaxWeitergaben = table.Column<int>(type: "int", nullable: false)
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
                    AdUsername = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DeletedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Nutzende", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Anmerkungen",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    VermerktVonId = table.Column<int>(type: "int", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DeletedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(650)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Anmerkungen", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Anmerkungen_Nutzende_VermerktVonId",
                        column: x => x.VermerktVonId,
                        principalTable: "Nutzende",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Leiterplatten",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    EinschraenkungId = table.Column<int>(type: "int", nullable: false),
                    EnddiagnoseId = table.Column<int>(type: "int", nullable: false),
                    Leiterplatte = table.Column<int>(type: "int", nullable: false),
                    LeiterplattentypId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Abgeschlossen = table.Column<bool>(type: "bit", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DeletedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Fehlerbeschreibung = table.Column<string>(type: "nvarchar(650)", nullable: false),
                    SerienNummer = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Leiterplatten", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Leiterplatten_Anmerkungen_Leiterplatte",
                        column: x => x.Leiterplatte,
                        principalTable: "Anmerkungen",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
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
                    LeiterplatteId = table.Column<int>(type: "int", nullable: false),
                    NachId = table.Column<int>(type: "int", nullable: false),
                    VerbuchtVonId = table.Column<int>(type: "int", nullable: false),
                    Anmerkung = table.Column<string>(type: "nvarchar(max)", nullable: false),
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
                name: "IX_Leiterplatten_Leiterplatte",
                table: "Leiterplatten",
                column: "Leiterplatte",
                unique: true);

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
    }
}
