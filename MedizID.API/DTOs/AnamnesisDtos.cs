using MedizID.API.Common.Enums;

namespace MedizID.API.DTOs;

public class CreateAnamnesisRequest
{
    public Guid AppointmentId { get; set; }
    
    #region Chief Complaint and Presenting Illness
    
    public string? ChiefComplaint { get; set; }
    public string? AdditionalComplaints { get; set; }
    public int? DurationYears { get; set; }
    public int? DurationMonths { get; set; }
    public int? DurationDays { get; set; }
    public string? PresentIllness { get; set; }
    public string? PresentingIllness { get; set; } // Alias for compatibility
    
    #endregion
    
    #region Medical History
    
    public string? MedicalHistory { get; set; }
    public string? FamilyHistory { get; set; }
    
    #endregion
    
    #region Allergies
    
    public string? AllergiesHistory { get; set; }
    public string? DrugAllergies { get; set; }
    public string? FoodAllergies { get; set; }
    public string? OtherAllergies { get; set; }
    
    #endregion
    
    #region Medications and Social History
    
    public string? MedicationHistory { get; set; }
    public string? SocialHistory { get; set; }
    public string? SurgicalHistory { get; set; }
    public bool? SmokingStatus { get; set; }
    public bool? AlcoholConsumption { get; set; }
    public bool? InsufficientVegetableFruitIntake { get; set; }
    
    #endregion
    
    #region Physical Examination - Vital Signs
    
    public string? GeneralAppearance { get; set; }
    public ConsciousnessLevel? ConsciousnessLevel { get; set; }
    public double? BodyTemperature { get; set; }
    public int? RespiratoryRate { get; set; }
    public int? PulseRate { get; set; }
    public HeartRhythm? HeartRhythm { get; set; }
    public int? SystolicBloodPressure { get; set; }
    public int? DiastolicBloodPressure { get; set; }
    public double? BodyWeight { get; set; }
    public double? Height { get; set; }
    public HeightMeasurementMethod? HeightMeasurementMethod { get; set; }
    public double? WaistCircumference { get; set; }
    public double? BodyMassIndex { get; set; }
    public string? BMIClassification { get; set; }
    public double? OxygenSaturation { get; set; }
    public int? PainScale { get; set; }
    public string? PhysicalActivity { get; set; }
    
    #endregion
    
    #region Detailed System Examination
    
    public string? HeadExamination { get; set; }
    public string? EyeExamination { get; set; }
    public string? EarExamination { get; set; }
    public string? NoseExamination { get; set; }
    public string? MouthExamination { get; set; }
    public string? ThroatExamination { get; set; }
    public string? LungExamination { get; set; }
    public string? CardiovascularExamination { get; set; }
    public string? ChestAxillaExamination { get; set; }
    public string? AbdominalExamination { get; set; }
    public string? UpperExtremityExamination { get; set; }
    public string? LowerExtremityExamination { get; set; }
    public string? GenitaliaExamination { get; set; }
    public string? SkinExamination { get; set; }
    public string? NailExamination { get; set; }
    public string? NeckExamination { get; set; }
    public string? NeurologicalExamination { get; set; }
    
    #endregion
    
    #region Assessment and Triage
    
    public TriageLevel? TriageLevel { get; set; }
    
    #endregion
    
    #region Clinical Documentation
    
    public string? BodyAnatomyFindings { get; set; }
    public string? ClinicalNotes { get; set; }
    public string? AdditionalFindings { get; set; }
    public string? ObservationNotes { get; set; }
    public string? AdditionalRemarks { get; set; }
    public string? BiopsychosocialAssessment { get; set; }
    
    #endregion
    
    #region Assessment and Plan
    
    public string? SubjectiveAssessment { get; set; }
    public string? ObjectiveFindings { get; set; }
    public string? Assessment { get; set; }
    public string? PlanOfCare { get; set; }
    public NursingCareType? NursingCareType { get; set; }
    public string? NursingCareDescription { get; set; }
    public string? NursingInterventions { get; set; }
    public string? PatientEducation { get; set; }
    public string? Treatment { get; set; }
    
    #endregion
}

public class AnamnesisResponse
{
    public Guid Id { get; set; }
    public Guid AppointmentId { get; set; }
    
    #region Chief Complaint and Presenting Illness
    
