namespace MedizID.API.Models;

public class Poli
{
    public Guid Id { get; set; }
    public Guid FacilityId { get; set; }
    public Guid InstallationId { get; set; }
    public string Name { get; set; } = null!;
    public string? Description { get; set; }
    public bool IsActive { get; set; } = true;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? UpdatedAt { get; set; }

    // Relationships
    public Facility Facility { get; set; } = null!;
    public Installation Installation { get; set; } = null!;
    public ICollection<PoliTimeSlot> TimeSlots { get; set; } = new List<PoliTimeSlot>();
}
