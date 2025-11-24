using MedizID.API.Models;

namespace MedizID.API.Repositories;

public interface IOdontogramRepository : IRepository<Odontogram>
{
    Task<Odontogram?> GetByMedicalRecordAsync(Guid medicalRecordId, CancellationToken cancellationToken = default);
    Task<IEnumerable<Odontogram>> GetByAppointmentAsync(Guid appointmentId, CancellationToken cancellationToken = default);
    Task<IEnumerable<Odontogram>> GetByToothNumberAsync(string toothNumber, CancellationToken cancellationToken = default);
}
