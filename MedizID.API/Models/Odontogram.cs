namespace MedizID.API.Models;

public class Odontogram
{
    public Guid Id { get; set; }
    public Guid AppointmentId { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? UpdatedAt { get; set; }

    // Relationships
    public Appointment Appointment { get; set; } = null!;
    public ICollection<OdontogramSurface> Surfaces { get; set; } = new List<OdontogramSurface>();
}

public class OdontogramSurface
{
    public Guid Id { get; set; }
    public Guid OdontogramId { get; set; }
    public int ToothNumber { get; set; }
    public string Surface { get; set; } = null!; // Buccal, Lingual, Occlusal, Mesial, Distal
    public string ConditionCode { get; set; } = null!; // Code from conditions (e.g., "car" for caries, "sou" for healthy)
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? UpdatedAt { get; set; }

    // Relationships
    public Odontogram Odontogram { get; set; } = null!;
}
