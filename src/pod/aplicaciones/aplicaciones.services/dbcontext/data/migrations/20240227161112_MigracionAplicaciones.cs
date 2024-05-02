using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace aplicaciones.services.dbcontext.data.migrations
{
    /// <inheritdoc />
    public partial class MigracionAplicaciones : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterDatabase()
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "aplicacion$aplicaciones",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    Nombre = table.Column<string>(type: "varchar(200)", maxLength: 200, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Activa = table.Column<bool>(type: "tinyint(1)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_aplicacion$aplicaciones", x => x.Id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "aplicacion$consentimientos",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    AplicacionId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    Tipo = table.Column<int>(type: "int", nullable: false),
                    Idioma = table.Column<string>(type: "varchar(10)", maxLength: 10, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    IdiomaDefault = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    Texto = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_aplicacion$consentimientos", x => x.Id);
                    table.ForeignKey(
                        name: "FK_aplicacion$consentimientos_aplicacion$aplicaciones_Aplicacio~",
                        column: x => x.AplicacionId,
                        principalTable: "aplicacion$aplicaciones",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "aplicacion$invitaciones",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    AplicacionId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    Fecha = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    Estado = table.Column<int>(type: "int", nullable: false),
                    Email = table.Column<string>(type: "varchar(250)", maxLength: 250, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    RolId = table.Column<int>(type: "int", nullable: false),
                    Nombre = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_aplicacion$invitaciones", x => x.Id);
                    table.ForeignKey(
                        name: "FK_aplicacion$invitaciones_aplicacion$aplicaciones_AplicacionId",
                        column: x => x.AplicacionId,
                        principalTable: "aplicacion$aplicaciones",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "aplicacion$logosaplicaciones",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    AplicacionId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    Tipo = table.Column<int>(type: "int", nullable: false),
                    Idioma = table.Column<string>(type: "varchar(10)", maxLength: 10, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    IdiomaDefault = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    LogoURLBase64 = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    EsSVG = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    EsUrl = table.Column<bool>(type: "tinyint(1)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_aplicacion$logosaplicaciones", x => x.Id);
                    table.ForeignKey(
                        name: "FK_aplicacion$logosaplicaciones_aplicacion$aplicaciones_Aplicac~",
                        column: x => x.AplicacionId,
                        principalTable: "aplicacion$aplicaciones",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "aplicacion$plantillasaplicaciones",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    TipoContenido = table.Column<int>(type: "int", nullable: false),
                    AplicacionId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    Plantilla = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_aplicacion$plantillasaplicaciones", x => x.Id);
                    table.ForeignKey(
                        name: "FK_aplicacion$plantillasaplicaciones_aplicacion$aplicaciones_Ap~",
                        column: x => x.AplicacionId,
                        principalTable: "aplicacion$aplicaciones",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "IX_aplicacion$consentimientos_AplicacionId",
                table: "aplicacion$consentimientos",
                column: "AplicacionId");

            migrationBuilder.CreateIndex(
                name: "IX_aplicacion$invitaciones_AplicacionId",
                table: "aplicacion$invitaciones",
                column: "AplicacionId");

            migrationBuilder.CreateIndex(
                name: "IX_aplicacion$logosaplicaciones_AplicacionId",
                table: "aplicacion$logosaplicaciones",
                column: "AplicacionId");

            migrationBuilder.CreateIndex(
                name: "IX_aplicacion$plantillasaplicaciones_AplicacionId",
                table: "aplicacion$plantillasaplicaciones",
                column: "AplicacionId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "aplicacion$consentimientos");

            migrationBuilder.DropTable(
                name: "aplicacion$invitaciones");

            migrationBuilder.DropTable(
                name: "aplicacion$logosaplicaciones");

            migrationBuilder.DropTable(
                name: "aplicacion$plantillasaplicaciones");

            migrationBuilder.DropTable(
                name: "aplicacion$aplicaciones");
        }
    }
}
