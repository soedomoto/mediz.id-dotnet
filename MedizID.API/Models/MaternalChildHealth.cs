namespace MedizID.API.Models;

public class MaternalChildHealth
{
    public Guid Id { get; set; }
    public Guid AppointmentId { get; set; }
    public string? PregnancyWeeks { get; set; }
    public string? BloodPressure { get; set; }
    public decimal? Weight { get; set; }
    public string? FetalHeartRate { get; set; }
    public string? Notes { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    // Relationships
    public Appointment Appointment { get; set; } = null!;
}
