using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace sat.bot.services.migrations
{
    /// <inheritdoc />
    public partial class urlDescarga : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "consulta@cfdisui",
                columns: table => new
                {
                    folioFiscal = table.Column<string>(type: "TEXT", maxLength: 64, nullable: false),
                    tipo = table.Column<int>(type: "NUMERIC", nullable: false),
                    rfcemisor = table.Column<string>(type: "TEXT", maxLength: 15, nullable: false),
                    nombreemisor = table.Column<string>(type: "TEXT", maxLength: 64, nullable: false),
                    rfcreceptor = table.Column<string>(type: "TEXT", maxLength: 15, nullable: false),
                    nombrereceptor = table.Column<string>(type: "TEXT", maxLength: 64, nullable: false),
                    fechaemision = table.Column<DateTime>(type: "TEXT", nullable: false),
                    fechacertificacion = table.Column<DateTime>(type: "TEXT", nullable: false),
                    paccertifico = table.Column<string>(type: "TEXT", maxLength: 25, nullable: false),
                    total = table.Column<decimal>(type: "NUMERIC", nullable: false),
                    efectocomprobante = table.Column<string>(type: "TEXT", maxLength: 64, nullable: false),
                    estatuscancelacion = table.Column<string>(type: "TEXT", maxLength: 64, nullable: false),
                    estadocomprobante = table.Column<string>(type: "TEXT", maxLength: 64, nullable: false),
                    statusprocesocancelacion = table.Column<string>(type: "TEXT", maxLength: 64, nullable: true),
                    fechaprocesocancelacion = table.Column<DateTime>(type: "TEXT", nullable: true),
                    motivo = table.Column<string>(type: "TEXT", maxLength: 128, nullable: true),
                    FolioSustitucion = table.Column<string>(type: "TEXT", maxLength: 64, nullable: true),
                    urldescarga = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_consulta@cfdisui", x => x.folioFiscal);
                });

            migrationBuilder.CreateTable(
                name: "rfc",
                columns: table => new
                {
                    rowid = table.Column<long>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    rfc = table.Column<string>(type: "TEXT", maxLength: 15, nullable: false),
                    nombre = table.Column<string>(type: "TEXT", maxLength: 250, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_rfc", x => x.rowid);
                });

            migrationBuilder.CreateTable(
                name: "cfdis",
                columns: table => new
                {
                    rowid = table.Column<long>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    uuid = table.Column<string>(type: "TEXT", maxLength: 64, nullable: false),
                    version = table.Column<string>(type: "TEXT", maxLength: 5, nullable: false),
                    emitido = table.Column<bool>(type: "INTEGER", nullable: false),
                    cancelado = table.Column<bool>(type: "INTEGER", nullable: false),
                    fechacfdi = table.Column<long>(type: "INTEGER", nullable: false),
                    iano = table.Column<int>(type: "INTEGER", nullable: false),
                    imes = table.Column<int>(type: "INTEGER", nullable: false),
                    idia = table.Column<int>(type: "INTEGER", nullable: false),
                    rfcid = table.Column<long>(type: "INTEGER", nullable: false),
                    subtotal = table.Column<decimal>(type: "NUMERIC", nullable: false),
                    total = table.Column<decimal>(type: "NUMERIC", nullable: false),
                    totaliretenidos = table.Column<decimal>(type: "NUMERIC", nullable: false),
                    totalitrasladados = table.Column<decimal>(type: "NUMERIC", nullable: false),
                    uso = table.Column<string>(type: "TEXT", maxLength: 5, nullable: true),
                    tieneiretenidos = table.Column<bool>(type: "INTEGER", nullable: false),
                    tieneitrasladados = table.Column<bool>(type: "INTEGER", nullable: false),
                    tienerelacionados = table.Column<bool>(type: "INTEGER", nullable: false),
                    tienei3os = table.Column<bool>(type: "INTEGER", maxLength: 5, nullable: false),
                    tieneinfoaduanera = table.Column<bool>(type: "INTEGER", nullable: false),
                    tienecpredial = table.Column<bool>(type: "INTEGER", nullable: false),
                    tienecomplementos = table.Column<bool>(type: "INTEGER", nullable: false),
                    tieneaddenda = table.Column<bool>(type: "INTEGER", nullable: false),
                    serie = table.Column<string>(type: "TEXT", maxLength: 100, nullable: true),
                    folio = table.Column<string>(type: "TEXT", maxLength: 100, nullable: true),
                    formapago = table.Column<string>(type: "TEXT", maxLength: 100, nullable: true),
                    moneda = table.Column<string>(type: "TEXT", maxLength: 5, nullable: true),
                    tipodecomprobante = table.Column<string>(type: "TEXT", maxLength: 5, nullable: false),
                    metodopago = table.Column<string>(type: "TEXT", maxLength: 5, nullable: true),
                    lugarexpedicion = table.Column<string>(type: "TEXT", maxLength: 5, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_cfdis", x => x.rowid);
                    table.ForeignKey(
                        name: "FK_cfdis_rfc_rfcid",
                        column: x => x.rfcid,
                        principalTable: "rfc",
                        principalColumn: "rowid",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_cfdis_rfcid",
                table: "cfdis",
                column: "rfcid");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "cfdis");

            migrationBuilder.DropTable(
                name: "consulta@cfdisui");

            migrationBuilder.DropTable(
                name: "rfc");
        }
    }
}
