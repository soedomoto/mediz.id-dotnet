namespace MedizID.API.Models;

public class Laboratorium
{
    public Guid Id { get; set; }
    public Guid AppointmentId { get; set; }
    public string TestName { get; set; } = null!;
    public string? TestCode { get; set; }
    public string? Result { get; set; }
    public string? Unit { get; set; }
    public string? ReferenceRange { get; set; }
    public string? Status { get; set; } // Normal, Abnormal
    public DateTime TestDate { get; set; } = DateTime.UtcNow;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    // Relationships
    public Appointment Appointment { get; set; } = null!;
}
