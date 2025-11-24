using MedizID.API.Models;

namespace MedizID.API.Repositories;

public interface ISTIRepository : IRepository<STI>
{
    Task<STI?> GetByMedicalRecordAsync(Guid medicalRecordId, CancellationToken cancellationToken = default);
    Task<IEnumerable<STI>> GetByPatientAsync(Guid patientId, CancellationToken cancellationToken = default);
    Task<IEnumerable<STI>> GetByAppointmentAsync(Guid appointmentId, CancellationToken cancellationToken = default);
}
