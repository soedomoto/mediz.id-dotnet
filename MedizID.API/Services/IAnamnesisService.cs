using MedizID.API.DTOs;

namespace MedizID.API.Services;

/// <summary>
/// Service interface for anamnesis operations
/// </summary>
public interface IAnamnesisService
{
    /// <summary>
    /// Get anamnesis by ID
    /// </summary>
    Task<AnamnesisResponse?> GetAnamnesisAsync(Guid anamnesisId, CancellationToken cancellationToken = default);

    /// <summary>
    /// Get anamnesis for a medical record
    /// </summary>
    Task<AnamnesisResponse?> GetMedicalRecordAnamnesisAsync(Guid medicalRecordId, CancellationToken cancellationToken = default);

    /// <summary>
    /// Get patient's anamnesis history
    /// </summary>
    Task<IEnumerable<AnamnesisResponse>> GetPatientAnamnesisHistoryAsync(Guid patientId, CancellationToken cancellationToken = default);

    /// <summary>
    /// Create new anamnesis
    /// </summary>
    Task<AnamnesisResponse> CreateAnamnesisAsync(CreateAnamnesisRequest request, CancellationToken cancellationToken = default);

    /// <summary>
    /// Update anamnesis
    /// </summary>
    Task<AnamnesisResponse> UpdateAnamnesisAsync(Guid anamnesisId, CreateAnamnesisRequest request, CancellationToken cancellationToken = default);
}
