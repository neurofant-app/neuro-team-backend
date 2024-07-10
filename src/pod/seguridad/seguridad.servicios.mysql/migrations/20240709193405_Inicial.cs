using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace seguridad.servicios.mysql.migrations
{
    /// <inheritdoc />
    public partial class Inicial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterDatabase()
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "seguridad$aplicacion",
                columns: table => new
                {
                    ApplicacionId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    Nombre = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Descripcion = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_seguridad$aplicacion", x => x.ApplicacionId);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "seguridad$grupousuarios",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    DominioId = table.Column<string>(type: "varchar(200)", maxLength: 200, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    ApplicacionId = table.Column<string>(type: "varchar(200)", maxLength: 200, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    UsuarioId = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Nombre = table.Column<string>(type: "varchar(200)", maxLength: 200, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Descripcion = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    AplicacionApplicacionId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_seguridad$grupousuarios", x => x.Id);
                    table.ForeignKey(
                        name: "FK_seguridad$grupousuarios_seguridad$aplicacion_AplicacionAppli~",
                        column: x => x.AplicacionApplicacionId,
                        principalTable: "seguridad$aplicacion",
                        principalColumn: "ApplicacionId",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "seguridad$instanciaaplicacion",
                columns: table => new
                {
                    Id = table.Column<string>(type: "varchar(255)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    DominioId = table.Column<string>(type: "varchar(200)", maxLength: 200, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    ApplicacionId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_seguridad$instanciaaplicacion", x => x.Id);
                    table.ForeignKey(
                        name: "FK_seguridad$instanciaaplicacion_seguridad$aplicacion_Applicaci~",
                        column: x => x.ApplicacionId,
                        principalTable: "seguridad$aplicacion",
                        principalColumn: "ApplicacionId",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "seguridad$modulo",
                columns: table => new
                {
                    ModuloId = table.Column<string>(type: "varchar(255)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Nombre = table.Column<string>(type: "varchar(200)", maxLength: 200, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Descripcion = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    ApplicacionId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_seguridad$modulo", x => x.ModuloId);
                    table.ForeignKey(
                        name: "FK_seguridad$modulo_seguridad$aplicacion_ApplicacionId",
                        column: x => x.ApplicacionId,
                        principalTable: "seguridad$aplicacion",
                        principalColumn: "ApplicacionId",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "seguridad$usuariogrupo",
                columns: table => new
                {
                    Id = table.Column<string>(type: "varchar(255)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    UsuarioId = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    GrupoId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_seguridad$usuariogrupo", x => x.Id);
                    table.ForeignKey(
                        name: "FK_seguridad$usuariogrupo_seguridad$grupousuarios_GrupoId",
                        column: x => x.GrupoId,
                        principalTable: "seguridad$grupousuarios",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "seguridad$rol",
                columns: table => new
                {
                    RolId = table.Column<string>(type: "varchar(255)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Permisos = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Personalizado = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    Nombre = table.Column<string>(type: "varchar(200)", maxLength: 200, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Descripcion = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    ModuloId = table.Column<string>(type: "varchar(255)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    InstanciaAplicacionId = table.Column<string>(type: "varchar(255)", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_seguridad$rol", x => x.RolId);
                    table.ForeignKey(
                        name: "FK_seguridad$rol_seguridad$instanciaaplicacion_InstanciaAplicac~",
                        column: x => x.InstanciaAplicacionId,
                        principalTable: "seguridad$instanciaaplicacion",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_seguridad$rol_seguridad$modulo_ModuloId",
                        column: x => x.ModuloId,
                        principalTable: "seguridad$modulo",
                        principalColumn: "ModuloId",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "seguridad$permiso",
                columns: table => new
                {
                    PermisoId = table.Column<string>(type: "varchar(255)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Ambito = table.Column<int>(type: "int", nullable: false),
                    Nombre = table.Column<string>(type: "varchar(200)", maxLength: 200, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Descripcion = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    ModuloId = table.Column<string>(type: "varchar(255)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    RolId = table.Column<string>(type: "varchar(255)", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_seguridad$permiso", x => x.PermisoId);
                    table.ForeignKey(
                        name: "FK_seguridad$permiso_seguridad$modulo_ModuloId",
                        column: x => x.ModuloId,
                        principalTable: "seguridad$modulo",
                        principalColumn: "ModuloId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_seguridad$permiso_seguridad$rol_RolId",
                        column: x => x.RolId,
                        principalTable: "seguridad$rol",
                        principalColumn: "RolId",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "seguridad$rolUsuario",
                columns: table => new
                {
                    RolId = table.Column<string>(type: "varchar(255)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    UsuarioId = table.Column<string>(type: "varchar(255)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Id = table.Column<string>(type: "varchar(255)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_seguridad$rolUsuario", x => new { x.UsuarioId, x.RolId });
                    table.ForeignKey(
                        name: "FK_seguridad$rolUsuario_seguridad$instanciaaplicacion_Id",
                        column: x => x.Id,
                        principalTable: "seguridad$instanciaaplicacion",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_seguridad$rolUsuario_seguridad$rol_RolId",
                        column: x => x.RolId,
                        principalTable: "seguridad$rol",
                        principalColumn: "RolId",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "seguridad$rolgrupo",
                columns: table => new
                {
                    Id = table.Column<string>(type: "varchar(255)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    RolId = table.Column<string>(type: "varchar(255)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    GrupoId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_seguridad$rolgrupo", x => x.Id);
                    table.ForeignKey(
                        name: "FK_seguridad$rolgrupo_seguridad$grupousuarios_GrupoId",
                        column: x => x.GrupoId,
                        principalTable: "seguridad$grupousuarios",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_seguridad$rolgrupo_seguridad$instanciaaplicacion_Id",
                        column: x => x.Id,
                        principalTable: "seguridad$instanciaaplicacion",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_seguridad$rolgrupo_seguridad$rol_RolId",
                        column: x => x.RolId,
                        principalTable: "seguridad$rol",
                        principalColumn: "RolId",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "seguridad$permisoGrupo",
                columns: table => new
                {
                    Id = table.Column<string>(type: "varchar(255)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    PermisoId = table.Column<string>(type: "varchar(255)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    GrupoId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_seguridad$permisoGrupo", x => x.Id);
                    table.ForeignKey(
                        name: "FK_seguridad$permisoGrupo_seguridad$grupousuarios_GrupoId",
                        column: x => x.GrupoId,
                        principalTable: "seguridad$grupousuarios",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_seguridad$permisoGrupo_seguridad$instanciaaplicacion_Id",
                        column: x => x.Id,
                        principalTable: "seguridad$instanciaaplicacion",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_seguridad$permisoGrupo_seguridad$permiso_PermisoId",
                        column: x => x.PermisoId,
                        principalTable: "seguridad$permiso",
                        principalColumn: "PermisoId",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "seguridad$permisoUsuario",
                columns: table => new
                {
                    PermisoId = table.Column<string>(type: "varchar(255)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    UsuarioId = table.Column<string>(type: "varchar(255)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Id = table.Column<string>(type: "varchar(255)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_seguridad$permisoUsuario", x => new { x.UsuarioId, x.PermisoId });
                    table.ForeignKey(
                        name: "FK_seguridad$permisoUsuario_seguridad$instanciaaplicacion_Id",
                        column: x => x.Id,
                        principalTable: "seguridad$instanciaaplicacion",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_seguridad$permisoUsuario_seguridad$permiso_PermisoId",
                        column: x => x.PermisoId,
                        principalTable: "seguridad$permiso",
                        principalColumn: "PermisoId",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "IX_seguridad$grupousuarios_AplicacionApplicacionId",
                table: "seguridad$grupousuarios",
                column: "AplicacionApplicacionId");

            migrationBuilder.CreateIndex(
                name: "IX_seguridad$instanciaaplicacion_ApplicacionId",
                table: "seguridad$instanciaaplicacion",
                column: "ApplicacionId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_seguridad$modulo_ApplicacionId",
                table: "seguridad$modulo",
                column: "ApplicacionId");

            migrationBuilder.CreateIndex(
                name: "IX_seguridad$permiso_ModuloId",
                table: "seguridad$permiso",
                column: "ModuloId");

            migrationBuilder.CreateIndex(
                name: "IX_seguridad$permiso_RolId",
                table: "seguridad$permiso",
                column: "RolId");

            migrationBuilder.CreateIndex(
                name: "IX_seguridad$permisoGrupo_GrupoId",
                table: "seguridad$permisoGrupo",
                column: "GrupoId");

            migrationBuilder.CreateIndex(
                name: "IX_seguridad$permisoGrupo_PermisoId",
                table: "seguridad$permisoGrupo",
                column: "PermisoId");

            migrationBuilder.CreateIndex(
                name: "IX_seguridad$permisoUsuario_Id",
                table: "seguridad$permisoUsuario",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_seguridad$permisoUsuario_PermisoId",
                table: "seguridad$permisoUsuario",
                column: "PermisoId");

            migrationBuilder.CreateIndex(
                name: "IX_seguridad$rol_InstanciaAplicacionId",
                table: "seguridad$rol",
                column: "InstanciaAplicacionId");

            migrationBuilder.CreateIndex(
                name: "IX_seguridad$rol_ModuloId",
                table: "seguridad$rol",
                column: "ModuloId");

            migrationBuilder.CreateIndex(
                name: "IX_seguridad$rolUsuario_Id",
                table: "seguridad$rolUsuario",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_seguridad$rolUsuario_RolId",
                table: "seguridad$rolUsuario",
                column: "RolId");

            migrationBuilder.CreateIndex(
                name: "IX_seguridad$rolgrupo_GrupoId",
                table: "seguridad$rolgrupo",
                column: "GrupoId");

            migrationBuilder.CreateIndex(
                name: "IX_seguridad$rolgrupo_RolId",
                table: "seguridad$rolgrupo",
                column: "RolId");

            migrationBuilder.CreateIndex(
                name: "IX_seguridad$usuariogrupo_GrupoId",
                table: "seguridad$usuariogrupo",
                column: "GrupoId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "seguridad$permisoGrupo");

            migrationBuilder.DropTable(
                name: "seguridad$permisoUsuario");

            migrationBuilder.DropTable(
                name: "seguridad$rolUsuario");

            migrationBuilder.DropTable(
                name: "seguridad$rolgrupo");

            migrationBuilder.DropTable(
                name: "seguridad$usuariogrupo");

            migrationBuilder.DropTable(
                name: "seguridad$permiso");

            migrationBuilder.DropTable(
                name: "seguridad$grupousuarios");

            migrationBuilder.DropTable(
                name: "seguridad$rol");

            migrationBuilder.DropTable(
                name: "seguridad$instanciaaplicacion");

            migrationBuilder.DropTable(
                name: "seguridad$modulo");

            migrationBuilder.DropTable(
                name: "seguridad$aplicacion");
        }
    }
}
