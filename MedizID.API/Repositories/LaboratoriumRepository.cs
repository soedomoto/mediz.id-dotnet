using Microsoft.EntityFrameworkCore;
using MedizID.API.Data;
using MedizID.API.Models;

namespace MedizID.API.Repositories;

public class LaboratoriumRepository : BaseRepository<Laboratorium>, ILaboratoriumRepository
{
    public LaboratoriumRepository(MedizIDDbContext context) : base(context)
    {
    }

    public async Task<IEnumerable<Laboratorium>> GetByMedicalRecordAsync(Guid medicalRecordId, CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .Where(l => l.MedicalRecordId == medicalRecordId)
            .ToListAsync(cancellationToken);
    }

    public async Task<IEnumerable<Laboratorium>> GetByAppointmentAsync(Guid appointmentId, CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .Where(l => l.MedicalRecord.AppointmentId == appointmentId)
            .ToListAsync(cancellationToken);
    }

    public async Task<IEnumerable<Laboratorium>> GetByPatientAsync(Guid patientId, CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .Where(l => l.MedicalRecord.PatientId == patientId)
            .ToListAsync(cancellationToken);
    }
}
