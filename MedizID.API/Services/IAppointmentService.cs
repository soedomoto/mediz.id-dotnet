using MedizID.API.DTOs;

namespace MedizID.API.Services;

/// <summary>
/// Service interface for appointment operations
/// </summary>
public interface IAppointmentService
{
    /// <summary>
    /// Get appointment by ID
    /// </summary>
    Task<AppointmentResponse?> GetAppointmentAsync(Guid appointmentId, CancellationToken cancellationToken = default);

    /// <summary>
    /// Get appointments for a patient
    /// </summary>
    Task<IEnumerable<AppointmentResponse>> GetPatientAppointmentsAsync(Guid patientId, CancellationToken cancellationToken = default);

    /// <summary>
    /// Get appointments for a facility
    /// </summary>
    Task<IEnumerable<AppointmentResponse>> GetFacilityAppointmentsAsync(Guid facilityId, CancellationToken cancellationToken = default);

    /// <summary>
    /// Get upcoming appointments
    /// </summary>
    Task<IEnumerable<AppointmentResponse>> GetUpcomingAppointmentsAsync(Guid facilityId, int daysAhead = 7, CancellationToken cancellationToken = default);

    /// <summary>
    /// Create new appointment
    /// </summary>
    Task<AppointmentResponse> CreateAppointmentAsync(CreateAppointmentRequest request, CancellationToken cancellationToken = default);

    /// <summary>
    /// Update appointment
    /// </summary>
    Task<AppointmentResponse> UpdateAppointmentAsync(Guid appointmentId, UpdateAppointmentRequest request, CancellationToken cancellationToken = default);

    /// <summary>
    /// Cancel appointment
    /// </summary>
    Task<bool> CancelAppointmentAsync(Guid appointmentId, string reason, CancellationToken cancellationToken = default);

    /// <summary>
    /// Complete appointment
    /// </summary>
    Task<bool> CompleteAppointmentAsync(Guid appointmentId, CancellationToken cancellationToken = default);
}
