using MedizID.API.DTOs;

namespace MedizID.API.Services;

/// <summary>
/// Service interface for medical record operations
/// </summary>
public interface IMedicalRecordService
{
    /// <summary>
    /// Get medical record by ID
    /// </summary>
    Task<MedicalRecordDetail?> GetMedicalRecordAsync(Guid recordId, CancellationToken cancellationToken = default);

    /// <summary>
    /// Get medical records for a patient
    /// </summary>
    Task<IEnumerable<MedicalRecordDetail>> GetPatientMedicalRecordsAsync(Guid patientId, CancellationToken cancellationToken = default);

    /// <summary>
    /// Get medical records for an appointment
    /// </summary>
    Task<IEnumerable<MedicalRecordDetail>> GetAppointmentMedicalRecordsAsync(Guid appointmentId, CancellationToken cancellationToken = default);

    /// <summary>
    /// Get latest medical record for patient
    /// </summary>
    Task<MedicalRecordDetail?> GetLatestMedicalRecordAsync(Guid patientId, CancellationToken cancellationToken = default);

    /// <summary>
    /// Create new medical record
    /// </summary>
    Task<MedicalRecordDetail> CreateMedicalRecordAsync(CreateMedicalRecordRequest request, CancellationToken cancellationToken = default);

    /// <summary>
    /// Update medical record
    /// </summary>
    Task<MedicalRecordDetail> UpdateMedicalRecordAsync(Guid recordId, UpdateMedicalRecordRequest request, CancellationToken cancellationToken = default);

    /// <summary>
    /// Get medical records by date range
    /// </summary>
    Task<IEnumerable<MedicalRecordDetail>> GetMedicalRecordsByDateRangeAsync(DateTime startDate, DateTime endDate, CancellationToken cancellationToken = default);
}
