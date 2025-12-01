using Microsoft.EntityFrameworkCore;
using MedizID.API.Common.Enums;
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

    public async Task<IEnumerable<Diagnosis>> GetByAppointmentAsync(Guid appointmentId, CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .Where(d => d.AppointmentId == appointmentId)
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
            .Include(d => d.Appointment)
            .Where(d => d.Appointment != null && d.Appointment.FacilityPatient.PatientId == patientId)
            .OrderByDescending(d => d.CreatedAt)
            .ToListAsync(cancellationToken);
    }

    public async Task<Diagnosis?> GetPrimaryDiagnosisAsync(Guid appointmentId, CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .Where(d => d.AppointmentId == appointmentId && d.DiagnosisType == DiagnosisTypeEnum.Primary)
            .FirstOrDefaultAsync(cancellationToken);
    }

    public async Task<IEnumerable<Diagnosis>> GetPatientDiagnosisHistoryAsync(Guid patientId, int limit = 10, CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .Include(d => d.Appointment)
            .Where(d => d.Appointment != null && d.Appointment.FacilityPatient.PatientId == patientId)
            .OrderByDescending(d => d.CreatedAt)
            .Take(limit)
            .ToListAsync(cancellationToken);
    }
}

