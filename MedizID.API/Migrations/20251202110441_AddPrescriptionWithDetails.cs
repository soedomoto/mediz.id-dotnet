using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MedizID.API.Migrations
{
    /// <inheritdoc />
    public partial class AddPrescriptionWithDetails : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Dosage",
                table: "Prescriptions");

            migrationBuilder.DropColumn(
                name: "Duration",
                table: "Prescriptions");

            migrationBuilder.DropColumn(
                name: "Frequency",
                table: "Prescriptions");

            migrationBuilder.DropColumn(
                name: "Instructions",
                table: "Prescriptions");

            migrationBuilder.DropColumn(
                name: "MedicationName",
                table: "Prescriptions");

            migrationBuilder.DropColumn(
                name: "Description",
                table: "Diagnoses");

            migrationBuilder.RenameColumn(
                name: "ExpiryDate",
                table: "Prescriptions",
                newName: "UpdatedAt");

            migrationBuilder.AddColumn<int>(
                name: "AIRecommendationConfidence",
                table: "Prescriptions",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsRecommendedByAI",
                table: "Prescriptions",
                type: "boolean",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "PrescriptionDetails",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    PrescriptionId = table.Column<Guid>(type: "uuid", nullable: false),
                    DrugId = table.Column<Guid>(type: "uuid", nullable: true),
                    MedicalEquipmentId = table.Column<Guid>(type: "uuid", nullable: true),
                    MedicationName = table.Column<string>(type: "text", nullable: true),
                    Dosage = table.Column<string>(type: "text", nullable: true),
                    Signa = table.Column<string>(type: "text", nullable: true),
                    Frequency = table.Column<string>(type: "text", nullable: true),
                    Quantity = table.Column<int>(type: "integer", nullable: true),
                    Instructions = table.Column<string>(type: "text", nullable: true),
                    Notes = table.Column<string>(type: "text", nullable: true),
                    Price = table.Column<decimal>(type: "numeric", nullable: true),
                    Packaging = table.Column<decimal>(type: "numeric", nullable: true),
                    RecipeType = table.Column<string>(type: "text", nullable: true),
                    RequestedQuantity = table.Column<int>(type: "integer", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PrescriptionDetails", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PrescriptionDetails_Drugs_DrugId",
                        column: x => x.DrugId,
                        principalTable: "Drugs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_PrescriptionDetails_MedicalEquipments_MedicalEquipmentId",
                        column: x => x.MedicalEquipmentId,
                        principalTable: "MedicalEquipments",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_PrescriptionDetails_Prescriptions_PrescriptionId",
                        column: x => x.PrescriptionId,
                        principalTable: "Prescriptions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_PrescriptionDetails_DrugId",
                table: "PrescriptionDetails",
                column: "DrugId");

            migrationBuilder.CreateIndex(
                name: "IX_PrescriptionDetails_MedicalEquipmentId",
                table: "PrescriptionDetails",
                column: "MedicalEquipmentId");

            migrationBuilder.CreateIndex(
                name: "IX_PrescriptionDetails_PrescriptionId",
                table: "PrescriptionDetails",
                column: "PrescriptionId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PrescriptionDetails");

            migrationBuilder.DropColumn(
                name: "AIRecommendationConfidence",
                table: "Prescriptions");

            migrationBuilder.DropColumn(
                name: "IsRecommendedByAI",
                table: "Prescriptions");

            migrationBuilder.RenameColumn(
                name: "UpdatedAt",
                table: "Prescriptions",
                newName: "ExpiryDate");

            migrationBuilder.AddColumn<string>(
                name: "Dosage",
                table: "Prescriptions",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "Duration",
                table: "Prescriptions",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "Frequency",
                table: "Prescriptions",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Instructions",
                table: "Prescriptions",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "MedicationName",
                table: "Prescriptions",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "Diagnoses",
                type: "text",
                nullable: true);
        }
    }
}
