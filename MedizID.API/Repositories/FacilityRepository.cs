using Microsoft.EntityFrameworkCore;
using MedizID.API.Data;
using MedizID.API.Models;

namespace MedizID.API.Repositories;

/// <summary>
/// Facility repository with specialized query methods
/// </summary>
public class FacilityRepository : BaseRepository<Facility>, IFacilityRepository
{
    public FacilityRepository(MedizIDDbContext context) : base(context)
    {
    }

    public async Task<Facility?> GetByNameAsync(string name, CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .FirstOrDefaultAsync(f => f.Name.ToLower() == name.ToLower(), cancellationToken);
    }

    public async Task<Facility?> GetDetailedAsync(Guid facilityId, CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .Include(f => f.Users)
            .Include(f => f.Departments)
            .Include(f => f.Appointments)
            .FirstOrDefaultAsync(f => f.Id == facilityId, cancellationToken);
    }

    public async Task<IEnumerable<Facility>> GetByTypeAsync(int facilityType, CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .Where(f => (int)f.Type == facilityType && f.IsActive)
            .OrderBy(f => f.Name)
            .ToListAsync(cancellationToken);
    }

    public async Task<IEnumerable<Facility>> GetByCityAsync(string city, CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .Where(f => f.City.ToLower() == city.ToLower() && f.IsActive)
            .OrderBy(f => f.Name)
            .ToListAsync(cancellationToken);
    }

    public async Task<Facility?> GetWithDepartmentsAsync(Guid facilityId, CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .Include(f => f.Departments)
            .FirstOrDefaultAsync(f => f.Id == facilityId, cancellationToken);
    }

    public async Task<Facility?> GetWithStaffAsync(Guid facilityId, CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .Include(f => f.Users)
            .FirstOrDefaultAsync(f => f.Id == facilityId, cancellationToken);
    }
}
