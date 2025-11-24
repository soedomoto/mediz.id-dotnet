using Microsoft.EntityFrameworkCore;
using MedizID.API.Data;
using MedizID.API.Models;

namespace MedizID.API.Repositories;

public class AdolescentHealthRepository : BaseRepository<AdolescentHealth>, IAdolescentHealthRepository
{
    public AdolescentHealthRepository(MedizIDDbContext context) : base(context)
    {
    }

    public async Task<AdolescentHealth?> GetByMedicalRecordAsync(Guid medicalRecordId, CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .FirstOrDefaultAsync(a => a.MedicalRecordId == medicalRecordId, cancellationToken);
    }

    public async Task<IEnumerable<AdolescentHealth>> GetByPatientAsync(Guid patientId, CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .Where(a => a.MedicalRecord.PatientId == patientId)
            .ToListAsync(cancellationToken);
    }
}
