using MedizID.API.Models;

namespace MedizID.API.Repositories;

/// <summary>
/// Specialized repository for diagnosis operations
/// </summary>
public interface IDiagnosisRepository : IRepository<Diagnosis>
{
    /// <summary>
    /// Get diagnoses by medical record
    /// </summary>
    Task<IEnumerable<Diagnosis>> GetByAppointmentAsync(Guid appointmentId, CancellationToken cancellationToken = default);

    /// <summary>
    /// Get diagnoses by ICD10 code
    /// </summary>
    Task<IEnumerable<Diagnosis>> GetByICD10CodeAsync(Guid icd10CodeId, CancellationToken cancellationToken = default);

    /// <summary>
    /// Get diagnoses by patient
    /// </summary>
    Task<IEnumerable<Diagnosis>> GetByPatientAsync(Guid patientId, CancellationToken cancellationToken = default);

    /// <summary>
    /// Get primary diagnosis for medical record
    /// </summary>
    Task<Diagnosis?> GetPrimaryDiagnosisAsync(Guid appointmentId, CancellationToken cancellationToken = default);

    /// <summary>
    /// Get diagnosis history for patient
    /// </summary>
    Task<IEnumerable<Diagnosis>> GetPatientDiagnosisHistoryAsync(Guid patientId, int limit = 10, CancellationToken cancellationToken = default);
}
