using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MedizID.API.Migrations
{
    /// <inheritdoc />
    public partial class STI_UpdateAllFields : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Symptoms",
                table: "STIs",
                newName: "ReferralStatus");

            migrationBuilder.RenameColumn(
                name: "DiagnosisSTI",
                table: "STIs",
                newName: "OtherAnamnesisNotes");

            migrationBuilder.AlterColumn<string>(
                name: "VisitStatus",
                table: "STIs",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AddColumn<string>(
                name: "ClinicalSigns",
                table: "STIs",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "CondomLastContact",
                table: "STIs",
                type: "boolean",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "CondomLastMonthContact",
                table: "STIs",
                type: "boolean",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "CondomWithPartner",
                table: "STIs",
                type: "boolean",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Diagnosis",
                table: "STIs",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "LaboratoryReferral",
                table: "STIs",
                type: "boolean",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "LaboratoryResults",
                table: "STIs",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "LaboratoryTests",
                table: "STIs",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "LastSexualContactDaysAgo",
                table: "STIs",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "MotherName",
                table: "STIs",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "PregnancyStatus",
                table: "STIs",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "SexPartnerCountLastMonth",
                table: "STIs",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdatedAt",
                table: "STIs",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "VaginalDouching",
                table: "STIs",
                type: "boolean",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "VisitNumber",
                table: "STIs",
                type: "integer",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ClinicalSigns",
                table: "STIs");

            migrationBuilder.DropColumn(
                name: "CondomLastContact",
                table: "STIs");

            migrationBuilder.DropColumn(
                name: "CondomLastMonthContact",
                table: "STIs");

            migrationBuilder.DropColumn(
                name: "CondomWithPartner",
                table: "STIs");

            migrationBuilder.DropColumn(
                name: "Diagnosis",
                table: "STIs");

            migrationBuilder.DropColumn(
                name: "LaboratoryReferral",
                table: "STIs");

            migrationBuilder.DropColumn(
                name: "LaboratoryResults",
                table: "STIs");

            migrationBuilder.DropColumn(
                name: "LaboratoryTests",
                table: "STIs");

            migrationBuilder.DropColumn(
                name: "LastSexualContactDaysAgo",
                table: "STIs");

            migrationBuilder.DropColumn(
                name: "MotherName",
                table: "STIs");

            migrationBuilder.DropColumn(
                name: "PregnancyStatus",
                table: "STIs");

            migrationBuilder.DropColumn(
                name: "SexPartnerCountLastMonth",
                table: "STIs");

            migrationBuilder.DropColumn(
                name: "UpdatedAt",
                table: "STIs");

            migrationBuilder.DropColumn(
                name: "VaginalDouching",
                table: "STIs");

            migrationBuilder.DropColumn(
                name: "VisitNumber",
                table: "STIs");

            migrationBuilder.RenameColumn(
                name: "ReferralStatus",
                table: "STIs",
                newName: "Symptoms");

            migrationBuilder.RenameColumn(
                name: "OtherAnamnesisNotes",
                table: "STIs",
                newName: "DiagnosisSTI");

            migrationBuilder.AlterColumn<string>(
                name: "VisitStatus",
                table: "STIs",
                type: "text",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);
        }
    }
}
