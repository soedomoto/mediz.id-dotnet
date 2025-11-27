namespace MedizID.API.DTOs;

public class CreateSTIRequest
{
    public Guid MedicalRecordId { get; set; }
    public Guid? AppointmentId { get; set; }
    public string VisitStatus { get; set; } = null!;
    public string RiskGroup { get; set; } = null!;
    public string? Screening { get; set; }
    public string? Diagnosis { get; set; }
    public string? Treatment { get; set; }
}

public class UpdateSTIRequest
{
    public string? VisitStatus { get; set; }
    public string? RiskGroup { get; set; }
    public string? Screening { get; set; }
    public string? Diagnosis { get; set; }
    public string? Treatment { get; set; }
}

public class STIResponse
{
    public Guid Id { get; set; }
    public Guid MedicalRecordId { get; set; }
    public Guid? AppointmentId { get; set; }
    public string VisitStatus { get; set; } = null!;
    public string RiskGroup { get; set; } = null!;
    public string? Screening { get; set; }
    public string? Diagnosis { get; set; }
    public string? Treatment { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
}

public class STIListResponse
{
    public List<STIResponse> Items { get; set; } = new();
    public int Total { get; set; }
}
