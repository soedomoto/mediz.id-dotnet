using System.Text.Json.Serialization;

namespace MedizID.API.DTOs;

public class CreateProcedureRequest
{
    public Guid AppointmentId { get; set; }
    public string ICD10PCSCode { get; set; } = null!;
    public string ICD10PCSDescription { get; set; } = null!;
    public string? Notes { get; set; }
    public string? ProcedureType { get; set; } = "Regular";
    public DateTime PlannedAt { get; set; }
}

public class UpdateProcedureRequest
{
    public string ICD10PCSCode { get; set; } = null!;
    public string ICD10PCSDescription { get; set; } = null!;
    public string? Notes { get; set; }
    public string? ProcedureType { get; set; }
    public DateTime? PlannedAt { get; set; }
    public DateTime? StartedAt { get; set; }
    public DateTime? FinishedAt { get; set; }
}

public class ProcedureDetailResponse
{
    public Guid Id { get; set; }
    public Guid AppointmentId { get; set; }
    public string ICD10PCSCode { get; set; } = null!;
    public string ICD10PCSDescription { get; set; } = null!;
    public string? Notes { get; set; }
    public string ProcedureType { get; set; } = "Regular";
    public DateTime PlannedAt { get; set; }
    public DateTime? StartedAt { get; set; }
    public DateTime? FinishedAt { get; set; }
    public bool? IsRecommendedByAI { get; set; }
    public int? AIRecommendationConfidence { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
}

public class ProcedureRecommendationItem
{
    [JsonPropertyName("icd10pcsCode")]
    public string Icd10pcsCode { get; set; } = null!;
    [JsonPropertyName("scientificDescription")]
    public string ScientificDescription { get; set; } = null!;
    [JsonPropertyName("confidenceScore")]
    public int ConfidenceScore { get; set; }
    [JsonPropertyName("clinicalNotes")]
    public string ClinicalNotes { get; set; } = null!;
}

public class ProcedureRecommendationResponse
{
    [JsonPropertyName("possibleProcedures")]
    public List<ProcedureRecommendationItem>? PossibleProcedures { get; set; }
}
