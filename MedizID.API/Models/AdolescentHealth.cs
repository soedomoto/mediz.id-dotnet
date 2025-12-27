namespace MedizID.API.Models;

public class AdolescentHealth
{
    public Guid Id { get; set; }
    public Guid AppointmentId { get; set; }
    
    // Demographic Information
    public string? Citizenship { get; set; }
    public string? Residence { get; set; }
    public string? SchoolName { get; set; }
    public string? Grade { get; set; }
    public int? ChildOrder { get; set; }
    public int? TotalSiblings { get; set; }
    public string? Occupation { get; set; }
    
    // Parent Information
    public string? FatherEducation { get; set; }
    public string? FatherOccupation { get; set; }
    public string? MotherEducation { get; set; }
    public string? MotherOccupation { get; set; }
    
    // Status Information
    public int? MaritalStatus { get; set; }
    public int? ParentStatus { get; set; }
    
    // Physical Measurements
    public decimal? Weight { get; set; }
    public decimal? Height { get; set; }
    
    // Medical Information
    public string? MainComplaint { get; set; }
    public string? MedicalHistory { get; set; }
    public string? Diagnosis { get; set; }
    
    // Health Conditions - Yes/No with Notes
    public bool? MenstrualDisorder { get; set; }
    public string? MenstrualDisorderNotes { get; set; }
    
    public bool? PremaritalSex { get; set; }
    public string? PremaritalSexNotes { get; set; }
    
    public bool? Pregnancy { get; set; }
    public string? PregnancyNotes { get; set; }
    
    public bool? DesiredPregnancy { get; set; }
    public string? DesiredPregnancyNotes { get; set; }
    
    public bool? UnwantedPregnancy { get; set; }
    public string? UnwantedPregnancyNotes { get; set; }
    
    public bool? TeenageDelivery { get; set; }
    public string? TeenageDeliveryNotes { get; set; }
    
    public bool? Abortion { get; set; }
    public string? AbortionNotes { get; set; }
    
    public bool? NutritionDisorder { get; set; }
    public string? NutritionDisorderNotes { get; set; }
    
    public bool? Anemia { get; set; }
    public string? AnemiaNotes { get; set; }
    
    public bool? ChronicEnergyDeficiency { get; set; }
    public string? ChronicEnergyDeficiencyNotes { get; set; }
    
    public bool? Obesity { get; set; }
    public string? ObesityNotes { get; set; }
    
    public bool? DrugAbuse { get; set; }
    public string? DrugAbuseNotes { get; set; }
    
    public bool? Smoking { get; set; }
    public string? SmokingNotes { get; set; }
    
    public bool? AlcoholUse { get; set; }
    public string? AlcoholUseNotes { get; set; }
    
    public bool? OtherSubstanceUse { get; set; }
    public string? OtherSubstanceUseNotes { get; set; }
    
    public bool? SexuallyTransmittedInfection { get; set; }
    public string? SexuallyTransmittedInfectionNotes { get; set; }
    
    public bool? ReproductiveInfection { get; set; }
    public string? ReproductiveInfectionNotes { get; set; }
    
    public bool? HIV { get; set; }
    public string? HIVNotes { get; set; }
    
    public bool? AIDS { get; set; }
    public string? AIDSNotes { get; set; }
    
    public bool? PsychologicalIssues { get; set; }
    public string? PsychologicalIssuesNotes { get; set; }
    
    public bool? GadgetAddiction { get; set; }
    public string? GadgetAddictionNotes { get; set; }
    
    public bool? SexualOrientation { get; set; }
    public string? SexualOrientationNotes { get; set; }
    
    public bool? MentalDisability { get; set; }
    public string? MentalDisabilityNotes { get; set; }
    
    public bool? EarlyMarriage { get; set; }
    public string? EarlyMarriageNotes { get; set; }
    
    public bool? ChildAbuse { get; set; }
    public string? ChildAbuseNotes { get; set; }
    
    public bool? PhysicalDisability { get; set; }
    public string? PhysicalDisabilityNotes { get; set; }
    
    public bool? LearningDifficulty { get; set; }
    public string? LearningDifficultyNotes { get; set; }
    
    public bool? OtherProblems { get; set; }
    public string? OtherProblemsNotes { get; set; }
    
    // Anamnesis Section
    public string? MainProblem { get; set; }
    public string? ProblemBackground { get; set; }
    public string? SolutionAlternatives { get; set; }
    public string? ClientDecision { get; set; }
    public string? Observations { get; set; }
    public string? Counselor { get; set; }
    
    // HEEADSS Assessment
    public string? HomeAssessment { get; set; }
    public string? EmploymentEducationAssessment { get; set; }
    public string? EatingAssessment { get; set; }
    public string? ActivityAssessment { get; set; }
    public string? DrugsAssessment { get; set; }
    public string? SexualityAssessment { get; set; }
    public string? SafetyAssessment { get; set; }
    public string? SuicideDepressionAssessment { get; set; }
    
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? UpdatedAt { get; set; }

    // Relationships
    public Appointment Appointment { get; set; } = null!;
}
