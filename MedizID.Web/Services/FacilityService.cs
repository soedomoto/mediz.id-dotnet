using MedizID.Web.Models;
using System.Net.Http.Json;

namespace MedizID.Web.Services;

/// <summary>Service interface for facility management operations</summary>
public interface IFacilityService
{
    Task<FacilityListResponse> GetFacilitiesAsync(int skip, int limit);
    Task<FacilityResponse> GetFacilityAsync(int id);
    Task<FacilityResponse> CreateFacilityAsync(CreateFacilityRequest request);
    Task<FacilityResponse> UpdateFacilityAsync(int id, UpdateFacilityRequest request);
    Task DeleteFacilityAsync(int id);
}

/// <summary>Implementation of facility service (replace with actual HTTP client)</summary>
public class FacilityService : IFacilityService
{
    private readonly HttpClient _httpClient;

    public FacilityService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<FacilityListResponse> GetFacilitiesAsync(int skip, int limit)
    {
        return await _httpClient.GetFromJsonAsync<FacilityListResponse>($"/api/v1/facilities?skip={skip}&limit={limit}")
            ?? new FacilityListResponse { Facilities = new(), Total = 0 };
    }

    public async Task<FacilityResponse> GetFacilityAsync(int id)
    {
        var response = await _httpClient.GetFromJsonAsync<FacilityResponse>($"/api/v1/facilities/{id}");
        return response ?? throw new InvalidOperationException("Facility not found");
    }

    public async Task<FacilityResponse> CreateFacilityAsync(CreateFacilityRequest request)
    {
        var response = await _httpClient.PostAsJsonAsync("/api/v1/facilities", request);
        response.EnsureSuccessStatusCode();
        var result = await response.Content.ReadFromJsonAsync<FacilityResponse>();
        return result ?? throw new InvalidOperationException("Failed to create facility");
    }

    public async Task<FacilityResponse> UpdateFacilityAsync(int id, UpdateFacilityRequest request)
    {
        var response = await _httpClient.PutAsJsonAsync($"/api/v1/facilities/{id}", request);
        response.EnsureSuccessStatusCode();
        var result = await response.Content.ReadFromJsonAsync<FacilityResponse>();
        return result ?? throw new InvalidOperationException("Failed to update facility");
    }

    public async Task DeleteFacilityAsync(int id)
    {
        var response = await _httpClient.DeleteAsync($"/api/v1/facilities/{id}");
        response.EnsureSuccessStatusCode();
    }
}
