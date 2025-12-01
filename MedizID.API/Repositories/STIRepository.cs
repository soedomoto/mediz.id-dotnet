using Microsoft.EntityFrameworkCore;
using MedizID.API.Data;
using MedizID.API.Models;

namespace MedizID.API.Repositories;

public class STIRepository : BaseRepository<STI>, ISTIRepository
{
    public STIRepository(MedizIDDbContext context) : base(context)
    {
    }

    public async Task<STI?> GetByAppointmentAsync(Guid appointmentId, CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .FirstOrDefaultAsync(s => s.AppointmentId == appointmentId, cancellationToken);
    }

    public async Task<IEnumerable<STI>> GetByPatientAsync(Guid patientId, CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .Include(s => s.Appointment)
            .Where(s => s.Appointment != null && s.Appointment.FacilityPatient.PatientId == patientId)
            .ToListAsync(cancellationToken);
    }
}
