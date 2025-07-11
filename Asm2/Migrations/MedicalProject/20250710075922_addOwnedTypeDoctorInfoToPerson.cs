using Microsoft.EntityFrameworkCore.Migrations;

namespace Asm2.Migrations.MedicalProject
{
    public partial class addOwnedTypeDoctorInfoToPerson : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Role",
                table: "Persons",
                newName: "DoctorInfo_Role");

            migrationBuilder.RenameColumn(
                name: "ProfileImageUrl",
                table: "Persons",
                newName: "DoctorInfo_ProfileImageUrl");

            migrationBuilder.AlterColumn<int>(
                name: "DoctorInfo_Role",
                table: "Persons",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<string>(
                name: "DoctorInfo_HospitalId",
                table: "Persons",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Persons_DoctorInfo_HospitalId",
                table: "Persons",
                column: "DoctorInfo_HospitalId");

            migrationBuilder.AddForeignKey(
                name: "FK_Persons_Hospitals_DoctorInfo_HospitalId",
                table: "Persons",
                column: "DoctorInfo_HospitalId",
                principalTable: "Hospitals",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Persons_Hospitals_DoctorInfo_HospitalId",
                table: "Persons");

            migrationBuilder.DropIndex(
                name: "IX_Persons_DoctorInfo_HospitalId",
                table: "Persons");

            migrationBuilder.DropColumn(
                name: "DoctorInfo_HospitalId",
                table: "Persons");

            migrationBuilder.RenameColumn(
                name: "DoctorInfo_Role",
                table: "Persons",
                newName: "Role");

            migrationBuilder.RenameColumn(
                name: "DoctorInfo_ProfileImageUrl",
                table: "Persons",
                newName: "ProfileImageUrl");

            migrationBuilder.AlterColumn<int>(
                name: "Role",
                table: "Persons",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);
        }
    }
}
