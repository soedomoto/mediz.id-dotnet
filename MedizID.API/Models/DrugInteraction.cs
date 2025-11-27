namespace MedizID.API.Models;

public class DrugInteraction
{
    public Guid Id { get; set; }
    public Guid Drug1Id { get; set; }
    public Guid Drug2Id { get; set; }
    public string? InteractionSeverity { get; set; } // Minor, Moderate, Major
    public string? Description { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}
