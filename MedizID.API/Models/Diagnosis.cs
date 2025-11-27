namespace MedizID.API.Models;

public class Diagnosis
{
    public Guid Id { get; set; }
    public Guid MedicalRecordId { get; set; }
    public string DiagnosisCode { get; set; } = null!; // ICD-10 code
    public string DiagnosisDescription { get; set; } = null!;
    public string? DiagnosisType { get; set; } // Primary, Secondary, etc.
    public int? ConfidencePercentage { get; set; }
    public string? Reason { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    // Relationships
    public MedicalRecord MedicalRecord { get; set; } = null!;
}
