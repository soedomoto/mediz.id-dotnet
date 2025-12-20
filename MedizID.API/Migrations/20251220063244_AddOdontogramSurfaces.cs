using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MedizID.API.Migrations
{
    /// <inheritdoc />
    public partial class AddOdontogramSurfaces : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Status",
                table: "Odontograms");

            migrationBuilder.DropColumn(
                name: "Surface",
                table: "Odontograms");

            migrationBuilder.DropColumn(
                name: "ToothNumber",
                table: "Odontograms");

            migrationBuilder.DropColumn(
                name: "Treatment",
                table: "Odontograms");

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdatedAt",
                table: "Odontograms",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "OdontogramSurfaces",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    OdontogramId = table.Column<Guid>(type: "uuid", nullable: false),
                    ToothNumber = table.Column<int>(type: "integer", nullable: false),
                    Surface = table.Column<string>(type: "text", nullable: false),
                    ConditionCode = table.Column<string>(type: "text", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OdontogramSurfaces", x => x.Id);
                    table.ForeignKey(
                        name: "FK_OdontogramSurfaces_Odontograms_OdontogramId",
                        column: x => x.OdontogramId,
                        principalTable: "Odontograms",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_OdontogramSurfaces_OdontogramId",
                table: "OdontogramSurfaces",
                column: "OdontogramId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "OdontogramSurfaces");

            migrationBuilder.DropColumn(
                name: "UpdatedAt",
                table: "Odontograms");

            migrationBuilder.AddColumn<string>(
                name: "Status",
                table: "Odontograms",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Surface",
                table: "Odontograms",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ToothNumber",
                table: "Odontograms",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Treatment",
                table: "Odontograms",
                type: "text",
                nullable: true);
        }
    }
}
