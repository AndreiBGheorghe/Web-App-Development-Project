using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Proiect.Migrations
{
    /// <inheritdoc />
    public partial class commit : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IdStudent",
                table: "Note");

            migrationBuilder.AddColumn<int>(
                name: "StudentIdStudent",
                table: "Note",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Note_StudentIdStudent",
                table: "Note",
                column: "StudentIdStudent");

            migrationBuilder.AddForeignKey(
                name: "FK_Note_Studenti_StudentIdStudent",
                table: "Note",
                column: "StudentIdStudent",
                principalTable: "Studenti",
                principalColumn: "IdStudent");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Note_Studenti_StudentIdStudent",
                table: "Note");

            migrationBuilder.DropIndex(
                name: "IX_Note_StudentIdStudent",
                table: "Note");

            migrationBuilder.DropColumn(
                name: "StudentIdStudent",
                table: "Note");

            migrationBuilder.AddColumn<int>(
                name: "IdStudent",
                table: "Note",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
