using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MedizID.API.Migrations
{
    /// <inheritdoc />
    public partial class ConvertEducationFieldsToInt : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // Clean up empty strings to NULL
            migrationBuilder.Sql(
                @"UPDATE ""FamilyPlannings"" 
                  SET ""WifeEducation"" = NULL 
                  WHERE ""WifeEducation"" = '' OR ""WifeEducation"" IS NULL;");
            
            migrationBuilder.Sql(
                @"UPDATE ""FamilyPlannings"" 
                  SET ""HusbandEducation"" = NULL 
                  WHERE ""HusbandEducation"" = '' OR ""HusbandEducation"" IS NULL;");
            
            // Change column type using USING clause for safe conversion
            migrationBuilder.Sql(
                @"ALTER TABLE ""FamilyPlannings"" 
                  ALTER COLUMN ""WifeEducation"" TYPE integer USING (""WifeEducation""::integer);");
            
            migrationBuilder.Sql(
                @"ALTER TABLE ""FamilyPlannings"" 
                  ALTER COLUMN ""HusbandEducation"" TYPE integer USING (""HusbandEducation""::integer);");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            // Revert to text type
            migrationBuilder.Sql(
                @"ALTER TABLE ""FamilyPlannings"" 
                  ALTER COLUMN ""WifeEducation"" TYPE text USING (""WifeEducation""::text);");
            
            migrationBuilder.Sql(
                @"ALTER TABLE ""FamilyPlannings"" 
                  ALTER COLUMN ""HusbandEducation"" TYPE text USING (""HusbandEducation""::text);");
        }
    }
}
