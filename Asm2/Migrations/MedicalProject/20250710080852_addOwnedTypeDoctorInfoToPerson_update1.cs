using Microsoft.EntityFrameworkCore.Migrations;

namespace Asm2.Migrations.MedicalProject
{
    public partial class addOwnedTypeDoctorInfoToPerson_update1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "DoctorInfo_Role",
                table: "Persons",
                newName: "Role");

            migrationBuilder.AlterColumn<int>(
                name: "Role",
                table: "Persons",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DoctorInfo_Specialty",
                table: "Persons",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DoctorInfo_Specialty",
                table: "Persons");

            migrationBuilder.RenameColumn(
                name: "Role",
                table: "Persons",
                newName: "DoctorInfo_Role");

            migrationBuilder.AlterColumn<int>(
                name: "DoctorInfo_Role",
                table: "Persons",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");
        }
    }
}
