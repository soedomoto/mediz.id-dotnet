namespace MedizID.API.DTOs;

public class CreateDiagnosisRequest
{
    public Guid MedicalRecordId { get; set; }
    public string DiagnosisCode { get; set; } = null!;
    public string DiagnosisDescription { get; set; } = null!;
    public string? DiagnosisType { get; set; }
    public int? ConfidencePercentage { get; set; }
    public string? Reason { get; set; }
}

public class DiagnosisResponse
{
    public Guid Id { get; set; }
    public string DiagnosisCode { get; set; } = null!;
    public string DiagnosisDescription { get; set; } = null!;
    public string? DiagnosisType { get; set; }
    public int? ConfidencePercentage { get; set; }
    public DateTime CreatedAt { get; set; }
}

public class DiagnosisDetailResponse
{
    public Guid Id { get; set; }
    public string DiagnosisCode { get; set; } = null!;
    public string DiagnosisDescription { get; set; } = null!;
    public string? DiagnosisType { get; set; }
    public int? ConfidencePercentage { get; set; }
    public string? Reason { get; set; }
    public Guid MedicalRecordId { get; set; }
    public DateTime CreatedAt { get; set; }
}

public class PatientDiagnosisDetail
{
    public Guid Id { get; set; }
    public Guid MedicalRecordId { get; set; }
    public string DiagnosisCode { get; set; } = null!;
    public string DiagnosisDescription { get; set; } = null!;
    public string? DiagnosisType { get; set; }
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
