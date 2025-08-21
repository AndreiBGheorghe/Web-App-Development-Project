using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Proiect.Migrations
{
    /// <inheritdoc />
    public partial class commit3 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Note_Cursuri_CursIdCurs",
                table: "Note");

            migrationBuilder.DropForeignKey(
                name: "FK_Note_Studenti_StudentIdStudent",
                table: "Note");

            migrationBuilder.DropIndex(
                name: "IX_Note_CursIdCurs",
                table: "Note");

            migrationBuilder.DropIndex(
                name: "IX_Note_StudentIdStudent",
                table: "Note");

            migrationBuilder.DropColumn(
                name: "CursIdCurs",
                table: "Note");

            migrationBuilder.DropColumn(
                name: "StudentIdStudent",
                table: "Note");

            migrationBuilder.AlterColumn<int>(
                name: "Rol",
                table: "Utilizatori",
                type: "int",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.CreateTable(
                name: "Mesaje",
                columns: table => new
                {
                    IdMesaj = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IdProfesor = table.Column<int>(type: "int", nullable: false),
                    NumeSecretar = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Continut = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DataTrimiterii = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Mesaje", x => x.IdMesaj);
                    table.ForeignKey(
                        name: "FK_Mesaje_Profesori_IdProfesor",
                        column: x => x.IdProfesor,
                        principalTable: "Profesori",
                        principalColumn: "IdProfesor",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Note_IdCurs",
                table: "Note",
                column: "IdCurs");

            migrationBuilder.CreateIndex(
                name: "IX_Note_IdStudent",
                table: "Note",
                column: "IdStudent");

            migrationBuilder.CreateIndex(
                name: "IX_Mesaje_IdProfesor",
                table: "Mesaje",
                column: "IdProfesor");

            migrationBuilder.AddForeignKey(
                name: "FK_Note_Cursuri_IdCurs",
                table: "Note",
                column: "IdCurs",
                principalTable: "Cursuri",
                principalColumn: "IdCurs",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Note_Studenti_IdStudent",
                table: "Note",
                column: "IdStudent",
                principalTable: "Studenti",
                principalColumn: "IdStudent",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Note_Cursuri_IdCurs",
                table: "Note");

            migrationBuilder.DropForeignKey(
                name: "FK_Note_Studenti_IdStudent",
                table: "Note");

            migrationBuilder.DropTable(
                name: "Mesaje");

            migrationBuilder.DropIndex(
                name: "IX_Note_IdCurs",
                table: "Note");

            migrationBuilder.DropIndex(
                name: "IX_Note_IdStudent",
                table: "Note");

            migrationBuilder.AlterColumn<string>(
                name: "Rol",
                table: "Utilizatori",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<int>(
                name: "CursIdCurs",
                table: "Note",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "StudentIdStudent",
                table: "Note",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Note_CursIdCurs",
                table: "Note",
                column: "CursIdCurs");

            migrationBuilder.CreateIndex(
                name: "IX_Note_StudentIdStudent",
                table: "Note",
                column: "StudentIdStudent");

            migrationBuilder.AddForeignKey(
                name: "FK_Note_Cursuri_CursIdCurs",
                table: "Note",
                column: "CursIdCurs",
                principalTable: "Cursuri",
                principalColumn: "IdCurs");

            migrationBuilder.AddForeignKey(
                name: "FK_Note_Studenti_StudentIdStudent",
                table: "Note",
                column: "StudentIdStudent",
                principalTable: "Studenti",
                principalColumn: "IdStudent");
        }
    }
}
