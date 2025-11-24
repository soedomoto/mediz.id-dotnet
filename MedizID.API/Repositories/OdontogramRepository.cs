using Microsoft.EntityFrameworkCore;
using MedizID.API.Data;
using MedizID.API.Models;

namespace MedizID.API.Repositories;

public class OdontogramRepository : BaseRepository<Odontogram>, IOdontogramRepository
{
    public OdontogramRepository(MedizIDDbContext context) : base(context)
    {
    }

    public async Task<Odontogram?> GetByMedicalRecordAsync(Guid medicalRecordId, CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .FirstOrDefaultAsync(o => o.MedicalRecordId == medicalRecordId, cancellationToken);
    }

    public async Task<IEnumerable<Odontogram>> GetByAppointmentAsync(Guid appointmentId, CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .Where(o => o.MedicalRecord.AppointmentId == appointmentId)
            .ToListAsync(cancellationToken);
    }

    public async Task<IEnumerable<Odontogram>> GetByToothNumberAsync(string toothNumber, CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .Where(o => o.ToothNumber == toothNumber)
            .ToListAsync(cancellationToken);
    }
}
