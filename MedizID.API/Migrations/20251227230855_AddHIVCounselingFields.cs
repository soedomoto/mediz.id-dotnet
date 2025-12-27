using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MedizID.API.Migrations
{
    /// <inheritdoc />
    public partial class AddHIVCounselingFields : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "TestSource",
                table: "HIVCounselings",
                newName: "NumberOfChildren");

            migrationBuilder.RenameColumn(
                name: "TestResult",
                table: "HIVCounselings",
                newName: "TestReasons");

            migrationBuilder.RenameColumn(
                name: "TestReason",
                table: "HIVCounselings",
                newName: "LastChildAge");

            migrationBuilder.RenameColumn(
                name: "Notes",
                table: "HIVCounselings",
                newName: "TestKnowledgeSource");

            migrationBuilder.RenameColumn(
                name: "LastTestDate",
                table: "HIVCounselings",
                newName: "WindowPeriodRiskDate");

            migrationBuilder.AlterColumn<string>(
                name: "VisitStatus",
                table: "HIVCounselings",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AlterColumn<string>(
                name: "PreviousTestResult",
                table: "HIVCounselings",
                type: "text",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "integer",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "PregnancyStatus",
                table: "HIVCounselings",
                type: "text",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "integer",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "PartnerHIVStatus",
                table: "HIVCounselings",
                type: "text",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "integer",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "ClientStatus",
                table: "HIVCounselings",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AddColumn<bool>(
                name: "AnalSexRisk",
                table: "HIVCounselings",
                type: "boolean",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "AnalSexRiskDate",
                table: "HIVCounselings",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "BloodTransfusionRisk",
                table: "HIVCounselings",
                type: "boolean",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "BloodTransfusionRiskDate",
                table: "HIVCounselings",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "HasFemalePartner",
                table: "HIVCounselings",
                type: "boolean",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "HasRegularPartner",
                table: "HIVCounselings",
                type: "boolean",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsIncarcerated",
                table: "HIVCounselings",
                type: "boolean",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "MotherName",
                table: "HIVCounselings",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "MotherToChildRisk",
                table: "HIVCounselings",
                type: "boolean",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "MotherToChildRiskDate",
                table: "HIVCounselings",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ObservationNotes",
                table: "HIVCounselings",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "OtherRiskDate",
                table: "HIVCounselings",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "OtherRiskDescription",
                table: "HIVCounselings",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "PartnerDateOfBirth",
                table: "HIVCounselings",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "PartnerLastTestDate",
                table: "HIVCounselings",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PartnerName",
                table: "HIVCounselings",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "PartnerPregnant",
                table: "HIVCounselings",
                type: "boolean",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "PreTestCounselingDate",
                table: "HIVCounselings",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "PreviousTestDate",
                table: "HIVCounselings",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PreviousTestLocation",
                table: "HIVCounselings",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "PreviouslyTested",
                table: "HIVCounselings",
                type: "boolean",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ReferralStatus",
                table: "HIVCounselings",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "RiskGroupStartDate",
                table: "HIVCounselings",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "SharedNeedlesRisk",
                table: "HIVCounselings",
                type: "boolean",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "SharedNeedlesRiskDate",
                table: "HIVCounselings",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "VaginalSexRisk",
                table: "HIVCounselings",
                type: "boolean",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "VaginalSexRiskDate",
                table: "HIVCounselings",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "WillingToTest",
                table: "HIVCounselings",
                type: "boolean",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "WindowPeriodRisk",
                table: "HIVCounselings",
                type: "boolean",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AnalSexRisk",
                table: "HIVCounselings");

            migrationBuilder.DropColumn(
                name: "AnalSexRiskDate",
                table: "HIVCounselings");

            migrationBuilder.DropColumn(
                name: "BloodTransfusionRisk",
                table: "HIVCounselings");

            migrationBuilder.DropColumn(
                name: "BloodTransfusionRiskDate",
                table: "HIVCounselings");

            migrationBuilder.DropColumn(
                name: "HasFemalePartner",
                table: "HIVCounselings");

            migrationBuilder.DropColumn(
                name: "HasRegularPartner",
                table: "HIVCounselings");

            migrationBuilder.DropColumn(
                name: "IsIncarcerated",
                table: "HIVCounselings");

            migrationBuilder.DropColumn(
                name: "MotherName",
                table: "HIVCounselings");

            migrationBuilder.DropColumn(
                name: "MotherToChildRisk",
                table: "HIVCounselings");

            migrationBuilder.DropColumn(
                name: "MotherToChildRiskDate",
                table: "HIVCounselings");

            migrationBuilder.DropColumn(
                name: "ObservationNotes",
                table: "HIVCounselings");

            migrationBuilder.DropColumn(
                name: "OtherRiskDate",
                table: "HIVCounselings");

            migrationBuilder.DropColumn(
                name: "OtherRiskDescription",
                table: "HIVCounselings");

            migrationBuilder.DropColumn(
                name: "PartnerDateOfBirth",
                table: "HIVCounselings");

            migrationBuilder.DropColumn(
                name: "PartnerLastTestDate",
                table: "HIVCounselings");

            migrationBuilder.DropColumn(
                name: "PartnerName",
                table: "HIVCounselings");

            migrationBuilder.DropColumn(
                name: "PartnerPregnant",
                table: "HIVCounselings");

            migrationBuilder.DropColumn(
                name: "PreTestCounselingDate",
                table: "HIVCounselings");

            migrationBuilder.DropColumn(
                name: "PreviousTestDate",
                table: "HIVCounselings");

            migrationBuilder.DropColumn(
                name: "PreviousTestLocation",
                table: "HIVCounselings");

            migrationBuilder.DropColumn(
                name: "PreviouslyTested",
                table: "HIVCounselings");

            migrationBuilder.DropColumn(
                name: "ReferralStatus",
                table: "HIVCounselings");

            migrationBuilder.DropColumn(
                name: "RiskGroupStartDate",
                table: "HIVCounselings");

            migrationBuilder.DropColumn(
                name: "SharedNeedlesRisk",
                table: "HIVCounselings");

            migrationBuilder.DropColumn(
                name: "SharedNeedlesRiskDate",
                table: "HIVCounselings");

            migrationBuilder.DropColumn(
                name: "VaginalSexRisk",
                table: "HIVCounselings");

            migrationBuilder.DropColumn(
                name: "VaginalSexRiskDate",
                table: "HIVCounselings");

            migrationBuilder.DropColumn(
                name: "WillingToTest",
                table: "HIVCounselings");

            migrationBuilder.DropColumn(
                name: "WindowPeriodRisk",
                table: "HIVCounselings");

            migrationBuilder.RenameColumn(
                name: "WindowPeriodRiskDate",
                table: "HIVCounselings",
                newName: "LastTestDate");

            migrationBuilder.RenameColumn(
                name: "TestReasons",
                table: "HIVCounselings",
                newName: "TestResult");

            migrationBuilder.RenameColumn(
                name: "TestKnowledgeSource",
                table: "HIVCounselings",
                newName: "Notes");

            migrationBuilder.RenameColumn(
                name: "NumberOfChildren",
                table: "HIVCounselings",
                newName: "TestSource");

            migrationBuilder.RenameColumn(
                name: "LastChildAge",
                table: "HIVCounselings",
                newName: "TestReason");

            migrationBuilder.AlterColumn<string>(
                name: "VisitStatus",
                table: "HIVCounselings",
                type: "text",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "PreviousTestResult",
                table: "HIVCounselings",
                type: "integer",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "PregnancyStatus",
                table: "HIVCounselings",
                type: "integer",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "PartnerHIVStatus",
                table: "HIVCounselings",
                type: "integer",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "ClientStatus",
                table: "HIVCounselings",
                type: "text",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);
        }
    }
}
