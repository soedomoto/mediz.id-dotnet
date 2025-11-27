namespace MedizID.API.Models;

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
