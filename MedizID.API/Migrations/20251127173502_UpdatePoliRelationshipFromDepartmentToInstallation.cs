using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MedizID.API.Migrations
{
    /// <inheritdoc />
    public partial class UpdatePoliRelationshipFromDepartmentToInstallation : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Polis_Departments_DepartmentId",
                table: "Polis");

            migrationBuilder.RenameColumn(
                name: "DepartmentId",
                table: "Polis",
                newName: "InstallationId");

            migrationBuilder.RenameIndex(
                name: "IX_Polis_DepartmentId",
                table: "Polis",
                newName: "IX_Polis_InstallationId");

            migrationBuilder.AddForeignKey(
                name: "FK_Polis_Installations_InstallationId",
                table: "Polis",
                column: "InstallationId",
                principalTable: "Installations",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Polis_Installations_InstallationId",
                table: "Polis");

            migrationBuilder.RenameColumn(
                name: "InstallationId",
                table: "Polis",
                newName: "DepartmentId");

            migrationBuilder.RenameIndex(
                name: "IX_Polis_InstallationId",
                table: "Polis",
                newName: "IX_Polis_DepartmentId");

            migrationBuilder.AddForeignKey(
                name: "FK_Polis_Departments_DepartmentId",
                table: "Polis",
                column: "DepartmentId",
                principalTable: "Departments",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
