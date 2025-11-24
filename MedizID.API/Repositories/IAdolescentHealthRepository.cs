using MedizID.API.Models;

namespace MedizID.API.Repositories;

public interface IAdolescentHealthRepository : IRepository<AdolescentHealth>
{
    Task<AdolescentHealth?> GetByMedicalRecordAsync(Guid medicalRecordId, CancellationToken cancellationToken = default);
    Task<IEnumerable<AdolescentHealth>> GetByPatientAsync(Guid patientId, CancellationToken cancellationToken = default);
}
