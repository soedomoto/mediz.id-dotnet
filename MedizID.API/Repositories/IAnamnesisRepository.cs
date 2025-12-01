using MedizID.API.Models;

namespace MedizID.API.Repositories;

/// <summary>
/// Specialized repository for anamnesis operations
/// </summary>
public interface IAnamnesisRepository : IRepository<Anamnesis>
{
    /// <summary>
    /// Get anamnesis by medical record
    /// </summary>
    Task<Anamnesis?> GetByAppointmentAsync(Guid appointmentId, CancellationToken cancellationToken = default);

    /// <summary>
    /// Get anamnesis by patient
    /// </summary>
    Task<IEnumerable<Anamnesis>> GetByPatientAsync(Guid patientId, CancellationToken cancellationToken = default);

    /// <summary>
    /// Get latest anamnesis for patient
    /// </summary>
    Task<Anamnesis?> GetLatestForPatientAsync(Guid patientId, CancellationToken cancellationToken = default);

    /// <summary>
    /// Get anamnesises created between dates
    /// </summary>
    Task<IEnumerable<Anamnesis>> GetByDateRangeAsync(DateTime startDate, DateTime endDate, CancellationToken cancellationToken = default);
}
