using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MedizID.API.Migrations
{
    /// <inheritdoc />
    public partial class UpdateAdolescentHealthFields : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "BMI",
                table: "AdolescentHealths");

            migrationBuilder.RenameColumn(
                name: "RiskyBehaviors",
                table: "AdolescentHealths",
                newName: "UnwantedPregnancyNotes");

            migrationBuilder.RenameColumn(
                name: "PubertanStage",
                table: "AdolescentHealths",
                newName: "TeenageDeliveryNotes");

            migrationBuilder.RenameColumn(
                name: "Notes",
                table: "AdolescentHealths",
                newName: "SuicideDepressionAssessment");

            migrationBuilder.RenameColumn(
                name: "MentalHealthStatus",
                table: "AdolescentHealths",
                newName: "SolutionAlternatives");

            migrationBuilder.AddColumn<bool>(
                name: "AIDS",
                table: "AdolescentHealths",
                type: "boolean",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "AIDSNotes",
                table: "AdolescentHealths",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "Abortion",
                table: "AdolescentHealths",
                type: "boolean",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "AbortionNotes",
                table: "AdolescentHealths",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ActivityAssessment",
                table: "AdolescentHealths",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "AlcoholUse",
                table: "AdolescentHealths",
                type: "boolean",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "AlcoholUseNotes",
                table: "AdolescentHealths",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "Anemia",
                table: "AdolescentHealths",
                type: "boolean",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "AnemiaNotes",
                table: "AdolescentHealths",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "ChildAbuse",
                table: "AdolescentHealths",
                type: "boolean",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ChildAbuseNotes",
                table: "AdolescentHealths",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ChildOrder",
                table: "AdolescentHealths",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "ChronicEnergyDeficiency",
                table: "AdolescentHealths",
                type: "boolean",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ChronicEnergyDeficiencyNotes",
                table: "AdolescentHealths",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Citizenship",
                table: "AdolescentHealths",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ClientDecision",
                table: "AdolescentHealths",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Counselor",
                table: "AdolescentHealths",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "DesiredPregnancy",
                table: "AdolescentHealths",
                type: "boolean",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DesiredPregnancyNotes",
                table: "AdolescentHealths",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Diagnosis",
                table: "AdolescentHealths",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "DrugAbuse",
                table: "AdolescentHealths",
                type: "boolean",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DrugAbuseNotes",
                table: "AdolescentHealths",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DrugsAssessment",
                table: "AdolescentHealths",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "EarlyMarriage",
                table: "AdolescentHealths",
                type: "boolean",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "EarlyMarriageNotes",
                table: "AdolescentHealths",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "EatingAssessment",
                table: "AdolescentHealths",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "EmploymentEducationAssessment",
                table: "AdolescentHealths",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "FatherEducation",
                table: "AdolescentHealths",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "FatherOccupation",
                table: "AdolescentHealths",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "GadgetAddiction",
                table: "AdolescentHealths",
                type: "boolean",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "GadgetAddictionNotes",
                table: "AdolescentHealths",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Grade",
                table: "AdolescentHealths",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "HIV",
                table: "AdolescentHealths",
                type: "boolean",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "HIVNotes",
                table: "AdolescentHealths",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "HomeAssessment",
                table: "AdolescentHealths",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "LearningDifficulty",
                table: "AdolescentHealths",
                type: "boolean",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "LearningDifficultyNotes",
                table: "AdolescentHealths",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "MainComplaint",
                table: "AdolescentHealths",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "MainProblem",
                table: "AdolescentHealths",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "MaritalStatus",
                table: "AdolescentHealths",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "MedicalHistory",
                table: "AdolescentHealths",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "MenstrualDisorder",
                table: "AdolescentHealths",
                type: "boolean",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "MenstrualDisorderNotes",
                table: "AdolescentHealths",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "MentalDisability",
                table: "AdolescentHealths",
                type: "boolean",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "MentalDisabilityNotes",
                table: "AdolescentHealths",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "MotherEducation",
                table: "AdolescentHealths",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "MotherOccupation",
                table: "AdolescentHealths",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "NutritionDisorder",
                table: "AdolescentHealths",
                type: "boolean",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "NutritionDisorderNotes",
                table: "AdolescentHealths",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "Obesity",
                table: "AdolescentHealths",
                type: "boolean",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ObesityNotes",
                table: "AdolescentHealths",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Observations",
                table: "AdolescentHealths",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Occupation",
                table: "AdolescentHealths",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "OtherProblems",
                table: "AdolescentHealths",
                type: "boolean",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "OtherProblemsNotes",
                table: "AdolescentHealths",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "OtherSubstanceUse",
                table: "AdolescentHealths",
                type: "boolean",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "OtherSubstanceUseNotes",
                table: "AdolescentHealths",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ParentStatus",
                table: "AdolescentHealths",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "PhysicalDisability",
                table: "AdolescentHealths",
                type: "boolean",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PhysicalDisabilityNotes",
                table: "AdolescentHealths",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "Pregnancy",
                table: "AdolescentHealths",
                type: "boolean",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PregnancyNotes",
                table: "AdolescentHealths",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "PremaritalSex",
                table: "AdolescentHealths",
                type: "boolean",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PremaritalSexNotes",
                table: "AdolescentHealths",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ProblemBackground",
                table: "AdolescentHealths",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "PsychologicalIssues",
                table: "AdolescentHealths",
                type: "boolean",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PsychologicalIssuesNotes",
                table: "AdolescentHealths",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "ReproductiveInfection",
                table: "AdolescentHealths",
                type: "boolean",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ReproductiveInfectionNotes",
                table: "AdolescentHealths",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Residence",
                table: "AdolescentHealths",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "SafetyAssessment",
                table: "AdolescentHealths",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "SchoolName",
                table: "AdolescentHealths",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "SexualOrientation",
                table: "AdolescentHealths",
                type: "boolean",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "SexualOrientationNotes",
                table: "AdolescentHealths",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "SexualityAssessment",
                table: "AdolescentHealths",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "SexuallyTransmittedInfection",
                table: "AdolescentHealths",
                type: "boolean",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "SexuallyTransmittedInfectionNotes",
                table: "AdolescentHealths",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "Smoking",
                table: "AdolescentHealths",
                type: "boolean",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "SmokingNotes",
                table: "AdolescentHealths",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "TeenageDelivery",
                table: "AdolescentHealths",
                type: "boolean",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "TotalSiblings",
                table: "AdolescentHealths",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "UnwantedPregnancy",
                table: "AdolescentHealths",
                type: "boolean",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdatedAt",
                table: "AdolescentHealths",
                type: "timestamp with time zone",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AIDS",
                table: "AdolescentHealths");

            migrationBuilder.DropColumn(
                name: "AIDSNotes",
                table: "AdolescentHealths");

            migrationBuilder.DropColumn(
                name: "Abortion",
                table: "AdolescentHealths");

            migrationBuilder.DropColumn(
                name: "AbortionNotes",
                table: "AdolescentHealths");

            migrationBuilder.DropColumn(
                name: "ActivityAssessment",
                table: "AdolescentHealths");

            migrationBuilder.DropColumn(
                name: "AlcoholUse",
                table: "AdolescentHealths");

            migrationBuilder.DropColumn(
                name: "AlcoholUseNotes",
                table: "AdolescentHealths");

            migrationBuilder.DropColumn(
                name: "Anemia",
                table: "AdolescentHealths");

            migrationBuilder.DropColumn(
                name: "AnemiaNotes",
                table: "AdolescentHealths");

            migrationBuilder.DropColumn(
                name: "ChildAbuse",
                table: "AdolescentHealths");

            migrationBuilder.DropColumn(
                name: "ChildAbuseNotes",
                table: "AdolescentHealths");

            migrationBuilder.DropColumn(
                name: "ChildOrder",
                table: "AdolescentHealths");

            migrationBuilder.DropColumn(
                name: "ChronicEnergyDeficiency",
                table: "AdolescentHealths");

            migrationBuilder.DropColumn(
                name: "ChronicEnergyDeficiencyNotes",
                table: "AdolescentHealths");

            migrationBuilder.DropColumn(
                name: "Citizenship",
                table: "AdolescentHealths");

            migrationBuilder.DropColumn(
                name: "ClientDecision",
                table: "AdolescentHealths");

            migrationBuilder.DropColumn(
                name: "Counselor",
                table: "AdolescentHealths");

            migrationBuilder.DropColumn(
                name: "DesiredPregnancy",
                table: "AdolescentHealths");

            migrationBuilder.DropColumn(
                name: "DesiredPregnancyNotes",
                table: "AdolescentHealths");

            migrationBuilder.DropColumn(
                name: "Diagnosis",
                table: "AdolescentHealths");

            migrationBuilder.DropColumn(
                name: "DrugAbuse",
                table: "AdolescentHealths");

            migrationBuilder.DropColumn(
                name: "DrugAbuseNotes",
                table: "AdolescentHealths");

            migrationBuilder.DropColumn(
                name: "DrugsAssessment",
                table: "AdolescentHealths");

            migrationBuilder.DropColumn(
                name: "EarlyMarriage",
                table: "AdolescentHealths");

            migrationBuilder.DropColumn(
                name: "EarlyMarriageNotes",
                table: "AdolescentHealths");

            migrationBuilder.DropColumn(
                name: "EatingAssessment",
                table: "AdolescentHealths");

            migrationBuilder.DropColumn(
                name: "EmploymentEducationAssessment",
                table: "AdolescentHealths");

            migrationBuilder.DropColumn(
                name: "FatherEducation",
                table: "AdolescentHealths");

            migrationBuilder.DropColumn(
                name: "FatherOccupation",
                table: "AdolescentHealths");

            migrationBuilder.DropColumn(
                name: "GadgetAddiction",
                table: "AdolescentHealths");

            migrationBuilder.DropColumn(
                name: "GadgetAddictionNotes",
                table: "AdolescentHealths");

            migrationBuilder.DropColumn(
                name: "Grade",
                table: "AdolescentHealths");

            migrationBuilder.DropColumn(
                name: "HIV",
                table: "AdolescentHealths");

            migrationBuilder.DropColumn(
                name: "HIVNotes",
                table: "AdolescentHealths");

            migrationBuilder.DropColumn(
                name: "HomeAssessment",
                table: "AdolescentHealths");

            migrationBuilder.DropColumn(
                name: "LearningDifficulty",
                table: "AdolescentHealths");

            migrationBuilder.DropColumn(
                name: "LearningDifficultyNotes",
                table: "AdolescentHealths");

            migrationBuilder.DropColumn(
                name: "MainComplaint",
                table: "AdolescentHealths");

            migrationBuilder.DropColumn(
                name: "MainProblem",
                table: "AdolescentHealths");

            migrationBuilder.DropColumn(
                name: "MaritalStatus",
                table: "AdolescentHealths");

            migrationBuilder.DropColumn(
                name: "MedicalHistory",
                table: "AdolescentHealths");

            migrationBuilder.DropColumn(
                name: "MenstrualDisorder",
                table: "AdolescentHealths");

            migrationBuilder.DropColumn(
                name: "MenstrualDisorderNotes",
                table: "AdolescentHealths");

            migrationBuilder.DropColumn(
                name: "MentalDisability",
                table: "AdolescentHealths");

            migrationBuilder.DropColumn(
                name: "MentalDisabilityNotes",
                table: "AdolescentHealths");

            migrationBuilder.DropColumn(
                name: "MotherEducation",
                table: "AdolescentHealths");

            migrationBuilder.DropColumn(
                name: "MotherOccupation",
                table: "AdolescentHealths");

            migrationBuilder.DropColumn(
                name: "NutritionDisorder",
                table: "AdolescentHealths");

            migrationBuilder.DropColumn(
                name: "NutritionDisorderNotes",
                table: "AdolescentHealths");

            migrationBuilder.DropColumn(
                name: "Obesity",
                table: "AdolescentHealths");

            migrationBuilder.DropColumn(
                name: "ObesityNotes",
                table: "AdolescentHealths");

            migrationBuilder.DropColumn(
                name: "Observations",
                table: "AdolescentHealths");

            migrationBuilder.DropColumn(
                name: "Occupation",
                table: "AdolescentHealths");

            migrationBuilder.DropColumn(
                name: "OtherProblems",
                table: "AdolescentHealths");

            migrationBuilder.DropColumn(
                name: "OtherProblemsNotes",
                table: "AdolescentHealths");

            migrationBuilder.DropColumn(
                name: "OtherSubstanceUse",
                table: "AdolescentHealths");

            migrationBuilder.DropColumn(
                name: "OtherSubstanceUseNotes",
                table: "AdolescentHealths");

            migrationBuilder.DropColumn(
                name: "ParentStatus",
                table: "AdolescentHealths");

            migrationBuilder.DropColumn(
                name: "PhysicalDisability",
                table: "AdolescentHealths");

            migrationBuilder.DropColumn(
                name: "PhysicalDisabilityNotes",
                table: "AdolescentHealths");

            migrationBuilder.DropColumn(
                name: "Pregnancy",
                table: "AdolescentHealths");

            migrationBuilder.DropColumn(
                name: "PregnancyNotes",
                table: "AdolescentHealths");

            migrationBuilder.DropColumn(
                name: "PremaritalSex",
                table: "AdolescentHealths");

            migrationBuilder.DropColumn(
                name: "PremaritalSexNotes",
                table: "AdolescentHealths");

            migrationBuilder.DropColumn(
                name: "ProblemBackground",
                table: "AdolescentHealths");

            migrationBuilder.DropColumn(
                name: "PsychologicalIssues",
                table: "AdolescentHealths");

            migrationBuilder.DropColumn(
                name: "PsychologicalIssuesNotes",
                table: "AdolescentHealths");

            migrationBuilder.DropColumn(
                name: "ReproductiveInfection",
                table: "AdolescentHealths");

            migrationBuilder.DropColumn(
                name: "ReproductiveInfectionNotes",
                table: "AdolescentHealths");

            migrationBuilder.DropColumn(
                name: "Residence",
                table: "AdolescentHealths");

            migrationBuilder.DropColumn(
                name: "SafetyAssessment",
                table: "AdolescentHealths");

            migrationBuilder.DropColumn(
                name: "SchoolName",
                table: "AdolescentHealths");

            migrationBuilder.DropColumn(
                name: "SexualOrientation",
                table: "AdolescentHealths");

            migrationBuilder.DropColumn(
                name: "SexualOrientationNotes",
                table: "AdolescentHealths");

            migrationBuilder.DropColumn(
                name: "SexualityAssessment",
                table: "AdolescentHealths");

            migrationBuilder.DropColumn(
                name: "SexuallyTransmittedInfection",
                table: "AdolescentHealths");

            migrationBuilder.DropColumn(
                name: "SexuallyTransmittedInfectionNotes",
                table: "AdolescentHealths");

            migrationBuilder.DropColumn(
                name: "Smoking",
                table: "AdolescentHealths");

            migrationBuilder.DropColumn(
                name: "SmokingNotes",
                table: "AdolescentHealths");

            migrationBuilder.DropColumn(
                name: "TeenageDelivery",
                table: "AdolescentHealths");

            migrationBuilder.DropColumn(
                name: "TotalSiblings",
                table: "AdolescentHealths");

            migrationBuilder.DropColumn(
                name: "UnwantedPregnancy",
                table: "AdolescentHealths");

            migrationBuilder.DropColumn(
                name: "UpdatedAt",
                table: "AdolescentHealths");

            migrationBuilder.RenameColumn(
                name: "UnwantedPregnancyNotes",
                table: "AdolescentHealths",
                newName: "RiskyBehaviors");

            migrationBuilder.RenameColumn(
                name: "TeenageDeliveryNotes",
                table: "AdolescentHealths",
                newName: "PubertanStage");

            migrationBuilder.RenameColumn(
                name: "SuicideDepressionAssessment",
                table: "AdolescentHealths",
                newName: "Notes");

            migrationBuilder.RenameColumn(
                name: "SolutionAlternatives",
                table: "AdolescentHealths",
                newName: "MentalHealthStatus");

            migrationBuilder.AddColumn<decimal>(
                name: "BMI",
                table: "AdolescentHealths",
                type: "numeric",
                nullable: true);
        }
    }
}
