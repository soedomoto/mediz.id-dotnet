namespace MedizID.API.DTOs;

public class CreateDiagnosisRequest
{
    public Guid AppointmentId { get; set; }
    public string ICD10Code { get; set; } = null!;
    public string ScientificDescription { get; set; } = null!;
    public string DiagnosisType { get; set; } = "Primary"; // Primary, Secondary, Complication
    public string CaseType { get; set; } = "New"; // New, Existing
    public int? ConfidencePercentage { get; set; }
    public string? ClinicalNotes { get; set; }
    public string? Description { get; set; }
}

public class DiagnosisResponse
{
    public Guid Id { get; set; }
    public string ICD10Code { get; set; } = null!;
    public string ScientificDescription { get; set; } = null!;
    public string DiagnosisType { get; set; } = null!;
    public string CaseType { get; set; } = null!;
    public int? ConfidencePercentage { get; set; }
    public DateTime CreatedAt { get; set; }
}

public class DiagnosisDetailResponse
{
    public Guid Id { get; set; }
    public string ICD10Code { get; set; } = null!;
    public string ScientificDescription { get; set; } = null!;
    public string DiagnosisType { get; set; } = null!;
    public string CaseType { get; set; } = null!;
    public int? ConfidencePercentage { get; set; }
    public string? ClinicalNotes { get; set; }
    public string? Description { get; set; }
    public bool? IsRecommendedByAI { get; set; }
    public int? AIRecommendationConfidence { get; set; }
    public Guid AppointmentId { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
}

public class PatientDiagnosisDetail
{
    public Guid Id { get; set; }
    public Guid AppointmentId { get; set; }
    public string ICD10Code { get; set; } = null!;
    public string ScientificDescription { get; set; } = null!;
    public string DiagnosisType { get; set; } = null!;
    public string CaseType { get; set; } = null!;
    public int? ConfidencePercentage { get; set; }
    public DateTime CreatedAt { get; set; }
}

public class CreateICD10CodeRequest
{
    public string Code { get; set; } = null!;
    public string Description { get; set; } = null!;
    public string? Category { get; set; }
}

public class ICD10CodeResponse
{
    public Guid Id { get; set; }
    public string Code { get; set; } = null!;
    public string Description { get; set; } = null!;
    public string? Category { get; set; }
    public bool IsActive { get; set; }
    public DateTime CreatedAt { get; set; }
}

public class DiagnosisSearchRequest
{
    public string? Query { get; set; }
    public string? Category { get; set; }
    public int Page { get; set; } = 1;
    public int PageSize { get; set; } = 10;
}

public class DiagnosisSearchResponse
{
    public List<ICD10CodeResponse> Results { get; set; } = new();
    public int Total { get; set; }
    public int Page { get; set; }
    public int PageSize { get; set; }
}

public class GenerateDiagnosisRecommendationRequest
{
    public Guid AppointmentId { get; set; }
    public string? Symptoms { get; set; }
    public string? ClinicalFindings { get; set; }
    public int? MaxRecommendations { get; set; } = 5;
}

public class DiagnosisRecommendation
{
    public string ICD10Code { get; set; } = null!;
    public string ScientificDescription { get; set; } = null!;
    public int ConfidenceScore { get; set; }
    public string? Reasoning { get; set; }
}

public class GenerateDiagnosisRecommendationResponse
{
    public Guid AppointmentId { get; set; }
    public List<DiagnosisRecommendation> Recommendations { get; set; } = new();
    public DateTime GeneratedAt { get; set; }
}

