using System.Text.Json;
using Blazored.LocalStorage;
using MedizID.UI.Services.Generated;
using MedizID.UI.Services.Generated.Models;

namespace MedizID.UI.Services;

/// <summary>
/// Service for retrieving dashboard statistics based on user role and context
/// </summary>
public class DashboardService
{
    private readonly MedizIdApiClient _apiClient;
    private readonly ILocalStorageService _localStorage;

    public DashboardService(MedizIdApiClient apiClient, ILocalStorageService localStorage)
    {
        _apiClient = apiClient;
        _localStorage = localStorage;
    }

    /// <summary>
    /// Gets the current user from local storage
    /// </summary>
    public async Task<LoginResponse?> GetCurrentUserAsync()
    {
        try
        {
            string? userJson = await _localStorage.GetItemAsync<string>("user");
            if (userJson is null)
                return null;

            return JsonSerializer.Deserialize<LoginResponse>(userJson);
        }
        catch
        {
            return null;
        }
    }

    /// <summary>
    /// Gets the complete dashboard summary for the current user
    /// </summary>
    public async Task<DashboardSummaryResponse?> GetDashboardSummaryAsync()
    {
        try
        {
            return await _apiClient.Api.V1.Dashboard.Summary.GetAsync();
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"Error getting dashboard summary: {ex.Message}");
            return null;
        }
    }

    /// <summary>
    /// Gets facility owner statistics
    /// </summary>
    public async Task<FacilityOwnerStatisticsDto?> GetFacilityOwnerStatisticsAsync()
    {
        try
        {
            return await _apiClient.Api.V1.Dashboard.FacilityOwner.GetAsync();
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"Error getting facility owner statistics: {ex.Message}");
            return null;
        }
    }

    /// <summary>
    /// Gets doctor statistics
    /// </summary>
    public async Task<DoctorStatisticsDto?> GetDoctorStatisticsAsync()
    {
        try
        {
            return await _apiClient.Api.V1.Dashboard.Doctor.GetAsync();
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"Error getting doctor statistics: {ex.Message}");
            return null;
        }
    }

    /// <summary>
    /// Gets midwife statistics
    /// </summary>
    public async Task<MidwifeStatisticsDto?> GetMidwifeStatisticsAsync()
    {
        try
        {
            return await _apiClient.Api.V1.Dashboard.Midwife.GetAsync();
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"Error getting midwife statistics: {ex.Message}");
            return null;
        }
    }

    /// <summary>
    /// Gets nurse statistics
    /// </summary>
    public async Task<NurseStatisticsDto?> GetNurseStatisticsAsync()
    {
        try
        {
            return await _apiClient.Api.V1.Dashboard.Nurse.GetAsync();
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"Error getting nurse statistics: {ex.Message}");
            return null;
        }
    }

    /// <summary>
    /// Gets patient appointment statistics
    /// </summary>
    public async Task<PatientAppointmentStatisticsDto?> GetPatientAppointmentStatisticsAsync()
    {
        try
        {
            return await _apiClient.Api.V1.Dashboard.PatientAppointments.GetAsync();
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"Error getting patient appointment statistics: {ex.Message}");
            return null;
        }
    }

    /// <summary>
    /// Gets appointment trend chart data for the last N days
    /// </summary>
    public async Task<AppointmentTrendChartDto?> GetAppointmentTrendChartAsync(int daysBack = 30)
    {
        try
        {
            return await _apiClient.Api.V1.Dashboard.Charts.AppointmentTrend.GetAsync(requestConfiguration =>
            {
                requestConfiguration.QueryParameters.DaysBack = daysBack;
            });
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"Error getting appointment trend chart: {ex.Message}");
            return null;
        }
    }

    /// <summary>
    /// Gets appointment status distribution chart
    /// </summary>
    public async Task<AppointmentStatusChartDto?> GetAppointmentStatusChartAsync()
    {
        try
        {
            return await _apiClient.Api.V1.Dashboard.Charts.AppointmentStatus.GetAsync();
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"Error getting appointment status chart: {ex.Message}");
            return null;
        }
    }

    /// <summary>
    /// Gets staff distribution chart by role
    /// </summary>
    public async Task<StaffDistributionChartDto?> GetStaffDistributionChartAsync()
    {
        try
        {
            return await _apiClient.Api.V1.Dashboard.Charts.StaffDistribution.GetAsync();
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"Error getting staff distribution chart: {ex.Message}");
            return null;
        }
    }

    /// <summary>
    /// Gets facility patient distribution chart
    /// </summary>
    public async Task<FacilityDistributionChartDto?> GetFacilityDistributionChartAsync()
    {
        try
        {
            return await _apiClient.Api.V1.Dashboard.Charts.FacilityDistribution.GetAsync();
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"Error getting facility distribution chart: {ex.Message}");
            return null;
        }
    }
}
