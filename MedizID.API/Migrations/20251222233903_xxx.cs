using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MedizID.API.Migrations
{
    /// <inheritdoc />
    public partial class xxx : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Immunizations_AppointmentId",
                table: "Immunizations");

            migrationBuilder.CreateIndex(
                name: "IX_Immunizations_AppointmentId",
                table: "Immunizations",
                column: "AppointmentId",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Immunizations_AppointmentId",
                table: "Immunizations");

            migrationBuilder.CreateIndex(
                name: "IX_Immunizations_AppointmentId",
                table: "Immunizations",
                column: "AppointmentId");
        }
    }
}
