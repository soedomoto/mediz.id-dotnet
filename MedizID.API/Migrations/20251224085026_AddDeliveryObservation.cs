using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MedizID.API.Migrations
{
    /// <inheritdoc />
    public partial class AddDeliveryObservation : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "DeliveryObservations",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    AppointmentId = table.Column<Guid>(type: "uuid", nullable: false),
                    GestationalAge = table.Column<int>(type: "integer", nullable: true),
                    GestationalAgeFromLmp = table.Column<int>(type: "integer", nullable: true),
                    MaternalCondition = table.Column<string>(type: "text", nullable: true),
                    MaternalDischargingCondition = table.Column<string>(type: "text", nullable: true),
                    NeonatalCondition = table.Column<string>(type: "text", nullable: true),
                    NeonatalWeight = table.Column<double>(type: "double precision", nullable: true),
                    Presentation = table.Column<string>(type: "text", nullable: true),
                    DeliveryLocation = table.Column<string>(type: "text", nullable: true),
                    BirthAttendant = table.Column<string>(type: "text", nullable: true),
                    DeliveryMode = table.Column<string>(type: "text", nullable: true),
                    OxytocinAdministered = table.Column<bool>(type: "boolean", nullable: true),
                    ControlledCordTraction = table.Column<bool>(type: "boolean", nullable: true),
                    UterineMassage = table.Column<bool>(type: "boolean", nullable: true),
                    BloodTransfusion = table.Column<bool>(type: "boolean", nullable: true),
                    AntibioticTherapy = table.Column<bool>(type: "boolean", nullable: true),
                    NeonatalResuscitation = table.Column<bool>(type: "boolean", nullable: true),
                    ProgramIntegration = table.Column<string>(type: "text", nullable: true),
                    AntiretroviralProphylaxis = table.Column<string>(type: "text", nullable: true),
                    HasComplications = table.Column<bool>(type: "boolean", nullable: true),
                    ComplicationsDescription = table.Column<string>(type: "text", nullable: true),
                    WasReferred = table.Column<bool>(type: "boolean", nullable: true),
                    ReferralDestination = table.Column<string>(type: "text", nullable: true),
                    MaternalConditionAtReferral = table.Column<string>(type: "text", nullable: true),
                    DeliveryAddress = table.Column<string>(type: "text", nullable: true),
                    CreatedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DeliveryObservations", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DeliveryObservations_Appointments_AppointmentId",
                        column: x => x.AppointmentId,
                        principalTable: "Appointments",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_DeliveryObservations_AppointmentId",
                table: "DeliveryObservations",
                column: "AppointmentId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DeliveryObservations");
        }
    }
}
