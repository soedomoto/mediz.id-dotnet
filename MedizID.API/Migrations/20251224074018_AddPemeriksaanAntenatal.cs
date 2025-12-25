using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MedizID.API.Migrations
{
    /// <inheritdoc />
    public partial class AddPemeriksaanAntenatal : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "PemeriksaanAntenatal",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    AppointmentId = table.Column<Guid>(type: "uuid", nullable: false),
                    PregnancyWeeks = table.Column<int>(type: "integer", nullable: true),
                    Trimester = table.Column<int>(type: "integer", nullable: true),
                    FetalHeartRate = table.Column<string>(type: "text", nullable: true),
                    HeadPosition = table.Column<string>(type: "text", nullable: true),
                    EstimatedFetalWeight = table.Column<decimal>(type: "numeric", nullable: true),
                    Presentation = table.Column<string>(type: "text", nullable: true),
                    FetalCount = table.Column<int>(type: "integer", nullable: true),
                    Anamnesis = table.Column<string>(type: "text", nullable: true),
                    Height = table.Column<decimal>(type: "numeric", nullable: true),
                    Weight = table.Column<decimal>(type: "numeric", nullable: true),
                    BloodPressure = table.Column<string>(type: "text", nullable: true),
                    LILA = table.Column<decimal>(type: "numeric", nullable: true),
                    NutritionStatus = table.Column<string>(type: "text", nullable: true),
                    FundusHeight = table.Column<decimal>(type: "numeric", nullable: true),
                    PatellaReflex = table.Column<string>(type: "text", nullable: true),
                    Hemoglobin = table.Column<decimal>(type: "numeric", nullable: true),
                    Anemia = table.Column<string>(type: "text", nullable: true),
                    Proteinuria = table.Column<int>(type: "integer", nullable: true),
                    Reduction = table.Column<int>(type: "integer", nullable: true),
                    BloodSugar = table.Column<int>(type: "integer", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
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

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PemeriksaanAntenatal");
        }
    }
}
