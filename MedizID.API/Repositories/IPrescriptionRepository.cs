using MedizID.API.Models;

namespace MedizID.API.Repositories;

/// <summary>
/// Specialized repository for prescription operations
/// </summary>
public interface IPrescriptionRepository : IRepository<Prescription>
{
    /// <summary>
    /// Get prescriptions by medical record
    /// </summary>
    Task<IEnumerable<Prescription>> GetByMedicalRecordAsync(Guid medicalRecordId, CancellationToken cancellationToken = default);

    /// <summary>
    /// Get prescriptions by patient
    /// </summary>
    Task<IEnumerable<Prescription>> GetByPatientAsync(Guid patientId, CancellationToken cancellationToken = default);

    /// <summary>
    /// Get active prescriptions for patient
    /// </summary>
    Task<IEnumerable<Prescription>> GetActivePrescriptionsAsync(Guid patientId, CancellationToken cancellationToken = default);

    /// <summary>
    /// Get prescriptions by drug
    /// </summary>
    Task<IEnumerable<Prescription>> GetByDrugAsync(Guid drugId, CancellationToken cancellationToken = default);

    /// <summary>
    /// Get prescription with drug details
    /// </summary>
    Task<Prescription?> GetDetailedAsync(Guid prescriptionId, CancellationToken cancellationToken = default);

    /// <summary>
    /// Get prescription history for patient
    /// </summary>
    Task<IEnumerable<Prescription>> GetPatientPrescriptionHistoryAsync(Guid patientId, int limit = 20, CancellationToken cancellationToken = default);
}
