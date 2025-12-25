using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MedizID.API.Migrations
{
    /// <inheritdoc />
    public partial class AddPostpartumObservation : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "postpartum_observations",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    AppointmentId = table.Column<Guid>(type: "uuid", nullable: false),
                    PncDate = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    BloodPressure = table.Column<string>(type: "text", nullable: true),
                    Temperature = table.Column<double>(type: "double precision", nullable: true),
                    Complications = table.Column<string>(type: "text", nullable: true),
                    RespiratoryRate = table.Column<string>(type: "text", nullable: true),
                    PulseRate = table.Column<string>(type: "text", nullable: true),
                    VaginalBleeding = table.Column<string>(type: "text", nullable: true),
                    PerinealCondition = table.Column<string>(type: "text", nullable: true),
                    InfectionSigns = table.Column<string>(type: "text", nullable: true),
                    UterineContraction = table.Column<string>(type: "text", nullable: true),
                    BirthCanalExamination = table.Column<string>(type: "text", nullable: true),
                    BreastExamination = table.Column<string>(type: "text", nullable: true),
                    MilkProduction = table.Column<string>(type: "text", nullable: true),
                    HighRiskComplicationManagement = table.Column<string>(type: "text", nullable: true),
                    BowelMovements = table.Column<string>(type: "text", nullable: true),
                    Urination = table.Column<string>(type: "text", nullable: true),
                    PostpartumDay = table.Column<int>(type: "integer", nullable: true),
                    RecordedInKiaBook = table.Column<string>(type: "text", nullable: true),
                    IronSupplementation = table.Column<string>(type: "text", nullable: true),
                    VitaminA = table.Column<string>(type: "text", nullable: true),
                    ReferralDestination = table.Column<string>(type: "text", nullable: true),
                    ArtStatus = table.Column<string>(type: "text", nullable: true),
                    AntiMalariaInfo = table.Column<string>(type: "text", nullable: true),
                    AntiTbcInfo = table.Column<string>(type: "text", nullable: true),
                    ThoraxPhotoStatus = table.Column<string>(type: "text", nullable: true),
                    Cd4IfComplications = table.Column<string>(type: "text", nullable: true),
                    ConditionAtArrival = table.Column<string>(type: "text", nullable: true),
                    ConditionAtDischarge = table.Column<string>(type: "text", nullable: true),
                    ContraceptionMethod = table.Column<string>(type: "text", nullable: true),
                    ContraceptionPlannedDate = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    ContraceptionImplementationDate = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    CreatedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_postpartum_observations", x => x.Id);
                    table.ForeignKey(
                        name: "FK_postpartum_observations_Appointments_AppointmentId",
                        column: x => x.AppointmentId,
                        principalTable: "Appointments",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_postpartum_observations_AppointmentId",
                table: "postpartum_observations",
                column: "AppointmentId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "postpartum_observations");
        }
    }
}
