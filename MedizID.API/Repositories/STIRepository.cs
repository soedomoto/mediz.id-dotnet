using Microsoft.EntityFrameworkCore;
using MedizID.API.Data;
using MedizID.API.Models;

namespace MedizID.API.Repositories;

public class STIRepository : BaseRepository<STI>, ISTIRepository
{
    public STIRepository(MedizIDDbContext context) : base(context)
    {
    }

    public async Task<STI?> GetByMedicalRecordAsync(Guid medicalRecordId, CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .FirstOrDefaultAsync(s => s.MedicalRecordId == medicalRecordId, cancellationToken);
    }

    public async Task<IEnumerable<STI>> GetByPatientAsync(Guid patientId, CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .Where(s => s.MedicalRecord.PatientId == patientId)
            .ToListAsync(cancellationToken);
    }

    public async Task<IEnumerable<STI>> GetByAppointmentAsync(Guid appointmentId, CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .Where(s => s.MedicalRecord.AppointmentId == appointmentId)
            .ToListAsync(cancellationToken);
    }
}
