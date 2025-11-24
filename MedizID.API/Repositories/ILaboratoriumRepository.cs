using MedizID.API.Models;

namespace MedizID.API.Repositories;

public interface ILaboratoriumRepository : IRepository<Laboratorium>
{
    Task<IEnumerable<Laboratorium>> GetByMedicalRecordAsync(Guid medicalRecordId, CancellationToken cancellationToken = default);
    Task<IEnumerable<Laboratorium>> GetByAppointmentAsync(Guid appointmentId, CancellationToken cancellationToken = default);
    Task<IEnumerable<Laboratorium>> GetByPatientAsync(Guid patientId, CancellationToken cancellationToken = default);
}
