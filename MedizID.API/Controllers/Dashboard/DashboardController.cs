using MedizID.API.DTOs;
using MedizID.API.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace MedizID.API.Controllers.Dashboard;

/// <summary>
/// Dashboard controller providing user-specific statistics and analytics
/// </summary>
[ApiController]
[Route("api/v1/[controller]")]
[Authorize]
[Produces("application/json")]
public class DashboardController : ControllerBase
{
    private readonly IDashboardService _dashboardService;
    private readonly ILogger<DashboardController> _logger;

    public DashboardController(IDashboardService dashboardService, ILogger<DashboardController> logger)
    {
        _dashboardService = dashboardService;
        _logger = logger;
    }

    /// <summary>
    /// Get complete dashboard summary for the current user
    /// </summary>
    /// <remarks>
    /// Returns user-specific statistics based on their role:
    /// - FacilityOwner/SuperAdmin: Managed facilities stats
    /// - Doctor/Midwife/Nurse: Staffed facilities stats
    /// - Patient: Appointment statistics
    /// </remarks>
    [HttpGet("summary", Name = "GetDashboardSummary")]
    [ProducesResponseType(typeof(DashboardSummaryResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetDashboardSummary(CancellationToken cancellationToken = default)
    {
        try
        {
            var userId = GetCurrentUserId();
            var summary = await _dashboardService.GetUserDashboardSummaryAsync(userId, cancellationToken);
            return Ok(summary);
        }
        catch (InvalidOperationException ex)
        {
            _logger.LogWarning($"Dashboard summary request failed: {ex.Message}");
            return NotFound(new ErrorResponse { ErrorCode = "NOT_FOUND", Message = ex.Message });
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error retrieving dashboard summary: {ex.Message}");
            return StatusCode(500, new ErrorResponse { ErrorCode = "INTERNAL_ERROR", Message = "An error occurred while retrieving dashboard summary" });
        }
    }

    /// <summary>
    /// Get facility owner/superuser statistics
    /// </summary>
    /// <remarks>
    /// Returns:
    /// - Number of managed facilities
    /// - Total patients across facilities
    /// - Total staff count
    /// - Today's and pending appointments
    /// - Detailed statistics per facility
    /// </remarks>
    [HttpGet("facility-owner", Name = "GetFacilityOwnerStatistics")]
    [ProducesResponseType(typeof(FacilityOwnerStatisticsDto), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> GetFacilityOwnerStatistics(CancellationToken cancellationToken = default)
    {
        try
        {
            var userId = GetCurrentUserId();
            var stats = await _dashboardService.GetFacilityOwnerStatisticsAsync(userId, cancellationToken);
            return Ok(stats);
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error retrieving facility owner statistics: {ex.Message}");
            return StatusCode(500, new ErrorResponse { ErrorCode = "INTERNAL_ERROR", Message = "An error occurred while retrieving statistics" });
        }
    }

    /// <summary>
    /// Get doctor statistics
    /// </summary>
    /// <remarks>
    /// Returns:
    /// - Number of staffed facilities
    /// - Today's appointments
    /// - Completed appointments
    /// - Patients seen
    /// - Statistics per facility
    /// </remarks>
    [HttpGet("doctor", Name = "GetDoctorStatistics")]
    [ProducesResponseType(typeof(DoctorStatisticsDto), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> GetDoctorStatistics(CancellationToken cancellationToken = default)
    {
        try
        {
            var userId = GetCurrentUserId();
            var stats = await _dashboardService.GetDoctorStatisticsAsync(userId, cancellationToken);
            return Ok(stats);
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error retrieving doctor statistics: {ex.Message}");
            return StatusCode(500, new ErrorResponse { ErrorCode = "INTERNAL_ERROR", Message = "An error occurred while retrieving statistics" });
        }
    }

    /// <summary>
    /// Get midwife statistics
    /// </summary>
    [HttpGet("midwife", Name = "GetMidwifeStatistics")]
    [ProducesResponseType(typeof(MidwifeStatisticsDto), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> GetMidwifeStatistics(CancellationToken cancellationToken = default)
    {
        try
        {
            var userId = GetCurrentUserId();
            var stats = await _dashboardService.GetMidwifeStatisticsAsync(userId, cancellationToken);
            return Ok(stats);
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error retrieving midwife statistics: {ex.Message}");
            return StatusCode(500, new ErrorResponse { ErrorCode = "INTERNAL_ERROR", Message = "An error occurred while retrieving statistics" });
        }
    }

    /// <summary>
    /// Get nurse statistics
    /// </summary>
    [HttpGet("nurse", Name = "GetNurseStatistics")]
    [ProducesResponseType(typeof(NurseStatisticsDto), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> GetNurseStatistics(CancellationToken cancellationToken = default)
    {
        try
        {
            var userId = GetCurrentUserId();
            var stats = await _dashboardService.GetNurseStatisticsAsync(userId, cancellationToken);
            return Ok(stats);
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error retrieving nurse statistics: {ex.Message}");
            return StatusCode(500, new ErrorResponse { ErrorCode = "INTERNAL_ERROR", Message = "An error occurred while retrieving statistics" });
        }
    }

    /// <summary>
    /// Get patient appointment statistics
    /// </summary>
    /// <remarks>
    /// Returns:
    /// - Upcoming appointments count
    /// - Completed appointments count
    /// - Cancelled appointments count
    /// - Enrolled facilities count
    /// - List of upcoming appointments
    /// - List of recent completed appointments
    /// </remarks>
    [HttpGet("patient-appointments", Name = "GetPatientAppointmentStatistics")]
    [ProducesResponseType(typeof(PatientAppointmentStatisticsDto), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> GetPatientAppointmentStatistics(CancellationToken cancellationToken = default)
    {
        try
        {
            var userId = GetCurrentUserId();
            var stats = await _dashboardService.GetPatientAppointmentStatisticsAsync(userId, cancellationToken);
            return Ok(stats);
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error retrieving patient appointment statistics: {ex.Message}");
            return StatusCode(500, new ErrorResponse { ErrorCode = "INTERNAL_ERROR", Message = "An error occurred while retrieving statistics" });
        }
    }

    /// <summary>
    /// Get appointment trend chart data
    /// </summary>
    /// <remarks>
    /// Returns daily appointment counts for the specified number of days
    /// </remarks>
    [HttpGet("charts/appointment-trend", Name = "GetAppointmentTrendChart")]
    [ProducesResponseType(typeof(AppointmentTrendChartDto), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> GetAppointmentTrendChart([FromQuery] int daysBack = 30, CancellationToken cancellationToken = default)
    {
        try
        {
            var userId = GetCurrentUserId();
            var chart = await _dashboardService.GetAppointmentTrendChartAsync(userId, daysBack, cancellationToken);
            return Ok(chart);
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error retrieving appointment trend chart: {ex.Message}");
            return StatusCode(500, new ErrorResponse { ErrorCode = "INTERNAL_ERROR", Message = "An error occurred while retrieving chart data" });
        }
    }

    /// <summary>
    /// Get appointment status distribution chart
    /// </summary>
    [HttpGet("charts/appointment-status", Name = "GetAppointmentStatusChart")]
    [ProducesResponseType(typeof(AppointmentStatusChartDto), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> GetAppointmentStatusChart(CancellationToken cancellationToken = default)
    {
        try
        {
            var userId = GetCurrentUserId();
            var chart = await _dashboardService.GetAppointmentStatusChartAsync(userId, cancellationToken);
            return Ok(chart);
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error retrieving appointment status chart: {ex.Message}");
            return StatusCode(500, new ErrorResponse { ErrorCode = "INTERNAL_ERROR", Message = "An error occurred while retrieving chart data" });
        }
    }

    /// <summary>
    /// Get staff distribution chart by role
    /// </summary>
    [HttpGet("charts/staff-distribution", Name = "GetStaffDistributionChart")]
    [ProducesResponseType(typeof(StaffDistributionChartDto), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> GetStaffDistributionChart(CancellationToken cancellationToken = default)
    {
        try
        {
            var userId = GetCurrentUserId();
            var chart = await _dashboardService.GetStaffDistributionChartAsync(userId, cancellationToken);
            return Ok(chart);
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error retrieving staff distribution chart: {ex.Message}");
            return StatusCode(500, new ErrorResponse { ErrorCode = "INTERNAL_ERROR", Message = "An error occurred while retrieving chart data" });
        }
    }

    /// <summary>
    /// Get facility patient distribution chart
    /// </summary>
    [HttpGet("charts/facility-distribution", Name = "GetFacilityDistributionChart")]
    [ProducesResponseType(typeof(FacilityDistributionChartDto), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> GetFacilityDistributionChart(CancellationToken cancellationToken = default)
    {
        try
        {
            var userId = GetCurrentUserId();
            var chart = await _dashboardService.GetFacilityDistributionChartAsync(userId, cancellationToken);
            return Ok(chart);
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error retrieving facility distribution chart: {ex.Message}");
            return StatusCode(500, new ErrorResponse { ErrorCode = "INTERNAL_ERROR", Message = "An error occurred while retrieving chart data" });
        }
    }

    /// <summary>
    /// Helper method to get current user ID from JWT claims
    /// </summary>
    private Guid GetCurrentUserId()
    {
        var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
        if (userIdClaim == null || !Guid.TryParse(userIdClaim.Value, out var userId))
        {
            throw new InvalidOperationException("Unable to retrieve current user ID");
        }
        return userId;
    }
}
