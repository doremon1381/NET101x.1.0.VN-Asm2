using Microsoft.EntityFrameworkCore.Migrations;

namespace Asm2.Migrations.MedicalProject
{
    public partial class addDiagnosisToAppointment : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Diagnoses_AppointmentId",
                table: "Diagnoses");

            migrationBuilder.CreateIndex(
                name: "IX_Diagnoses_AppointmentId",
                table: "Diagnoses",
                column: "AppointmentId",
                unique: true,
                filter: "[AppointmentId] IS NOT NULL");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Diagnoses_AppointmentId",
                table: "Diagnoses");

            migrationBuilder.CreateIndex(
                name: "IX_Diagnoses_AppointmentId",
                table: "Diagnoses",
                column: "AppointmentId");
        }
    }
}
