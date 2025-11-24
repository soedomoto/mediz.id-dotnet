using MedizID.API.Models;

namespace MedizID.API.Repositories;

/// <summary>
/// Specialized repository for facility operations
/// </summary>
public interface IFacilityRepository : IRepository<Facility>
{
    /// <summary>
    /// Get facility by name
    /// </summary>
    Task<Facility?> GetByNameAsync(string name, CancellationToken cancellationToken = default);

    /// <summary>
    /// Get facility with all related data (users, departments, appointments)
    /// </summary>
    Task<Facility?> GetDetailedAsync(Guid facilityId, CancellationToken cancellationToken = default);

    /// <summary>
    /// Get facilities by type
    /// </summary>
    Task<IEnumerable<Facility>> GetByTypeAsync(int facilityType, CancellationToken cancellationToken = default);

    /// <summary>
    /// Get facilities by city
    /// </summary>
    Task<IEnumerable<Facility>> GetByCityAsync(string city, CancellationToken cancellationToken = default);

    /// <summary>
    /// Get facility with departments
    /// </summary>
    Task<Facility?> GetWithDepartmentsAsync(Guid facilityId, CancellationToken cancellationToken = default);

    /// <summary>
    /// Get facility with staff
    /// </summary>
    Task<Facility?> GetWithStaffAsync(Guid facilityId, CancellationToken cancellationToken = default);
}
