namespace MedizID.API.Models;

public class Prescription
{
    public Guid Id { get; set; }
    public Guid AppointmentId { get; set; }
    public string MedicationName { get; set; } = null!;
    public string Dosage { get; set; } = null!;
    public string Frequency { get; set; } = null!;
    public int Duration { get; set; } // in days
    public string? Instructions { get; set; }
    public DateTime? ExpiryDate { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    // Relationships
    public Appointment Appointment { get; set; } = null!;
}