    public string? ChiefComplaint { get; set; }
    public string? AdditionalComplaints { get; set; }
    public int? DurationYears { get; set; }
    public int? DurationMonths { get; set; }
    public int? DurationDays { get; set; }
    public string? PresentingIllness { get; set; }
    
    #endregion
    
    #region Medical History
    
    public string? MedicalHistory { get; set; }
    public string? FamilyHistory { get; set; }
    public string? AllergiesHistory { get; set; }
    public string? DrugAllergies { get; set; }
    public string? FoodAllergies { get; set; }
    public string? OtherAllergies { get; set; }
    
    #endregion
    
    #region Medications and Social History
    
    public string? CurrentMedications { get; set; }
    public string? SocialHistory { get; set; }
    public bool? SmokingStatus { get; set; }
    public bool? AlcoholConsumption { get; set; }
    public bool? InsufficientVegetableFruitIntake { get; set; }
    
    #endregion
    
    #region Physical Examination - Vital Signs
    
    public string? GeneralAppearance { get; set; }
    public ConsciousnessLevel? ConsciousnessLevel { get; set; }
    public double? BodyTemperature { get; set; }
    public int? RespiratoryRate { get; set; }
    public int? PulseRate { get; set; }
    public HeartRhythm? HeartRhythm { get; set; }
    public int? SystolicBloodPressure { get; set; }
    public int? DiastolicBloodPressure { get; set; }
    public double? BodyWeight { get; set; }
    public double? Height { get; set; }
    public HeightMeasurementMethod? HeightMeasurementMethod { get; set; }
    public double? WaistCircumference { get; set; }
    public double? BodyMassIndex { get; set; }
    public string? BMIClassification { get; set; }
    public double? OxygenSaturation { get; set; }
    public int? PainScale { get; set; }
    public string? PhysicalActivity { get; set; }
    
    #endregion
    
    #region Detailed System Examination
    
    public string? HeadExamination { get; set; }
    public string? EyeExamination { get; set; }
    public string? EarExamination { get; set; }
    public string? NoseExamination { get; set; }
    public string? MouthExamination { get; set; }
    public string? ThroatExamination { get; set; }
    public string? LungExamination { get; set; }
    public string? CardiovascularExamination { get; set; }
    public string? ChestAxillaExamination { get; set; }
    public string? AbdominalExamination { get; set; }
    public string? UpperExtremityExamination { get; set; }
    public string? LowerExtremityExamination { get; set; }
    public string? GenitaliaExamination { get; set; }
    public string? SkinExamination { get; set; }
    public string? NailExamination { get; set; }
    public string? NeckExamination { get; set; }
    public string? NeurologicalExamination { get; set; }
    
    #endregion
    
    #region Assessment and Triage
    
    public TriageLevel? TriageLevel { get; set; }
    
    #endregion
    
    #region Clinical Documentation
    
    public string? BodyAnatomyFindings { get; set; }
    public string? ClinicalNotes { get; set; }
    public string? AdditionalFindings { get; set; }
    public string? ObservationNotes { get; set; }
    public string? AdditionalRemarks { get; set; }
    public string? BiopsychosocialAssessment { get; set; }
    
    #endregion
    
    #region Assessment and Plan
    
    public string? SubjectiveAssessment { get; set; }
    public string? ObjectiveFindings { get; set; }
    public string? Assessment { get; set; }
    public string? PlanOfCare { get; set; }
    public NursingCareType? NursingCareType { get; set; }
    public string? NursingCareDescription { get; set; }
    public string? NursingInterventions { get; set; }
    public string? PatientEducation { get; set; }
    public string? Treatment { get; set; }
    
    #endregion
    
    public DateTime CreatedAt { get; set; }
}

public class AnamnesisCreateResponse
{
    public Guid Id { get; set; }
    public Guid AppointmentId { get; set; }
    public string Message { get; set; } = "Anamnesis created successfully";
}

public class AnamnesisUpdateResponse
{
    public Guid Id { get; set; }
    public string Message { get; set; } = "Anamnesis updated successfully";
}

public class PatientAnamnesisPublic
{
    public Guid Id { get; set; }
    public Guid AppointmentId { get; set; }
    public string? ChiefComplaint { get; set; }
    public string? PresentingIllness { get; set; }
    public DateTime CreatedAt { get; set; }
}

public class PatientAnamnesisDetail
{
    public Guid Id { get; set; }
    public Guid AppointmentId { get; set; }
    
    #region Chief Complaint and Presenting Illness
    
