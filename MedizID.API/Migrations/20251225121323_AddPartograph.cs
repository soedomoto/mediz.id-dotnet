using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MedizID.API.Migrations
{
    /// <inheritdoc />
    public partial class AddPartograph : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Partographs",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    AppointmentId = table.Column<Guid>(type: "uuid", nullable: false),
                    AdmissionDate = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    AdmissionTime = table.Column<TimeSpan>(type: "interval", nullable: true),
                    OnsetOfLaborDate = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    OnsetOfLaborTime = table.Column<TimeSpan>(type: "interval", nullable: true),
                    RuptureOfMembranesDate = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    RuptureOfMembranesTime = table.Column<TimeSpan>(type: "interval", nullable: true),
                    CervicalDilation = table.Column<string>(type: "text", nullable: true),
                    CervicalEffacement = table.Column<string>(type: "text", nullable: true),
                    FetalDescent = table.Column<string>(type: "text", nullable: true),
                    Molding = table.Column<string>(type: "text", nullable: true),
                    FetalHeartRateReadings = table.Column<string>(type: "text", nullable: true),
                    AmnioticFluidStatus = table.Column<string>(type: "text", nullable: true),
                    MoldingMonitoring = table.Column<string>(type: "text", nullable: true),
                    PulseRateReadings = table.Column<string>(type: "text", nullable: true),
                    BloodPressureReadings = table.Column<string>(type: "text", nullable: true),
                    TemperatureReadings = table.Column<string>(type: "text", nullable: true),
                    UrineOutput = table.Column<string>(type: "text", nullable: true),
                    OxytocinAdministration = table.Column<string>(type: "text", nullable: true),
                    IVFluidAdministration = table.Column<string>(type: "text", nullable: true),
                    OtherMedications = table.Column<string>(type: "text", nullable: true),
                    LaborNotes = table.Column<string>(type: "text", nullable: true),
                    Complications = table.Column<string>(type: "text", nullable: true),
                    ComplicationActions = table.Column<string>(type: "text", nullable: true),
                    DeliveryDateTime = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    PostpartumMaternalCondition = table.Column<string>(type: "text", nullable: true),
                    PlacentaDelivery = table.Column<string>(type: "text", nullable: true),
                    ThirdStageDuration = table.Column<string>(type: "text", nullable: true),
                    MaternalHemorrhageEstimate = table.Column<int>(type: "integer", nullable: true),
                    PerinealCondition = table.Column<string>(type: "text", nullable: true),
                    BladderStatus = table.Column<string>(type: "text", nullable: true),
                    UterineContractionStatus = table.Column<string>(type: "text", nullable: true),
                    CreatedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Partographs", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Partographs_Appointments_AppointmentId",
                        column: x => x.AppointmentId,
                        principalTable: "Appointments",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Partographs_AppointmentId",
                table: "Partographs",
                column: "AppointmentId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Partographs");
        }
    }
}
