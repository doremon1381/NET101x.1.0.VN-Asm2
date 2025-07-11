using Microsoft.EntityFrameworkCore.Migrations;

namespace Asm2.Migrations.MedicalProject
{
    public partial class updateReferenceOfAppointment1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Appointments_Persons_PersonId",
                table: "Appointments");

            migrationBuilder.DropIndex(
                name: "IX_Appointments_PersonId",
                table: "Appointments");

            migrationBuilder.DropColumn(
                name: "PersonId",
                table: "Appointments");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "PersonId",
                table: "Appointments",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Appointments_PersonId",
                table: "Appointments",
                column: "PersonId");

            migrationBuilder.AddForeignKey(
                name: "FK_Appointments_Persons_PersonId",
                table: "Appointments",
                column: "PersonId",
                principalTable: "Persons",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
