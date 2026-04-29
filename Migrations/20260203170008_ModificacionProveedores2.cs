using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Proyecto_Lola.Migrations
{
    /// <inheritdoc />
    public partial class ModificacionProveedores2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Anotacion",
                table: "Proveedores",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Anotacion",
                table: "Proveedores");
        }
    }
}
