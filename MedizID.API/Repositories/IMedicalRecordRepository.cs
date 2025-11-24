using MedizID.API.Models;

namespace MedizID.API.Repositories;

/// <summary>
/// Specialized repository for medical record operations
/// </summary>
public interface IMedicalRecordRepository : IRepository<MedicalRecord>
{
    /// <summary>
    /// Get medical records by patient
    /// </summary>
    Task<IEnumerable<MedicalRecord>> GetByPatientAsync(Guid patientId, CancellationToken cancellationToken = default);

    /// <summary>
    /// Get medical records by appointment
    /// </summary>
    Task<IEnumerable<MedicalRecord>> GetByAppointmentAsync(Guid appointmentId, CancellationToken cancellationToken = default);

    /// <summary>
    /// Get medical record with diagnoses and prescriptions
    /// </summary>
    Task<MedicalRecord?> GetDetailedAsync(Guid recordId, CancellationToken cancellationToken = default);

    /// <summary>
    /// Get latest medical record for patient
    /// </summary>
    Task<MedicalRecord?> GetLatestForPatientAsync(Guid patientId, CancellationToken cancellationToken = default);

    /// <summary>
    /// Get medical records created between dates
    /// </summary>
    Task<IEnumerable<MedicalRecord>> GetByDateRangeAsync(DateTime startDate, DateTime endDate, CancellationToken cancellationToken = default);

    /// <summary>
    /// Get medical records by record type
    /// </summary>
    Task<IEnumerable<MedicalRecord>> GetByTypeAsync(string recordType, CancellationToken cancellationToken = default);
}
