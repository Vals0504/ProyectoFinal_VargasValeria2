using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ProyectoFinal_VargasValeria.Migrations
{
    /// <inheritdoc />
    public partial class RemoveCarreraIdFromEstudiantes : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Estudiantes_Carreras_CarreraId",
                table: "Estudiantes");

            migrationBuilder.AlterColumn<int>(
                name: "CarreraId",
                table: "Estudiantes",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddForeignKey(
                name: "FK_Estudiantes_Carreras_CarreraId",
                table: "Estudiantes",
                column: "CarreraId",
                principalTable: "Carreras",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Estudiantes_Carreras_CarreraId",
                table: "Estudiantes");

            migrationBuilder.AlterColumn<int>(
                name: "CarreraId",
                table: "Estudiantes",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Estudiantes_Carreras_CarreraId",
                table: "Estudiantes",
                column: "CarreraId",
                principalTable: "Carreras",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
