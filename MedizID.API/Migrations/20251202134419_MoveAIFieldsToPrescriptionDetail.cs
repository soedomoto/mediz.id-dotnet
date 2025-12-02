using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MedizID.API.Migrations
{
    /// <inheritdoc />
    public partial class MoveAIFieldsToPrescriptionDetail : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AIRecommendationConfidence",
                table: "Prescriptions");

            migrationBuilder.DropColumn(
                name: "IsRecommendedByAI",
                table: "Prescriptions");

            migrationBuilder.AddColumn<int>(
                name: "AIRecommendationConfidence",
                table: "PrescriptionDetails",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsRecommendedByAI",
                table: "PrescriptionDetails",
                type: "boolean",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AIRecommendationConfidence",
                table: "PrescriptionDetails");

            migrationBuilder.DropColumn(
                name: "IsRecommendedByAI",
                table: "PrescriptionDetails");

            migrationBuilder.AddColumn<int>(
                name: "AIRecommendationConfidence",
                table: "Prescriptions",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsRecommendedByAI",
                table: "Prescriptions",
                type: "boolean",
                nullable: true);
        }
    }
}
