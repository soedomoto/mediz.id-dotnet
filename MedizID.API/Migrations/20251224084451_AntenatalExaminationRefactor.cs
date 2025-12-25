using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MedizID.API.Migrations
{
    /// <inheritdoc />
    public partial class AntenatalExaminationRefactor : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PemeriksaanAntenatal");

            migrationBuilder.CreateTable(
                name: "AntenatalExaminations",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    AppointmentId = table.Column<Guid>(type: "uuid", nullable: false),
                    PregnancyWeeks = table.Column<int>(type: "integer", nullable: true),
                    Trimester = table.Column<int>(type: "integer", nullable: true),
                    FetalHeartRate = table.Column<string>(type: "text", nullable: true),
                    HeadPosition = table.Column<string>(type: "text", nullable: true),
                    EstimatedFetalWeight = table.Column<double>(type: "double precision", nullable: true),
                    Presentation = table.Column<string>(type: "text", nullable: true),
                    FetalCount = table.Column<int>(type: "integer", nullable: true),
                    ClinicalHistory = table.Column<string>(type: "text", nullable: true),
                    Height = table.Column<double>(type: "double precision", nullable: true),
                    Weight = table.Column<double>(type: "double precision", nullable: true),
                    BloodPressure = table.Column<string>(type: "text", nullable: true),
                    UpperArmCircumference = table.Column<double>(type: "double precision", nullable: true),
                    NutritionStatus = table.Column<string>(type: "text", nullable: true),
                    FundusHeight = table.Column<double>(type: "double precision", nullable: true),
                    PatellaReflex = table.Column<string>(type: "text", nullable: true),
                    Hemoglobin = table.Column<double>(type: "double precision", nullable: true),
                    Anemia = table.Column<string>(type: "text", nullable: true),
                    ProteinUrine = table.Column<int>(type: "integer", nullable: true),
                    UrineReducingSubstances = table.Column<int>(type: "integer", nullable: true),
                    BloodGlucose = table.Column<int>(type: "integer", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AntenatalExaminations", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AntenatalExaminations_Appointments_AppointmentId",
                        column: x => x.AppointmentId,
                        principalTable: "Appointments",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AntenatalExaminations_AppointmentId",
                table: "AntenatalExaminations",
                column: "AppointmentId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AntenatalExaminations");

            migrationBuilder.CreateTable(
                name: "PemeriksaanAntenatal",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    AppointmentId = table.Column<Guid>(type: "uuid", nullable: false),
                    Anamnesis = table.Column<string>(type: "text", nullable: true),
                    Anemia = table.Column<string>(type: "text", nullable: true),
                    BloodPressure = table.Column<string>(type: "text", nullable: true),
                    BloodSugar = table.Column<int>(type: "integer", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    EstimatedFetalWeight = table.Column<decimal>(type: "numeric", nullable: true),
                    FetalCount = table.Column<int>(type: "integer", nullable: true),
                    FetalHeartRate = table.Column<string>(type: "text", nullable: true),
                    FundusHeight = table.Column<decimal>(type: "numeric", nullable: true),
                    HeadPosition = table.Column<string>(type: "text", nullable: true),
                    Height = table.Column<decimal>(type: "numeric", nullable: true),
                    Hemoglobin = table.Column<decimal>(type: "numeric", nullable: true),
                    LILA = table.Column<decimal>(type: "numeric", nullable: true),
                    NutritionStatus = table.Column<string>(type: "text", nullable: true),
                    PatellaReflex = table.Column<string>(type: "text", nullable: true),
                    PregnancyWeeks = table.Column<int>(type: "integer", nullable: true),
                    Presentation = table.Column<string>(type: "text", nullable: true),
                    Proteinuria = table.Column<int>(type: "integer", nullable: true),
                    Reduction = table.Column<int>(type: "integer", nullable: true),
                    Trimester = table.Column<int>(type: "integer", nullable: true),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    Weight = table.Column<decimal>(type: "numeric", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PemeriksaanAntenatal", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PemeriksaanAntenatal_Appointments_AppointmentId",
                        column: x => x.AppointmentId,
                        principalTable: "Appointments",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_PemeriksaanAntenatal_AppointmentId",
                table: "PemeriksaanAntenatal",
                column: "AppointmentId");
        }
    }
}
