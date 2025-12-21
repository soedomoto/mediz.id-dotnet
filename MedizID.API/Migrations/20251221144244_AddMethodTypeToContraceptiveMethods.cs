using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MedizID.API.Migrations
{
    /// <inheritdoc />
    public partial class AddMethodTypeToContraceptiveMethods : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "MethodName",
                table: "FamilyPlanningContraceptiveMethods");

            migrationBuilder.AddColumn<int>(
                name: "MethodType",
                table: "FamilyPlanningContraceptiveMethods",
                type: "integer",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "MethodType",
                table: "FamilyPlanningContraceptiveMethods");

            migrationBuilder.AddColumn<string>(
                name: "MethodName",
                table: "FamilyPlanningContraceptiveMethods",
                type: "text",
                nullable: true);
        }
    }
}
