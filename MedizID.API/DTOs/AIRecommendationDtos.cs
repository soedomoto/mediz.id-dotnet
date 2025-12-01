namespace MedizID.API.DTOs;

public class CreateAIRecommendationRequest
{
    public Guid AppointmentId { get; set; }
    public string RecommendationType { get; set; } = null!;
    public string Content { get; set; } = null!;
    public float ConfidenceScore { get; set; }
}

public class UpdateAIRecommendationFeedbackRequest
{
    public string? FeedbackStatus { get; set; }
    public string? FeedbackNotes { get; set; }
}

public class AIRecommendationResponse
{
    public Guid Id { get; set; }
    public Guid AppointmentId { get; set; }
    public string RecommendationType { get; set; } = null!;
    public string Content { get; set; } = null!;
    public float ConfidenceScore { get; set; }
    public string? FeedbackStatus { get; set; }
    public string? FeedbackNotes { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
}

public class AIRecommendationPrescriptionRequest
{
    public Guid AppointmentId { get; set; }
}

public class PrescriptionAIResponse
{
    public List<AIRecommendationResponse> Recommendations { get; set; } = new();
}

public class AIDiagnosisResponse
{
    public List<AIRecommendationResponse> Recommendations { get; set; } = new();
}
