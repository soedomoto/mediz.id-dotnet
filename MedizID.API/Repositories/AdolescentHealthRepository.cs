using Microsoft.EntityFrameworkCore;
using MedizID.API.Data;
using MedizID.API.Models;

namespace MedizID.API.Repositories;

public class AdolescentHealthRepository : BaseRepository<AdolescentHealth>, IAdolescentHealthRepository
{
    public AdolescentHealthRepository(MedizIDDbContext context) : base(context)
    {
    }

    public async Task<AdolescentHealth?> GetByAppointmentAsync(Guid appointmentId, CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .FirstOrDefaultAsync(a => a.AppointmentId == appointmentId, cancellationToken);
    }

    public async Task<IEnumerable<AdolescentHealth>> GetByPatientAsync(Guid patientId, CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .Include(a => a.Appointment)
            .Where(a => a.Appointment != null && a.Appointment.FacilityPatient.PatientId == patientId)
            .ToListAsync(cancellationToken);
    }
}
