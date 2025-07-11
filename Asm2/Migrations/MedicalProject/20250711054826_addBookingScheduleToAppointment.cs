using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Asm2.Migrations.MedicalProject
{
    public partial class addBookingScheduleToAppointment : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "BookingSchedule_End",
                table: "Appointments",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "BookingSchedule_ExpireAt",
                table: "Appointments",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<int>(
                name: "BookingSchedule_Priority",
                table: "Appointments",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<DateTime>(
                name: "BookingSchedule_Start",
                table: "Appointments",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "BookingSchedule_End",
                table: "Appointments");

            migrationBuilder.DropColumn(
                name: "BookingSchedule_ExpireAt",
                table: "Appointments");

            migrationBuilder.DropColumn(
                name: "BookingSchedule_Priority",
                table: "Appointments");

            migrationBuilder.DropColumn(
                name: "BookingSchedule_Start",
                table: "Appointments");
        }
    }
}
