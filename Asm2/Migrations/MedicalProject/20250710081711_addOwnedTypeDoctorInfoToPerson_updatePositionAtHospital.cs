using Microsoft.EntityFrameworkCore.Migrations;

namespace Asm2.Migrations.MedicalProject
{
    public partial class addOwnedTypeDoctorInfoToPerson_updatePositionAtHospital : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "DoctorInfo_PositionAtHospital",
                table: "Persons",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DoctorInfo_PositionAtHospital",
                table: "Persons");
        }
    }
}
