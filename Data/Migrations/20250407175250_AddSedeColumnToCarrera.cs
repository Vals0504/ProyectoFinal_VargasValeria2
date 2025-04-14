using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ProyectoFinal_VargasValeria.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddSedeColumnToCarrera : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Sede",
                table: "Carreras",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Sede",
                table: "Carreras");
        }
    }
}
