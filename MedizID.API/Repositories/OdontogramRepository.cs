using Microsoft.EntityFrameworkCore;
using MedizID.API.Data;
using MedizID.API.Models;

namespace MedizID.API.Repositories;

public class OdontogramRepository : BaseRepository<Odontogram>, IOdontogramRepository
{
    public OdontogramRepository(MedizIDDbContext context) : base(context)
    {
    }

    public async Task<Odontogram?> GetByAppointmentAsync(Guid appointmentId, CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .Include(o => o.Surfaces)
            .FirstOrDefaultAsync(o => o.AppointmentId == appointmentId, cancellationToken);
    }

    public async Task<IEnumerable<Odontogram>> GetByToothNumberAsync(string toothNumber, CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .Include(o => o.Surfaces)
            .Where(o => o.Surfaces.Any(s => s.ToothNumber.ToString() == toothNumber))
            .ToListAsync(cancellationToken);
    }
}
