using MedizID.API.DTOs;

namespace MedizID.API.Services;

/// <summary>
/// Service interface for diagnosis operations
/// </summary>
public interface IDiagnosisService
{
    /// <summary>
    /// Get diagnosis by ID
    /// </summary>
    Task<DiagnosisResponse?> GetDiagnosisAsync(Guid diagnosisId, CancellationToken cancellationToken = default);

    /// <summary>
    /// Get diagnoses for a medical record
    /// </summary>
    Task<IEnumerable<DiagnosisResponse>> GetMedicalRecordDiagnosesAsync(Guid medicalRecordId, CancellationToken cancellationToken = default);

    /// <summary>
    /// Get diagnosis history for a patient
    /// </summary>
    Task<IEnumerable<DiagnosisResponse>> GetPatientDiagnosisHistoryAsync(Guid patientId, int limit = 10, CancellationToken cancellationToken = default);

    /// <summary>
    /// Create new diagnosis
    /// </summary>
    Task<DiagnosisResponse> CreateDiagnosisAsync(CreateDiagnosisRequest request, CancellationToken cancellationToken = default);

    /// <summary>
    /// Update diagnosis
    /// </summary>
    Task<DiagnosisResponse> UpdateDiagnosisAsync(Guid diagnosisId, CreateDiagnosisRequest request, CancellationToken cancellationToken = default);

    /// <summary>
    /// Delete diagnosis
    /// </summary>
    Task<bool> DeleteDiagnosisAsync(Guid diagnosisId, CancellationToken cancellationToken = default);

    /// <summary>
    /// Search diagnoses by ICD10 code
    /// </summary>
    Task<DiagnosisSearchResponse> SearchDiagnosesAsync(DiagnosisSearchRequest request, CancellationToken cancellationToken = default);
}
