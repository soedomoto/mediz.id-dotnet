using Microsoft.EntityFrameworkCore;
using MedizID.API.Data;
using MedizID.API.Models;

namespace MedizID.API.Repositories;

/// <summary>
/// Anamnesis repository with specialized query methods
/// </summary>
public class AnamnesisRepository : BaseRepository<Anamnesis>, IAnamnesisRepository
{
    public AnamnesisRepository(MedizIDDbContext context) : base(context)
    {
    }

    public async Task<Anamnesis?> GetByMedicalRecordAsync(Guid medicalRecordId, CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .FirstOrDefaultAsync(a => a.MedicalRecordId == medicalRecordId, cancellationToken);
    }

    public async Task<IEnumerable<Anamnesis>> GetByPatientAsync(Guid patientId, CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .Where(a => a.MedicalRecord != null && a.MedicalRecord.PatientId == patientId)
            .OrderByDescending(a => a.CreatedAt)
            .ToListAsync(cancellationToken);
    }

    public async Task<Anamnesis?> GetLatestForPatientAsync(Guid patientId, CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .Where(a => a.MedicalRecord != null && a.MedicalRecord.PatientId == patientId)
            .OrderByDescending(a => a.CreatedAt)
            .FirstOrDefaultAsync(cancellationToken);
    }

    public async Task<IEnumerable<Anamnesis>> GetByDateRangeAsync(DateTime startDate, DateTime endDate, CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .Where(a => a.CreatedAt >= startDate && a.CreatedAt <= endDate)
            .OrderByDescending(a => a.CreatedAt)
            .ToListAsync(cancellationToken);
    }
}
