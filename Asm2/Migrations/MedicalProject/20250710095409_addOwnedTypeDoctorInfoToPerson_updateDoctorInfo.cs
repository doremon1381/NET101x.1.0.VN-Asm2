using Microsoft.EntityFrameworkCore.Migrations;

namespace Asm2.Migrations.MedicalProject
{
    public partial class addOwnedTypeDoctorInfoToPerson_updateDoctorInfo : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Address",
                table: "Persons",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<double>(
                name: "DoctorInfo_ExperienceInYrs",
                table: "Persons",
                type: "float",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DoctorInfo_SubSpecialties",
                table: "Persons",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Address",
                table: "Persons");

            migrationBuilder.DropColumn(
                name: "DoctorInfo_ExperienceInYrs",
                table: "Persons");

            migrationBuilder.DropColumn(
                name: "DoctorInfo_SubSpecialties",
                table: "Persons");
        }
    }
}
