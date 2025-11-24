using MedizID.API.Common.Enums;

namespace MedizID.API.Models;

public class FamilyPlanning
{
    public Guid Id { get; set; }
    public Guid MedicalRecordId { get; set; }
    public string? CurrentMethod { get; set; }
    public DateTime? StartDate { get; set; }
    public string? Status { get; set; }
    public string? Notes { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    // Relationships
    public MedicalRecord MedicalRecord { get; set; } = null!;
}

public class MaternalChildHealth
{
    public Guid Id { get; set; }
    public Guid MedicalRecordId { get; set; }
    public string? PregnancyWeeks { get; set; }
    public string? BloodPressure { get; set; }
    public decimal? Weight { get; set; }
    public string? FetalHeartRate { get; set; }
    public string? Notes { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    // Relationships
    public MedicalRecord MedicalRecord { get; set; } = null!;
}

public class Immunization
{
    public Guid Id { get; set; }
    public Guid MedicalRecordId { get; set; }
    public string VaccineName { get; set; } = null!;
    public DateTime VaccineDate { get; set; }
    public int? DoseNumber { get; set; }
    public string? Lot { get; set; }
    public string? Route { get; set; }
    public string? Site { get; set; }
    public string? Reactions { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    // Relationships
    public MedicalRecord MedicalRecord { get; set; } = null!;
}

public class MedicalProcedure
{
    public Guid Id { get; set; }
    public Guid MedicalRecordId { get; set; }
    public string ProcedureName { get; set; } = null!;
    public DateTime ProcedureDate { get; set; }
    public string? Indication { get; set; }
    public string? Findings { get; set; }
    public string? Complications { get; set; }
    public string? Notes { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    // Relationships
    public MedicalRecord MedicalRecord { get; set; } = null!;
}

public class Odontogram
{
    public Guid Id { get; set; }
    public Guid MedicalRecordId { get; set; }
    public string? ToothNumber { get; set; }
    public string? Surface { get; set; }
    public string? Status { get; set; } // Caries, Filling, Missing, etc.
    public string? Treatment { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    // Relationships
    public MedicalRecord MedicalRecord { get; set; } = null!;
}

public class AdolescentHealth
{
    public Guid Id { get; set; }
    public Guid MedicalRecordId { get; set; }
    public string? PubertanStage { get; set; }
    public decimal? Height { get; set; }
    public decimal? Weight { get; set; }
    public decimal? BMI { get; set; }
    public string? RiskyBehaviors { get; set; }
    public string? MentalHealthStatus { get; set; }
    public string? Notes { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    // Relationships
    public MedicalRecord MedicalRecord { get; set; } = null!;
}

public class HIVCounseling
{
    public Guid Id { get; set; }
    public Guid MedicalRecordId { get; set; }
    public HIVCounselingVisitStatusEnum VisitStatus { get; set; }
    public HIVCounselingClientStatusEnum ClientStatus { get; set; }
    public HIVCounselingRiskGroupEnum? RiskGroup { get; set; }
    public HIVCounselingPregnancyStatusEnum? PregnancyStatus { get; set; }
    public HIVCounselingPartnerHIVStatusEnum? PartnerHIVStatus { get; set; }
    public HIVCounselingTestReasonEnum? TestReason { get; set; }
    public HIVCounselingTestSourceEnum? TestSource { get; set; }
    public HIVCounselingPreviousTestResultEnum? PreviousTestResult { get; set; }
    public DateTime? LastTestDate { get; set; }
    public string? TestResult { get; set; }
    public string? Notes { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    // Relationships
    public MedicalRecord MedicalRecord { get; set; } = null!;
}

public class STI
{
    public Guid Id { get; set; }
    public Guid MedicalRecordId { get; set; }
    public STIVisitStatusEnum VisitStatus { get; set; }
    public STIRiskGroupEnum? RiskGroup { get; set; }
    public STIVisitReasonEnum? VisitReason { get; set; }
    public string? Symptoms { get; set; }
    public string? DiagnosisSTI { get; set; }
    public string? Treatment { get; set; }
    public string? Partner { get; set; }
    public string? Notes { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    // Relationships
    public MedicalRecord MedicalRecord { get; set; } = null!;
}
