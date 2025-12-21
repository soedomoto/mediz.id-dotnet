using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MedizID.API.Migrations
{
    /// <inheritdoc />
    public partial class aaa : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "VaccineName",
                table: "Immunizations",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text");

            // Drop and recreate Site column with type conversion
            migrationBuilder.Sql("ALTER TABLE \"Immunizations\" DROP COLUMN \"Site\"");
            migrationBuilder.AddColumn<int>(
                name: "Site",
                table: "Immunizations",
                type: "integer",
                nullable: true);

            // Drop and recreate Route column with type conversion
            migrationBuilder.Sql("ALTER TABLE \"Immunizations\" DROP COLUMN \"Route\"");
            migrationBuilder.AddColumn<int>(
                name: "Route",
                table: "Immunizations",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "AgeCategory",
                table: "Immunizations",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "BirthLength",
                table: "Immunizations",
                type: "numeric",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "BirthPlace",
                table: "Immunizations",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "BirthWeight",
                table: "Immunizations",
                type: "numeric",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "BreastfeedingStatus",
                table: "Immunizations",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ChildNumber",
                table: "Immunizations",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "DateOfBirth",
                table: "Immunizations",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "DeliveryPersonnel",
                table: "Immunizations",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "DeliveryPlace",
                table: "Immunizations",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "DeliveryType",
                table: "Immunizations",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "FatherName",
                table: "Immunizations",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "FatherOccupation",
                table: "Immunizations",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "MotherName",
                table: "Immunizations",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "MotherOccupation",
                table: "Immunizations",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "NeonatalVisit1",
                table: "Immunizations",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "NeonatalVisit2",
                table: "Immunizations",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "NeonatalVisit3",
                table: "Immunizations",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "NeonatalVisit4",
                table: "Immunizations",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "NurseId",
                table: "Immunizations",
                type: "uuid",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "ProviderId",
                table: "Immunizations",
                type: "uuid",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ReactionSeverity",
                table: "Immunizations",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdatedAt",
                table: "Immunizations",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "VaccineType",
                table: "Immunizations",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "VitaminAStatusSixMonths",
                table: "Immunizations",
                type: "integer",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AgeCategory",
                table: "Immunizations");

            migrationBuilder.DropColumn(
                name: "BirthLength",
                table: "Immunizations");

            migrationBuilder.DropColumn(
                name: "BirthPlace",
                table: "Immunizations");

            migrationBuilder.DropColumn(
                name: "BirthWeight",
                table: "Immunizations");

            migrationBuilder.DropColumn(
                name: "BreastfeedingStatus",
                table: "Immunizations");

            migrationBuilder.DropColumn(
                name: "ChildNumber",
                table: "Immunizations");

            migrationBuilder.DropColumn(
                name: "DateOfBirth",
                table: "Immunizations");

            migrationBuilder.DropColumn(
                name: "DeliveryPersonnel",
                table: "Immunizations");

            migrationBuilder.DropColumn(
                name: "DeliveryPlace",
                table: "Immunizations");

            migrationBuilder.DropColumn(
                name: "DeliveryType",
                table: "Immunizations");

            migrationBuilder.DropColumn(
                name: "FatherName",
                table: "Immunizations");

            migrationBuilder.DropColumn(
                name: "FatherOccupation",
                table: "Immunizations");

            migrationBuilder.DropColumn(
                name: "MotherName",
                table: "Immunizations");

            migrationBuilder.DropColumn(
                name: "MotherOccupation",
                table: "Immunizations");

            migrationBuilder.DropColumn(
                name: "NeonatalVisit1",
                table: "Immunizations");

            migrationBuilder.DropColumn(
                name: "NeonatalVisit2",
                table: "Immunizations");

            migrationBuilder.DropColumn(
                name: "NeonatalVisit3",
                table: "Immunizations");

            migrationBuilder.DropColumn(
                name: "NeonatalVisit4",
                table: "Immunizations");

            migrationBuilder.DropColumn(
                name: "NurseId",
                table: "Immunizations");

            migrationBuilder.DropColumn(
                name: "ProviderId",
                table: "Immunizations");

            migrationBuilder.DropColumn(
                name: "ReactionSeverity",
                table: "Immunizations");

            migrationBuilder.DropColumn(
                name: "UpdatedAt",
                table: "Immunizations");

            migrationBuilder.DropColumn(
                name: "VaccineType",
                table: "Immunizations");

            migrationBuilder.DropColumn(
                name: "VitaminAStatusSixMonths",
                table: "Immunizations");

            migrationBuilder.AlterColumn<string>(
                name: "VaccineName",
                table: "Immunizations",
                type: "text",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Site",
                table: "Immunizations",
                type: "text",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "integer",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Route",
                table: "Immunizations",
                type: "text",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "integer",
                oldNullable: true);
        }
    }
}
