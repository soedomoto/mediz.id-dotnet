namespace MedizID.API.Models;

public class AIRecommendation
{
    public Guid Id { get; set; }
    public Guid AppointmentId { get; set; }
    public string RecommendationType { get; set; } = null!; // Diagnosis, Prescription, etc.
    public string Content { get; set; } = null!;
    public float ConfidenceScore { get; set; }
    public string? FeedbackStatus { get; set; } // Accepted, Rejected, Pending
    public string? FeedbackNotes { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? UpdatedAt { get; set; }

    // Relationships
    public Appointment Appointment { get; set; } = null!;
}
