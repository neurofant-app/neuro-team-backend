using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace aplicaciones.services.dbcontext.data.migrations
{
    /// <inheritdoc />
    public partial class MGApps : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_aplicacion$plantillasaplicaciones_aplicacion$aplicaciones_Ap~",
                table: "aplicacion$plantillasaplicaciones");

            migrationBuilder.DropPrimaryKey(
                name: "PK_aplicacion$plantillasaplicaciones",
                table: "aplicacion$plantillasaplicaciones");

            migrationBuilder.RenameTable(
                name: "aplicacion$plantillasaplicaciones",
                newName: "aplicacion$plantillasinvitaciones");

            migrationBuilder.RenameIndex(
                name: "IX_aplicacion$plantillasaplicaciones_AplicacionId",
                table: "aplicacion$plantillasinvitaciones",
                newName: "IX_aplicacion$plantillasinvitaciones_AplicacionId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_aplicacion$plantillasinvitaciones",
                table: "aplicacion$plantillasinvitaciones",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_aplicacion$plantillasinvitaciones_aplicacion$aplicaciones_Ap~",
                table: "aplicacion$plantillasinvitaciones",
                column: "AplicacionId",
                principalTable: "aplicacion$aplicaciones",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_aplicacion$plantillasinvitaciones_aplicacion$aplicaciones_Ap~",
                table: "aplicacion$plantillasinvitaciones");

            migrationBuilder.DropPrimaryKey(
                name: "PK_aplicacion$plantillasinvitaciones",
                table: "aplicacion$plantillasinvitaciones");

            migrationBuilder.RenameTable(
                name: "aplicacion$plantillasinvitaciones",
                newName: "aplicacion$plantillasaplicaciones");

            migrationBuilder.RenameIndex(
                name: "IX_aplicacion$plantillasinvitaciones_AplicacionId",
                table: "aplicacion$plantillasaplicaciones",
                newName: "IX_aplicacion$plantillasaplicaciones_AplicacionId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_aplicacion$plantillasaplicaciones",
                table: "aplicacion$plantillasaplicaciones",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_aplicacion$plantillasaplicaciones_aplicacion$aplicaciones_Ap~",
                table: "aplicacion$plantillasaplicaciones",
                column: "AplicacionId",
                principalTable: "aplicacion$aplicaciones",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
