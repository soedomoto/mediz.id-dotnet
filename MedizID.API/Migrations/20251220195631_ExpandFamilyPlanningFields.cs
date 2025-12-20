using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MedizID.API.Migrations
{
    /// <inheritdoc />
    public partial class ExpandFamilyPlanningFields : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Status",
                table: "FamilyPlannings",
                newName: "WifeOccupation");

            migrationBuilder.RenameColumn(
                name: "StartDate",
                table: "FamilyPlannings",
                newName: "UpdatedAt");

            migrationBuilder.RenameColumn(
                name: "Notes",
                table: "FamilyPlannings",
                newName: "WifeEducation");

            migrationBuilder.RenameColumn(
                name: "CurrentMethod",
                table: "FamilyPlannings",
                newName: "UterinePosition");

            migrationBuilder.AddColumn<bool>(
                name: "AbdominalPain",
                table: "FamilyPlannings",
                type: "boolean",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "AbnormalUterinebleeding",
                table: "FamilyPlannings",
                type: "boolean",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "AbnormalVaginalDischarge",
                table: "FamilyPlannings",
                type: "boolean",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "AllowedContraceptiveMethods",
                table: "FamilyPlannings",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "BloodClottingDisorder",
                table: "FamilyPlannings",
                type: "boolean",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "DiabetesSigns",
                table: "FamilyPlannings",
                type: "boolean",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "Dysmenorrhea",
                table: "FamilyPlannings",
                type: "boolean",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "EctopicPregnancyHistory",
                table: "FamilyPlannings",
                type: "boolean",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "FamilyPlanningStage",
                table: "FamilyPlannings",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "FollowUpDate",
                table: "FamilyPlannings",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "HusbandEducation",
                table: "FamilyPlannings",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "HusbandOccupation",
                table: "FamilyPlannings",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IUDStillInPlace",
                table: "FamilyPlannings",
                type: "boolean",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "InflammationSigns",
                table: "FamilyPlannings",
                type: "boolean",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "KBParticipantStatus",
                table: "FamilyPlannings",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "LastContraceptiveMethod",
                table: "FamilyPlannings",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "NumberOfLivingChildren",
                table: "FamilyPlannings",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ObservationNotes",
                table: "FamilyPlannings",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "OrchitisEpididymitis",
                table: "FamilyPlannings",
                type: "boolean",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "PelvicPain",
                table: "FamilyPlannings",
                type: "boolean",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "PregnancySigns",
                table: "FamilyPlannings",
                type: "boolean",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "RemovalDate",
                table: "FamilyPlannings",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "SelectedContraceptiveMethod",
                table: "FamilyPlannings",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "ServiceDate",
                table: "FamilyPlannings",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "SpouseName",
                table: "FamilyPlannings",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "TumorOrMalignancy",
                table: "FamilyPlannings",
                type: "boolean",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "TumorOrMalignancyMOP",
                table: "FamilyPlannings",
                type: "boolean",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "YoungestChildMonths",
                table: "FamilyPlannings",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "YoungestChildYears",
                table: "FamilyPlannings",
                type: "integer",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AbdominalPain",
                table: "FamilyPlannings");

            migrationBuilder.DropColumn(
                name: "AbnormalUterinebleeding",
                table: "FamilyPlannings");

            migrationBuilder.DropColumn(
                name: "AbnormalVaginalDischarge",
                table: "FamilyPlannings");

            migrationBuilder.DropColumn(
                name: "AllowedContraceptiveMethods",
                table: "FamilyPlannings");

            migrationBuilder.DropColumn(
                name: "BloodClottingDisorder",
                table: "FamilyPlannings");

            migrationBuilder.DropColumn(
                name: "DiabetesSigns",
                table: "FamilyPlannings");

            migrationBuilder.DropColumn(
                name: "Dysmenorrhea",
                table: "FamilyPlannings");

            migrationBuilder.DropColumn(
                name: "EctopicPregnancyHistory",
                table: "FamilyPlannings");

            migrationBuilder.DropColumn(
                name: "FamilyPlanningStage",
                table: "FamilyPlannings");

            migrationBuilder.DropColumn(
                name: "FollowUpDate",
                table: "FamilyPlannings");

            migrationBuilder.DropColumn(
                name: "HusbandEducation",
                table: "FamilyPlannings");

            migrationBuilder.DropColumn(
                name: "HusbandOccupation",
                table: "FamilyPlannings");

            migrationBuilder.DropColumn(
                name: "IUDStillInPlace",
                table: "FamilyPlannings");

            migrationBuilder.DropColumn(
                name: "InflammationSigns",
                table: "FamilyPlannings");

            migrationBuilder.DropColumn(
                name: "KBParticipantStatus",
                table: "FamilyPlannings");

            migrationBuilder.DropColumn(
                name: "LastContraceptiveMethod",
                table: "FamilyPlannings");

            migrationBuilder.DropColumn(
                name: "NumberOfLivingChildren",
                table: "FamilyPlannings");

            migrationBuilder.DropColumn(
                name: "ObservationNotes",
                table: "FamilyPlannings");

            migrationBuilder.DropColumn(
                name: "OrchitisEpididymitis",
                table: "FamilyPlannings");

            migrationBuilder.DropColumn(
                name: "PelvicPain",
                table: "FamilyPlannings");

            migrationBuilder.DropColumn(
                name: "PregnancySigns",
                table: "FamilyPlannings");

            migrationBuilder.DropColumn(
                name: "RemovalDate",
                table: "FamilyPlannings");

            migrationBuilder.DropColumn(
                name: "SelectedContraceptiveMethod",
                table: "FamilyPlannings");

            migrationBuilder.DropColumn(
                name: "ServiceDate",
                table: "FamilyPlannings");

            migrationBuilder.DropColumn(
                name: "SpouseName",
                table: "FamilyPlannings");

            migrationBuilder.DropColumn(
                name: "TumorOrMalignancy",
                table: "FamilyPlannings");

            migrationBuilder.DropColumn(
                name: "TumorOrMalignancyMOP",
                table: "FamilyPlannings");

            migrationBuilder.DropColumn(
                name: "YoungestChildMonths",
                table: "FamilyPlannings");

            migrationBuilder.DropColumn(
                name: "YoungestChildYears",
                table: "FamilyPlannings");

            migrationBuilder.RenameColumn(
                name: "WifeOccupation",
                table: "FamilyPlannings",
                newName: "Status");

            migrationBuilder.RenameColumn(
                name: "WifeEducation",
                table: "FamilyPlannings",
                newName: "Notes");

            migrationBuilder.RenameColumn(
                name: "UterinePosition",
                table: "FamilyPlannings",
                newName: "CurrentMethod");

            migrationBuilder.RenameColumn(
                name: "UpdatedAt",
                table: "FamilyPlannings",
                newName: "StartDate");
        }
    }
}
