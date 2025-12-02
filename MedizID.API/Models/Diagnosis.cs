using MedizID.API.Common.Enums;

namespace MedizID.API.Models;

public class Diagnosis
{
    public Guid Id { get; set; }
    public Guid AppointmentId { get; set; }
    public string ICD10Code { get; set; } = null!; // ICD-10 diagnostic code
    public string ScientificDescription { get; set; } = null!; // Medical/scientific terminology
    public DiagnosisTypeEnum DiagnosisType { get; set; } = DiagnosisTypeEnum.Primary;
    public DiagnosisCaseTypeEnum CaseType { get; set; } = DiagnosisCaseTypeEnum.New;
    public int? ConfidencePercentage { get; set; }
    public string? ClinicalNotes { get; set; }
    public bool? IsRecommendedByAI { get; set; }
    public int? AIRecommendationConfidence { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? UpdatedAt { get; set; }

    // Relationships
    public Appointment Appointment { get; set; } = null!;
}
