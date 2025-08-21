using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Proiect.Migrations
{
    /// <inheritdoc />
    public partial class commit1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "CursIdCurs",
                table: "Note",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "IdStudent",
                table: "Note",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "Utilizatori",
                columns: table => new
                {
                    IdUtilizator = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    NumeUtilizator = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Parola = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Rol = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Utilizatori", x => x.IdUtilizator);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Note_CursIdCurs",
                table: "Note",
                column: "CursIdCurs");

            migrationBuilder.CreateIndex(
                name: "IX_Cursuri_IdProfesor",
                table: "Cursuri",
                column: "IdProfesor");

            migrationBuilder.AddForeignKey(
                name: "FK_Cursuri_Profesori_IdProfesor",
                table: "Cursuri",
                column: "IdProfesor",
                principalTable: "Profesori",
                principalColumn: "IdProfesor",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Note_Cursuri_CursIdCurs",
                table: "Note",
                column: "CursIdCurs",
                principalTable: "Cursuri",
                principalColumn: "IdCurs");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Cursuri_Profesori_IdProfesor",
                table: "Cursuri");

            migrationBuilder.DropForeignKey(
                name: "FK_Note_Cursuri_CursIdCurs",
                table: "Note");

            migrationBuilder.DropTable(
                name: "Utilizatori");

            migrationBuilder.DropIndex(
                name: "IX_Note_CursIdCurs",
                table: "Note");

            migrationBuilder.DropIndex(
                name: "IX_Cursuri_IdProfesor",
                table: "Cursuri");

            migrationBuilder.DropColumn(
                name: "CursIdCurs",
                table: "Note");

            migrationBuilder.DropColumn(
                name: "IdStudent",
                table: "Note");
        }
    }
}
