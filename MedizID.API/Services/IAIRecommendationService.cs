using MedizID.API.DTOs;

namespace MedizID.API.Services;

/// <summary>
/// Service interface for AI recommendations
/// </summary>
public interface IAIRecommendationService
{
    /// <summary>
    /// Get AI recommendations for a medical record
    /// </summary>
    Task<IEnumerable<AIRecommendationResponse>> GetRecommendationsAsync(Guid medicalRecordId, CancellationToken cancellationToken = default);

    /// <summary>
    /// Generate diagnosis recommendations
    /// </summary>
    Task<AIDiagnosisResponse> GenerateDiagnosisRecommendationsAsync(Guid medicalRecordId, CancellationToken cancellationToken = default);

    /// <summary>
    /// Generate prescription recommendations
    /// </summary>
    Task<PrescriptionAIResponse> GeneratePrescriptionRecommendationsAsync(Guid medicalRecordId, CancellationToken cancellationToken = default);

    /// <summary>
    /// Provide feedback on AI recommendation
    /// </summary>
    Task<bool> ProvideFeedbackAsync(Guid recommendationId, UpdateAIRecommendationFeedbackRequest request, CancellationToken cancellationToken = default);

    /// <summary>
    /// Get AI recommendation acceptance rate
    /// </summary>
    Task<float> GetAcceptanceRateAsync(CancellationToken cancellationToken = default);
}
