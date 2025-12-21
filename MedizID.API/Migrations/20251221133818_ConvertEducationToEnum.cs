using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MedizID.API.Migrations
{
    /// <inheritdoc />
    public partial class ConvertEducationToEnum : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // First, set any NULL values or problematic values
            migrationBuilder.Sql(
                @"UPDATE ""FamilyPlannings"" 
                  SET ""WifeEducation"" = NULL 
                  WHERE ""WifeEducation"" IS NULL OR ""WifeEducation"" = ''");
            
            migrationBuilder.Sql(
                @"UPDATE ""FamilyPlannings"" 
                  SET ""HusbandEducation"" = NULL 
                  WHERE ""HusbandEducation"" IS NULL OR ""HusbandEducation"" = ''");
            
            // Convert columns to integer using CAST
            migrationBuilder.AlterColumn<int>(
                name: "WifeEducation",
                table: "FamilyPlannings",
                type: "integer",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "HusbandEducation",
                table: "FamilyPlannings",
                type: "integer",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "WifeEducation",
                table: "FamilyPlannings",
                type: "text",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "integer",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "HusbandEducation",
                table: "FamilyPlannings",
                type: "text",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "integer",
                oldNullable: true);
        }
    }
}
