using MedizID.API.Models;

namespace MedizID.API.Repositories;

public interface IAdolescentHealthRepository : IRepository<AdolescentHealth>
{
    Task<AdolescentHealth?> GetByAppointmentAsync(Guid appointmentId, CancellationToken cancellationToken = default);
    Task<IEnumerable<AdolescentHealth>> GetByPatientAsync(Guid patientId, CancellationToken cancellationToken = default);
}
