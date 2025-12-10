using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MedizID.API.Migrations
{
    /// <inheritdoc />
    public partial class UpdateICD9ToICD10PCS : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "ICD9Description",
                table: "Procedures",
                newName: "ICD10PCSDescription");

            migrationBuilder.RenameColumn(
                name: "ICD9Code",
                table: "Procedures",
                newName: "ICD10PCSCode");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "ICD10PCSDescription",
                table: "Procedures",
                newName: "ICD9Description");

            migrationBuilder.RenameColumn(
                name: "ICD10PCSCode",
                table: "Procedures",
                newName: "ICD9Code");
        }
    }
}
