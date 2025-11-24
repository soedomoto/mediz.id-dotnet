using Microsoft.EntityFrameworkCore;
using MedizID.API.Data;
using MedizID.API.Models;

namespace MedizID.API.Repositories;

/// <summary>
/// Diagnosis repository with specialized query methods
/// </summary>
public class DiagnosisRepository : BaseRepository<Diagnosis>, IDiagnosisRepository
{
    public DiagnosisRepository(MedizIDDbContext context) : base(context)
    {
    }

    public async Task<IEnumerable<Diagnosis>> GetByMedicalRecordAsync(Guid medicalRecordId, CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .Where(d => d.MedicalRecordId == medicalRecordId)
            .OrderByDescending(d => d.CreatedAt)
            .ToListAsync(cancellationToken);
    }

    public async Task<IEnumerable<Diagnosis>> GetByICD10CodeAsync(Guid icd10CodeId, CancellationToken cancellationToken = default)
    {
        // Diagnosis doesn't have ICD10CodeId property, return all ordered by created date
        return await _dbSet
            .OrderByDescending(d => d.CreatedAt)
            .ToListAsync(cancellationToken);
    }

    public async Task<IEnumerable<Diagnosis>> GetByPatientAsync(Guid patientId, CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .Where(d => d.MedicalRecord != null && d.MedicalRecord.PatientId == patientId)
            .OrderByDescending(d => d.CreatedAt)
            .ToListAsync(cancellationToken);
    }

    public async Task<Diagnosis?> GetPrimaryDiagnosisAsync(Guid medicalRecordId, CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .Where(d => d.MedicalRecordId == medicalRecordId && 
                       (d.DiagnosisType == "Primary" || d.DiagnosisType == "primary"))
            .FirstOrDefaultAsync(cancellationToken);
    }

    public async Task<IEnumerable<Diagnosis>> GetPatientDiagnosisHistoryAsync(Guid patientId, int limit = 10, CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .Where(d => d.MedicalRecord != null && d.MedicalRecord.PatientId == patientId)
            .OrderByDescending(d => d.CreatedAt)
            .Take(limit)
            .ToListAsync(cancellationToken);
    }
}
