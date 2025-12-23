using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MedizID.API.Migrations
{
    /// <inheritdoc />
    public partial class AddImmunizationFields : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateTime>(
                name: "VaccineDate",
                table: "Immunizations",
                type: "timestamp with time zone",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "timestamp with time zone");

            migrationBuilder.AddColumn<string>(
                name: "Address",
                table: "Immunizations",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DoctorName",
                table: "Immunizations",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Gender",
                table: "Immunizations",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "NurseName",
                table: "Immunizations",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "ServiceDate",
                table: "Immunizations",
                type: "timestamp with time zone",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Address",
                table: "Immunizations");

            migrationBuilder.DropColumn(
                name: "DoctorName",
                table: "Immunizations");

            migrationBuilder.DropColumn(
                name: "Gender",
                table: "Immunizations");

            migrationBuilder.DropColumn(
                name: "NurseName",
                table: "Immunizations");

            migrationBuilder.DropColumn(
                name: "ServiceDate",
                table: "Immunizations");

            migrationBuilder.AlterColumn<DateTime>(
                name: "VaccineDate",
                table: "Immunizations",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                oldClrType: typeof(DateTime),
                oldType: "timestamp with time zone",
                oldNullable: true);
        }
    }
}
