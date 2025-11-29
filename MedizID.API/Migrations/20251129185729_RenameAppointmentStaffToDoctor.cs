using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MedizID.API.Migrations
{
    /// <inheritdoc />
    public partial class RenameAppointmentStaffToDoctor : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Appointments_FacilityStaffs_FacilityStaffId",
                table: "Appointments");

            migrationBuilder.RenameColumn(
                name: "FacilityStaffId",
                table: "Appointments",
                newName: "FacilityDoctorId");

            migrationBuilder.RenameIndex(
                name: "IX_Appointments_FacilityStaffId",
                table: "Appointments",
                newName: "IX_Appointments_FacilityDoctorId");

            migrationBuilder.AddForeignKey(
                name: "FK_Appointments_FacilityStaffs_FacilityDoctorId",
                table: "Appointments",
                column: "FacilityDoctorId",
                principalTable: "FacilityStaffs",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Appointments_FacilityStaffs_FacilityDoctorId",
                table: "Appointments");

            migrationBuilder.RenameColumn(
                name: "FacilityDoctorId",
                table: "Appointments",
                newName: "FacilityStaffId");

            migrationBuilder.RenameIndex(
                name: "IX_Appointments_FacilityDoctorId",
                table: "Appointments",
                newName: "IX_Appointments_FacilityStaffId");

            migrationBuilder.AddForeignKey(
                name: "FK_Appointments_FacilityStaffs_FacilityStaffId",
                table: "Appointments",
                column: "FacilityStaffId",
                principalTable: "FacilityStaffs",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);
        }
    }
}
