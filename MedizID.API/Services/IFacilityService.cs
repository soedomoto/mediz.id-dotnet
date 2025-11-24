using MedizID.API.DTOs;

namespace MedizID.API.Services;

/// <summary>
/// Service interface for facility operations
/// </summary>
public interface IFacilityService
{
    /// <summary>
    /// Get facility by ID
    /// </summary>
    Task<FacilityResponse?> GetFacilityAsync(Guid facilityId, CancellationToken cancellationToken = default);

    /// <summary>
    /// Get all active facilities
    /// </summary>
    Task<IEnumerable<FacilityResponse>> GetFacilitiesAsync(CancellationToken cancellationToken = default);

    /// <summary>
    /// Get facilities by type
    /// </summary>
    Task<IEnumerable<FacilityResponse>> GetFacilitiesByTypeAsync(int facilityType, CancellationToken cancellationToken = default);

    /// <summary>
    /// Get facilities by city
    /// </summary>
    Task<IEnumerable<FacilityResponse>> GetFacilitiesByCityAsync(string city, CancellationToken cancellationToken = default);

    /// <summary>
    /// Create new facility
    /// </summary>
    Task<FacilityResponse> CreateFacilityAsync(CreateFacilityRequest request, CancellationToken cancellationToken = default);

    /// <summary>
    /// Update facility
    /// </summary>
    Task<FacilityResponse> UpdateFacilityAsync(Guid facilityId, UpdateFacilityRequest request, CancellationToken cancellationToken = default);

    /// <summary>
    /// Get facility departments
    /// </summary>
    Task<IEnumerable<DepartmentResponse>> GetFacilityDepartmentsAsync(Guid facilityId, CancellationToken cancellationToken = default);

    /// <summary>
    /// Get facility staff
    /// </summary>
    Task<IEnumerable<FacilityStaffResponse>> GetFacilityStaffAsync(Guid facilityId, CancellationToken cancellationToken = default);
}
