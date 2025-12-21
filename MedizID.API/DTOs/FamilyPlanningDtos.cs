namespace MedizID.API.DTOs;

/// <summary>
/// Request DTO for creating/updating Family Planning records
/// </summary>
public class CreateFamilyPlanningRequest
{
    public Guid AppointmentId { get; set; }
    
    // Section VI-VII: Spouse Information and Education
    public string? SpouseName { get; set; }
    public int? HusbandEducation { get; set; }
    public int? WifeEducation { get; set; }
    
    // Section VIII: Employment
    public string? HusbandOccupation { get; set; }
    public string? WifeOccupation { get; set; }
    
    // Section IX-X: Family Planning Status
    public int? FamilyPlanningStage { get; set; }
    public int? NumberOfLivingChildren { get; set; }
    
    // Section XI: Youngest Child Age
    public int? YoungestChildYears { get; set; }
    public int? YoungestChildMonths { get; set; }
    
    // Section XII: KB Participant Status
    public int? KBParticipantStatus { get; set; }
    public int? LastContraceptiveMethod { get; set; }
    
    // Section 1-8: Pre-Insertion Examination
    public bool? PregnancySigns { get; set; }
    public bool? AbnormalVaginalDischarge { get; set; }
    public bool? AbdominalPain { get; set; }
    public bool? EctopicPregnancyHistory { get; set; }
    public bool? AbnormalUterinebleeding { get; set; }
    public bool? IUDStillInPlace { get; set; }
    public bool? PelvicPain { get; set; }
    public bool? Dysmenorrhea { get; set; }
    
    // Section 9: Internal Examination Findings
    public bool? InflammationSigns { get; set; }
    public bool? TumorOrMalignancy { get; set; }
    
    // Section 10: Uterine Position
    public int? UterinePosition { get; set; }
    
    // Section 11: Additional Examination for MOP/MOW
    public bool? DiabetesSigns { get; set; }
    public bool? BloodClottingDisorder { get; set; }
    public bool? OrchitisEpididymitis { get; set; }
    public bool? TumorOrMalignancyMOP { get; set; }
    
    // Section 12-17: Contraceptive Selection and Service
    public string? AllowedContraceptiveMethods { get; set; }
    public string? SelectedContraceptiveMethod { get; set; }
    public DateTime? ServiceDate { get; set; }
    public DateTime? FollowUpDate { get; set; }
    public DateTime? RemovalDate { get; set; }
    
    // Observation/Monitoring
    public string? ObservationNotes { get; set; }
    
    // Collections
    public List<CreateFamilyPlanningContraceptiveMethodRequest>? ContraceptiveMethods { get; set; }
}

/// <summary>
/// Response DTO for Family Planning records
/// </summary>
public class FamilyPlanningResponse
{
    public Guid Id { get; set; }
    public Guid AppointmentId { get; set; }
    
    // Section VI-VII: Spouse Information and Education
    public string? SpouseName { get; set; }
    public int? HusbandEducation { get; set; }
    public int? WifeEducation { get; set; }
    
    // Section VIII: Employment
    public string? HusbandOccupation { get; set; }
    public string? WifeOccupation { get; set; }
    
    // Section IX-X: Family Planning Status
    public int? FamilyPlanningStage { get; set; }
    public int? NumberOfLivingChildren { get; set; }
    
    // Section XI: Youngest Child Age
    public int? YoungestChildYears { get; set; }
    public int? YoungestChildMonths { get; set; }
    
    // Section XII: KB Participant Status
    public int? KBParticipantStatus { get; set; }
    public int? LastContraceptiveMethod { get; set; }
    
    // Section 1-8: Pre-Insertion Examination
    public bool? PregnancySigns { get; set; }
    public bool? AbnormalVaginalDischarge { get; set; }
    public bool? AbdominalPain { get; set; }
    public bool? EctopicPregnancyHistory { get; set; }
    public bool? AbnormalUterinebleeding { get; set; }
    public bool? IUDStillInPlace { get; set; }
    public bool? PelvicPain { get; set; }
    public bool? Dysmenorrhea { get; set; }
    
    // Section 9: Internal Examination Findings
    public bool? InflammationSigns { get; set; }
    public bool? TumorOrMalignancy { get; set; }
    
    // Section 10: Uterine Position
    public int? UterinePosition { get; set; }
    
    // Section 11: Additional Examination for MOP/MOW
    public bool? DiabetesSigns { get; set; }
    public bool? BloodClottingDisorder { get; set; }
    public bool? OrchitisEpididymitis { get; set; }
    public bool? TumorOrMalignancyMOP { get; set; }
    
    // Section 12-17: Contraceptive Selection and Service
    public string? AllowedContraceptiveMethods { get; set; }
    public string? SelectedContraceptiveMethod { get; set; }
    public DateTime? ServiceDate { get; set; }
    public DateTime? FollowUpDate { get; set; }
    public DateTime? RemovalDate { get; set; }
    
    // Observation/Monitoring
    public string? ObservationNotes { get; set; }
    
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
    
    // Collections
    public List<FamilyPlanningContraceptiveMethodDto> ContraceptiveMethods { get; set; } = new();
    public List<FamilyPlanningLaboratoryResultDto> LaboratoryResults { get; set; } = new();
    public List<FamilyPlanningProcedureDto> Procedures { get; set; } = new();
}

/// <summary>
/// DTO for creating/updating Contraceptive Methods
/// </summary>
public class CreateFamilyPlanningContraceptiveMethodRequest
{
    public int? MethodType { get; set; }
    public DateTime? ServiceDate { get; set; }
    public int? Quantity { get; set; }
    public string? Notes { get; set; }
}

/// <summary>
/// DTO for Contraceptive Methods
/// </summary>
public class FamilyPlanningContraceptiveMethodDto
{
    public Guid Id { get; set; }
    public Guid FamilyPlanningId { get; set; }
    public int? MethodType { get; set; }
    public DateTime? ServiceDate { get; set; }
    public int? Quantity { get; set; }
    public string? Notes { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
    
    /// <summary>
    /// Temporary ID for client-side operations (not persisted)
    /// </summary>
    [System.Text.Json.Serialization.JsonIgnore]
    public Guid? TempId { get; set; }
}

/// <summary>
/// DTO for Laboratory Results
/// </summary>
public class FamilyPlanningLaboratoryResultDto
{
    public Guid Id { get; set; }
    public Guid FamilyPlanningId { get; set; }
    public string? TestName { get; set; }
    public string? Result { get; set; }
    public string? ReferenceValue { get; set; }
    public DateTime? TestDate { get; set; }
    public string? Notes { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
}

/// <summary>
/// DTO for Procedures
/// </summary>
public class FamilyPlanningProcedureDto
{
    public Guid Id { get; set; }
    public Guid FamilyPlanningId { get; set; }
    public string? ProcedureName { get; set; }
    public DateTime? ProcedureDate { get; set; }
    public string? PerformedBy { get; set; }
    public string? Outcome { get; set; }
    public string? Complications { get; set; }
    public string? Notes { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
}