    public string? ChiefComplaint { get; set; }
    public string? AdditionalComplaints { get; set; }
    public int? DurationYears { get; set; }
    public int? DurationMonths { get; set; }
    public int? DurationDays { get; set; }
    public string? PresentingIllness { get; set; }
    
    #endregion
    
    #region Medical History
    
    public string? PastMedicalHistory { get; set; }
    public string? FamilyHistory { get; set; }
    
    #endregion
    
    #region Allergies
    
    public string? DrugAllergies { get; set; }
    public string? FoodAllergies { get; set; }
    public string? OtherAllergies { get; set; }
    
    #endregion
    
    #region Medications and Social History
    
    public string? CurrentMedications { get; set; }
    public string? SocialHistory { get; set; }
    public bool? SmokingStatus { get; set; }
    public bool? AlcoholConsumption { get; set; }
    public bool? InsufficientVegetableFruitIntake { get; set; }
    
    #endregion
    
    #region Physical Examination - Vital Signs
    
    public string? GeneralAppearance { get; set; }
    public ConsciousnessLevel? ConsciousnessLevel { get; set; }
    public double? BodyTemperature { get; set; }
    public int? RespiratoryRate { get; set; }
    public int? PulseRate { get; set; }
    public HeartRhythm? HeartRhythm { get; set; }
    public int? SystolicBloodPressure { get; set; }
    public int? DiastolicBloodPressure { get; set; }
    public double? BodyWeight { get; set; }
    public double? Height { get; set; }
    public HeightMeasurementMethod? HeightMeasurementMethod { get; set; }
    public double? WaistCircumference { get; set; }
    public double? BodyMassIndex { get; set; }
    public string? BMIClassification { get; set; }
    public double? OxygenSaturation { get; set; }
    public int? PainScale { get; set; }
    public string? PhysicalActivity { get; set; }
    
    #endregion
    
    #region Detailed System Examination
    
    public string? HeadExamination { get; set; }
    public string? EyeExamination { get; set; }
    public string? EarExamination { get; set; }
    public string? NoseExamination { get; set; }
    public string? MouthExamination { get; set; }
    public string? ThroatExamination { get; set; }
    public string? LungExamination { get; set; }
    public string? CardiovascularExamination { get; set; }
    public string? ChestAxillaExamination { get; set; }
    public string? AbdominalExamination { get; set; }
    public string? UpperExtremityExamination { get; set; }
    public string? LowerExtremityExamination { get; set; }
    public string? GenitaliaExamination { get; set; }
    public string? SkinExamination { get; set; }
    public string? NailExamination { get; set; }
    public string? NeckExamination { get; set; }
    public string? NeurologicalExamination { get; set; }
    
    #endregion
    
    #region Assessment and Triage
    
    public TriageLevel? TriageLevel { get; set; }
    
    #endregion
    
    #region Clinical Documentation
    
    public string? BodyAnatomyFindings { get; set; }
    public string? ClinicalNotes { get; set; }
    public string? AdditionalFindings { get; set; }
    public string? ObservationNotes { get; set; }
    public string? AdditionalRemarks { get; set; }
    public string? BiopsychosocialAssessment { get; set; }
    
    #endregion
    
    #region Assessment and Plan
    
    public string? SubjectiveAssessment { get; set; }
    public string? ObjectiveFindings { get; set; }
    public string? Assessment { get; set; }
    public string? PlanOfCare { get; set; }
    public NursingCareType? NursingCareType { get; set; }
    public string? NursingCareDescription { get; set; }
    public string? NursingInterventions { get; set; }
    public string? PatientEducation { get; set; }
    public string? Treatment { get; set; }
    
    #endregion
    
    public DateTime CreatedAt { get; set; }
}

public class CreateAnamnesisTemplateRequest
{
    public string Name { get; set; } = null!;
    public string? Description { get; set; }
    public string? TemplateContent { get; set; }
}

public class UpdateAnamnesisTemplateRequest
{
    public string? Name { get; set; }
    public string? Description { get; set; }
    public string? TemplateContent { get; set; }
    public bool? IsActive { get; set; }
}

public class AnamnesisTemplateResponse
{
    public Guid Id { get; set; }
    public Guid FacilityId { get; set; }
    public string Name { get; set; } = null!;
    public string? Description { get; set; }
    public string? TemplateContent { get; set; }
    public bool IsActive { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
}
