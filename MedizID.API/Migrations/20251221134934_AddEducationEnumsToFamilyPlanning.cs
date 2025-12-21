using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MedizID.API.Migrations
{
    /// <inheritdoc />
    public partial class AddEducationEnumsToFamilyPlanning : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // Clean up empty strings to NULL
            migrationBuilder.Sql(
                @"UPDATE ""FamilyPlannings"" 
                  SET ""FamilyPlanningStage"" = NULL 
                  WHERE ""FamilyPlanningStage"" = '' OR ""FamilyPlanningStage"" IS NULL;");
            
            migrationBuilder.Sql(
                @"UPDATE ""FamilyPlannings"" 
                  SET ""KBParticipantStatus"" = NULL 
                  WHERE ""KBParticipantStatus"" = '' OR ""KBParticipantStatus"" IS NULL;");
            
            migrationBuilder.Sql(
                @"UPDATE ""FamilyPlannings"" 
                  SET ""LastContraceptiveMethod"" = NULL 
                  WHERE ""LastContraceptiveMethod"" = '' OR ""LastContraceptiveMethod"" IS NULL;");
            
            // Change column types using USING clause for safe conversion
            migrationBuilder.Sql(
                @"ALTER TABLE ""FamilyPlannings"" 
                  ALTER COLUMN ""FamilyPlanningStage"" TYPE integer USING (""FamilyPlanningStage""::integer);");
            
            migrationBuilder.Sql(
                @"ALTER TABLE ""FamilyPlannings"" 
                  ALTER COLUMN ""KBParticipantStatus"" TYPE integer USING (""KBParticipantStatus""::integer);");
            
            migrationBuilder.Sql(
                @"ALTER TABLE ""FamilyPlannings"" 
                  ALTER COLUMN ""LastContraceptiveMethod"" TYPE integer USING (""LastContraceptiveMethod""::integer);");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            // Revert to text type
            migrationBuilder.Sql(
                @"ALTER TABLE ""FamilyPlannings"" 
                  ALTER COLUMN ""FamilyPlanningStage"" TYPE text USING (""FamilyPlanningStage""::text);");
            
            migrationBuilder.Sql(
                @"ALTER TABLE ""FamilyPlannings"" 
                  ALTER COLUMN ""KBParticipantStatus"" TYPE text USING (""KBParticipantStatus""::text);");
            
            migrationBuilder.Sql(
                @"ALTER TABLE ""FamilyPlannings"" 
                  ALTER COLUMN ""LastContraceptiveMethod"" TYPE text USING (""LastContraceptiveMethod""::text);");
        }
    }
}
