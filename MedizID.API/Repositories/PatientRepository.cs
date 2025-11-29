using Microsoft.EntityFrameworkCore;
using MedizID.API.Data;
using MedizID.API.Models;

namespace MedizID.API.Repositories;

/// <summary>
/// Patient repository with specialized query methods
/// </summary>
public class PatientRepository : BaseRepository<Patient>, IPatientRepository
{
    public PatientRepository(MedizIDDbContext context) : base(context)
    {
    }

    public async Task<Patient?> GetByMedicalRecordNumberAsync(string mrn, CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .FirstOrDefaultAsync(p => p.MedicalRecordNumber == mrn, cancellationToken);
    }

    public async Task<IEnumerable<Patient>> GetByFacilityAsync(Guid facilityId, CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .Where(p => p.FacilityId == facilityId)
            .ToListAsync(cancellationToken);
    }

    public async Task<IEnumerable<Patient>> GetByPhoneAsync(string phone, CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .Where(p => p.PhoneNumber == phone)
            .ToListAsync(cancellationToken);
    }

    public async Task<IEnumerable<Patient>> SearchByNameAsync(string firstName, string lastName, CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .Where(p => p.FirstName.Contains(firstName) && p.LastName.Contains(lastName))
            .ToListAsync(cancellationToken);
    }

    public async Task<Patient?> GetWithMedicalRecordsAsync(Guid patientId, CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .FirstOrDefaultAsync(p => p.Id == patientId, cancellationToken);
    }

    public async Task<Patient?> GetWithAppointmentsAsync(Guid patientId, CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .FirstOrDefaultAsync(p => p.Id == patientId, cancellationToken);
    }
}
