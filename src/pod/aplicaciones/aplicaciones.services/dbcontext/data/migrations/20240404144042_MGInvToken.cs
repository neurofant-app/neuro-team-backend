using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace aplicaciones.services.dbcontext.data.migrations
{
    /// <inheritdoc />
    public partial class MGInvToken : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Tipo",
                table: "aplicacion$invitaciones",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "Token",
                table: "aplicacion$invitaciones",
                type: "longtext",
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Tipo",
                table: "aplicacion$invitaciones");

            migrationBuilder.DropColumn(
                name: "Token",
                table: "aplicacion$invitaciones");
        }
    }
}
