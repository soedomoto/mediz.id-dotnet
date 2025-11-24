using MedizID.API.DTOs;

namespace MedizID.API.Services;

/// <summary>
/// Service interface for patient operations
/// </summary>
public interface IPatientService
{
    /// <summary>
    /// Get patient by ID with basic information
    /// </summary>
    Task<PatientDetail?> GetPatientAsync(Guid patientId, CancellationToken cancellationToken = default);

    /// <summary>
    /// Get all patients for a facility
    /// </summary>
    Task<IEnumerable<PatientDetail>> GetPatientsAsync(Guid facilityId, CancellationToken cancellationToken = default);

    /// <summary>
    /// Get patient with complete medical history
    /// </summary>
    Task<PatientDetail?> GetPatientWithHistoryAsync(Guid patientId, CancellationToken cancellationToken = default);

    /// <summary>
    /// Search patients by name
    /// </summary>
    Task<IEnumerable<PatientDetail>> SearchPatientsByNameAsync(string firstName, string lastName, CancellationToken cancellationToken = default);

    /// <summary>
    /// Get patient by medical record number
    /// </summary>
    Task<PatientDetail?> GetPatientByMRNAsync(string mrn, CancellationToken cancellationToken = default);

    /// <summary>
    /// Create new patient
    /// </summary>
    Task<PatientDetail> CreatePatientAsync(CreatePatientRequest request, CancellationToken cancellationToken = default);

    /// <summary>
    /// Update patient information
    /// </summary>
    Task<PatientDetail> UpdatePatientAsync(Guid patientId, UpdatePatientRequest request, CancellationToken cancellationToken = default);

    /// <summary>
    /// Get patient allergies
    /// </summary>
    Task<PatientAllergiesResponse?> GetPatientAllergiesAsync(Guid patientId, CancellationToken cancellationToken = default);

    /// <summary>
    /// Get patient appointments
    /// </summary>
    Task<IEnumerable<AppointmentResponse>> GetPatientAppointmentsAsync(Guid patientId, CancellationToken cancellationToken = default);
}
