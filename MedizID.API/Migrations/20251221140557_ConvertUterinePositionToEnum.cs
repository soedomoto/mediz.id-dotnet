using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MedizID.API.Migrations
{
    /// <inheritdoc />
    public partial class ConvertUterinePositionToEnum : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
                ALTER TABLE ""FamilyPlannings""
                ALTER COLUMN ""UterinePosition"" TYPE integer
                USING CASE 
                    WHEN ""UterinePosition"" ~ '^[0-9]+$' THEN CAST(""UterinePosition"" AS integer)
                    ELSE NULL
                END;");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
                ALTER TABLE ""FamilyPlannings""
                ALTER COLUMN ""UterinePosition"" TYPE text;");
        }
    }
}
