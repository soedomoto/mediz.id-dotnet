using MedizID.API.Models;

namespace MedizID.API.Repositories;

/// <summary>
/// Specialized repository for patient operations
/// </summary>
public interface IPatientRepository : IRepository<Patient>
{
    /// <summary>
    /// Get patient by medical record number
    /// </summary>
    Task<Patient?> GetByMedicalRecordNumberAsync(string mrn, CancellationToken cancellationToken = default);

    /// <summary>
    /// Get patients by facility
    /// </summary>
    Task<IEnumerable<Patient>> GetByFacilityAsync(Guid facilityId, CancellationToken cancellationToken = default);

    /// <summary>
    /// Get patients by phone number
    /// </summary>
    Task<IEnumerable<Patient>> GetByPhoneAsync(string phone, CancellationToken cancellationToken = default);

    /// <summary>
    /// Search patients by name
    /// </summary>
    Task<IEnumerable<Patient>> SearchByNameAsync(string firstName, string lastName, CancellationToken cancellationToken = default);

    /// <summary>
    /// Get patient with related medical records
    /// </summary>
    Task<Patient?> GetWithMedicalRecordsAsync(Guid patientId, CancellationToken cancellationToken = default);

    /// <summary>
    /// Get patient with appointments
    /// </summary>
    Task<Patient?> GetWithAppointmentsAsync(Guid patientId, CancellationToken cancellationToken = default);
}
