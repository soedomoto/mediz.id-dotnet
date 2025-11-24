using MedizID.API.Models;

namespace MedizID.API.Repositories;

/// <summary>
/// Specialized repository for appointment operations
/// </summary>
public interface IAppointmentRepository : IRepository<Appointment>
{
    /// <summary>
    /// Get appointments by patient
    /// </summary>
    Task<IEnumerable<Appointment>> GetByPatientAsync(Guid patientId, CancellationToken cancellationToken = default);

    /// <summary>
    /// Get appointments by facility
    /// </summary>
    Task<IEnumerable<Appointment>> GetByFacilityAsync(Guid facilityId, CancellationToken cancellationToken = default);

    /// <summary>
    /// Get appointments in date range
    /// </summary>
    Task<IEnumerable<Appointment>> GetByDateRangeAsync(DateTime startDate, DateTime endDate, CancellationToken cancellationToken = default);

    /// <summary>
    /// Get appointments by status
    /// </summary>
    Task<IEnumerable<Appointment>> GetByStatusAsync(string status, CancellationToken cancellationToken = default);

    /// <summary>
    /// Get upcoming appointments
    /// </summary>
    Task<IEnumerable<Appointment>> GetUpcomingAsync(int daysAhead = 7, CancellationToken cancellationToken = default);

    /// <summary>
    /// Get appointment with related data (patient, medical record)
    /// </summary>
    Task<Appointment?> GetDetailedAsync(Guid appointmentId, CancellationToken cancellationToken = default);
}
