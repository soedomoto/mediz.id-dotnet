using Microsoft.EntityFrameworkCore;
using MedizID.API.Data;
using MedizID.API.Models;

namespace MedizID.API.Repositories;

/// <summary>
/// Medical record repository with specialized query methods
/// </summary>
public class MedicalRecordRepository : BaseRepository<MedicalRecord>, IMedicalRecordRepository
{
    public MedicalRecordRepository(MedizIDDbContext context) : base(context)
    {
    }

    public async Task<IEnumerable<MedicalRecord>> GetByPatientAsync(Guid patientId, CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .Where(mr => mr.PatientId == patientId)
            .OrderByDescending(mr => mr.CreatedAt)
            .ToListAsync(cancellationToken);
    }

    public async Task<IEnumerable<MedicalRecord>> GetByAppointmentAsync(Guid appointmentId, CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .Where(mr => mr.AppointmentId == appointmentId)
            .OrderByDescending(mr => mr.CreatedAt)
            .ToListAsync(cancellationToken);
    }

    public async Task<MedicalRecord?> GetDetailedAsync(Guid recordId, CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .Include(mr => mr.Diagnoses)
            .Include(mr => mr.Prescriptions)
            .Include(mr => mr.Anamnesis)
            .Include(mr => mr.Patient)
            .Include(mr => mr.Appointment)
            .FirstOrDefaultAsync(mr => mr.Id == recordId, cancellationToken);
    }

    public async Task<MedicalRecord?> GetLatestForPatientAsync(Guid patientId, CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .Where(mr => mr.PatientId == patientId)
            .OrderByDescending(mr => mr.CreatedAt)
            .FirstOrDefaultAsync(cancellationToken);
    }

    public async Task<IEnumerable<MedicalRecord>> GetByDateRangeAsync(DateTime startDate, DateTime endDate, CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .Where(mr => mr.CreatedAt >= startDate && mr.CreatedAt <= endDate)
            .OrderByDescending(mr => mr.CreatedAt)
            .ToListAsync(cancellationToken);
    }

    public async Task<IEnumerable<MedicalRecord>> GetByTypeAsync(string recordType, CancellationToken cancellationToken = default)
    {
        // MedicalRecord doesn't have a RecordType field, filter by treatment instead
        return await _dbSet
            .Where(mr => mr.Treatment != null && mr.Treatment.Contains(recordType))
            .OrderByDescending(mr => mr.CreatedAt)
            .ToListAsync(cancellationToken);
    }
}
