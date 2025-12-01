using MedizID.API.Models;

namespace MedizID.API.Repositories;

public interface IOdontogramRepository : IRepository<Odontogram>
{
    Task<Odontogram?> GetByAppointmentAsync(Guid appointmentId, CancellationToken cancellationToken = default);
    Task<IEnumerable<Odontogram>> GetByToothNumberAsync(string toothNumber, CancellationToken cancellationToken = default);
}
