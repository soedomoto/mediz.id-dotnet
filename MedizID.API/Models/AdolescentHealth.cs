namespace MedizID.API.Models;

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
