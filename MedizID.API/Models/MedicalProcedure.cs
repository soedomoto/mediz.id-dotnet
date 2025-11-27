namespace MedizID.API.Models;

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
