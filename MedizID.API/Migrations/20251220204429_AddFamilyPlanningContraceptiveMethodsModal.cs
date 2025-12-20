using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MedizID.API.Migrations
{
    /// <inheritdoc />
    public partial class AddFamilyPlanningContraceptiveMethodsModal : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "FamilyPlanningContraceptiveMethods",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    FamilyPlanningId = table.Column<Guid>(type: "uuid", nullable: false),
                    MethodName = table.Column<string>(type: "text", nullable: true),
                    ServiceDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    Quantity = table.Column<int>(type: "integer", nullable: true),
                    Notes = table.Column<string>(type: "text", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FamilyPlanningContraceptiveMethods", x => x.Id);
                    table.ForeignKey(
                        name: "FK_FamilyPlanningContraceptiveMethods_FamilyPlannings_FamilyPl~",
                        column: x => x.FamilyPlanningId,
                        principalTable: "FamilyPlannings",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "FamilyPlanningLaboratoryResults",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    FamilyPlanningId = table.Column<Guid>(type: "uuid", nullable: false),
                    TestName = table.Column<string>(type: "text", nullable: true),
                    Result = table.Column<string>(type: "text", nullable: true),
                    ReferenceValue = table.Column<string>(type: "text", nullable: true),
                    TestDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    Notes = table.Column<string>(type: "text", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FamilyPlanningLaboratoryResults", x => x.Id);
                    table.ForeignKey(
                        name: "FK_FamilyPlanningLaboratoryResults_FamilyPlannings_FamilyPlann~",
                        column: x => x.FamilyPlanningId,
                        principalTable: "FamilyPlannings",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "FamilyPlanningProcedures",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    FamilyPlanningId = table.Column<Guid>(type: "uuid", nullable: false),
                    ProcedureName = table.Column<string>(type: "text", nullable: true),
                    ProcedureDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    PerformedBy = table.Column<string>(type: "text", nullable: true),
                    Outcome = table.Column<string>(type: "text", nullable: true),
                    Complications = table.Column<string>(type: "text", nullable: true),
                    Notes = table.Column<string>(type: "text", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FamilyPlanningProcedures", x => x.Id);
                    table.ForeignKey(
                        name: "FK_FamilyPlanningProcedures_FamilyPlannings_FamilyPlanningId",
                        column: x => x.FamilyPlanningId,
                        principalTable: "FamilyPlannings",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_FamilyPlanningContraceptiveMethods_FamilyPlanningId",
                table: "FamilyPlanningContraceptiveMethods",
                column: "FamilyPlanningId");

            migrationBuilder.CreateIndex(
                name: "IX_FamilyPlanningLaboratoryResults_FamilyPlanningId",
                table: "FamilyPlanningLaboratoryResults",
                column: "FamilyPlanningId");

            migrationBuilder.CreateIndex(
                name: "IX_FamilyPlanningProcedures_FamilyPlanningId",
                table: "FamilyPlanningProcedures",
                column: "FamilyPlanningId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "FamilyPlanningContraceptiveMethods");

            migrationBuilder.DropTable(
                name: "FamilyPlanningLaboratoryResults");

            migrationBuilder.DropTable(
                name: "FamilyPlanningProcedures");
        }
    }
}
