using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MedizID.API.Migrations
{
    /// <inheritdoc />
    public partial class AddAntenatalCareObservation : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AntenatalCareObservations",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    AppointmentId = table.Column<Guid>(type: "uuid", nullable: false),
                    MedicalPersonnelId = table.Column<Guid>(type: "uuid", nullable: true),
                    MedicalPersonnelName = table.Column<string>(type: "text", nullable: true),
                    NurseId = table.Column<Guid>(type: "uuid", nullable: true),
                    NurseName = table.Column<string>(type: "text", nullable: true),
                    MaternityHealthPostName = table.Column<string>(type: "text", nullable: true),
                    CadreName = table.Column<string>(type: "text", nullable: true),
                    TraditionalBirthAttendantName = table.Column<string>(type: "text", nullable: true),
                    ObstetricComplicationHistory = table.Column<string>(type: "text", nullable: true),
                    ChronicDiseaseAndAllergy = table.Column<string>(type: "text", nullable: true),
                    DiseaseHistory = table.Column<string>(type: "text", nullable: true),
                    Gravida = table.Column<int>(type: "integer", nullable: true),
                    Partus = table.Column<int>(type: "integer", nullable: true),
                    Abortus = table.Column<int>(type: "integer", nullable: true),
                    AliveChildren = table.Column<int>(type: "integer", nullable: true),
                    PlannedDeliveryDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    PlannedDeliveryAssistant = table.Column<int>(type: "integer", nullable: true),
                    PlannedDeliveryPlace = table.Column<int>(type: "integer", nullable: true),
                    PlannedCompanion = table.Column<int>(type: "integer", nullable: true),
                    PlannedTransportation = table.Column<int>(type: "integer", nullable: true),
                    BloodDonorStatus = table.Column<int>(type: "integer", nullable: true),
                    LastMenstrualPeriodDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    EstimatedDeliveryDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    PreviousDeliveryDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    KiaBookStatus = table.Column<int>(type: "integer", nullable: true),
                    PrePregnancyWeight = table.Column<decimal>(type: "numeric", nullable: true),
                    Height = table.Column<decimal>(type: "numeric", nullable: true),
                    MotherKsurScore = table.Column<int>(type: "integer", nullable: true),
                    PregnancyRiskCategory = table.Column<int>(type: "integer", nullable: true),
                    HighRiskDescription = table.Column<string>(type: "text", nullable: true),
                    CasuisticRiskType = table.Column<int>(type: "integer", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AntenatalCareObservations", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AntenatalCareObservations_Appointments_AppointmentId",
                        column: x => x.AppointmentId,
                        principalTable: "Appointments",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AntenatalCareObservations_AppointmentId",
                table: "AntenatalCareObservations",
                column: "AppointmentId",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AntenatalCareObservations");
        }
    }
}
