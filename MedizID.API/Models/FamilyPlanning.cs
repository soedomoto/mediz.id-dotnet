namespace MedizID.API.Models;

public class FamilyPlanning
{
    public Guid Id { get; set; }
    public Guid AppointmentId { get; set; }
    public string? CurrentMethod { get; set; }
    public DateTime? StartDate { get; set; }
    public string? Status { get; set; }
    public string? Notes { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    // Relationships
    public Appointment Appointment { get; set; } = null!;
}
