namespace MedizID.API.Models;

public class Anamnesis
{
    public Guid Id { get; set; }
    public Guid MedicalRecordId { get; set; }
    public string? ChiefComplaint { get; set; }
    public string? PresentingIllness { get; set; }
    public string? PastMedicalHistory { get; set; }
    public string? Allergies { get; set; }
    public string? CurrentMedications { get; set; }
    public string? SocialHistory { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    // Relationships
    public MedicalRecord MedicalRecord { get; set; } = null!;
}
