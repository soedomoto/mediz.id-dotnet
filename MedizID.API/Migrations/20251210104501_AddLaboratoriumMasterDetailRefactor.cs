using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace MedizID.API.Migrations
{
    /// <inheritdoc />
    public partial class AddLaboratoriumMasterDetailRefactor : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // First, convert Status column data from text to integer using USING clause
            migrationBuilder.Sql(@"
                ALTER TABLE ""LaboratoriumTests"" 
                ALTER COLUMN ""Status"" TYPE integer USING (
                    CASE LOWER(""Status"")
                        WHEN 'normal' THEN 0
                        WHEN 'abnormal' THEN 1
                        WHEN 'critical' THEN 2
                        WHEN 'pending' THEN 3
                        WHEN 'inconclusive' THEN 4
                        WHEN 'notperformed' THEN 5
                        ELSE 3  -- Default to Pending
                    END
                );
            ");

            migrationBuilder.DropColumn(
                name: "TestName",
                table: "LaboratoriumTests");

            migrationBuilder.RenameColumn(
                name: "Unit",
                table: "LaboratoriumTests",
                newName: "TestNotes");

            migrationBuilder.RenameColumn(
                name: "TestCode",
                table: "LaboratoriumTests",
                newName: "SampleCollectionLocation");

            migrationBuilder.RenameColumn(
                name: "ReferenceRange",
                table: "LaboratoriumTests",
                newName: "LabTechnician");

            migrationBuilder.AddColumn<string>(
                name: "AIClinicalNotes",
                table: "LaboratoriumTests",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "AIRecommendationConfidence",
                table: "LaboratoriumTests",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsRecommendedByAI",
                table: "LaboratoriumTests",
                type: "boolean",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "LaboratoriumTestMasterId",
                table: "LaboratoriumTests",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<DateTime>(
                name: "SampleCollectionDate",
                table: "LaboratoriumTests",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdatedAt",
                table: "LaboratoriumTests",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "LaboratoriumTestMasters",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    TestName = table.Column<string>(type: "text", nullable: false),
                    TestCode = table.Column<string>(type: "text", nullable: true),
                    Category = table.Column<int>(type: "integer", nullable: false),
                    Unit = table.Column<string>(type: "text", nullable: true),
                    ReferenceRange = table.Column<string>(type: "text", nullable: true),
                    Description = table.Column<string>(type: "text", nullable: true),
                    SampleType = table.Column<int>(type: "integer", nullable: true),
                    SamplePreparation = table.Column<string>(type: "text", nullable: true),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false),
                    SortOrder = table.Column<int>(type: "integer", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LaboratoriumTestMasters", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_LaboratoriumTests_LaboratoriumTestMasterId",
                table: "LaboratoriumTests",
                column: "LaboratoriumTestMasterId");

            migrationBuilder.AddForeignKey(
                name: "FK_LaboratoriumTests_LaboratoriumTestMasters_LaboratoriumTestM~",
                table: "LaboratoriumTests",
                column: "LaboratoriumTestMasterId",
                principalTable: "LaboratoriumTestMasters",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_LaboratoriumTests_LaboratoriumTestMasters_LaboratoriumTestM~",
                table: "LaboratoriumTests");

            migrationBuilder.DropTable(
                name: "LaboratoriumTestMasters");

            migrationBuilder.DropIndex(
                name: "IX_LaboratoriumTests_LaboratoriumTestMasterId",
                table: "LaboratoriumTests");

            migrationBuilder.DropColumn(
                name: "AIClinicalNotes",
                table: "LaboratoriumTests");

            migrationBuilder.DropColumn(
                name: "AIRecommendationConfidence",
                table: "LaboratoriumTests");

            migrationBuilder.DropColumn(
                name: "IsRecommendedByAI",
                table: "LaboratoriumTests");

            migrationBuilder.DropColumn(
                name: "LaboratoriumTestMasterId",
                table: "LaboratoriumTests");

            migrationBuilder.DropColumn(
                name: "SampleCollectionDate",
                table: "LaboratoriumTests");

            migrationBuilder.DropColumn(
                name: "UpdatedAt",
                table: "LaboratoriumTests");

            migrationBuilder.RenameColumn(
                name: "TestNotes",
                table: "LaboratoriumTests",
                newName: "Unit");

            migrationBuilder.RenameColumn(
                name: "SampleCollectionLocation",
                table: "LaboratoriumTests",
                newName: "TestCode");

            migrationBuilder.RenameColumn(
                name: "LabTechnician",
                table: "LaboratoriumTests",
                newName: "ReferenceRange");

            migrationBuilder.AlterColumn<string>(
                name: "Status",
                table: "LaboratoriumTests",
                type: "text",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "integer",
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "TestName",
                table: "LaboratoriumTests",
                type: "text",
                nullable: false,
                defaultValue: "");
        }
    }
}
