using MedizID.API.DTOs;
using MedizID.API.Common.Enums;
using Microsoft.EntityFrameworkCore;

namespace MedizID.API.Services;

public interface IDashboardService
{
    Task<DashboardSummaryResponse> GetUserDashboardSummaryAsync(Guid userId, CancellationToken cancellationToken = default);
    Task<FacilityOwnerStatisticsDto> GetFacilityOwnerStatisticsAsync(Guid userId, CancellationToken cancellationToken = default);
    Task<DoctorStatisticsDto> GetDoctorStatisticsAsync(Guid userId, CancellationToken cancellationToken = default);
    Task<MidwifeStatisticsDto> GetMidwifeStatisticsAsync(Guid userId, CancellationToken cancellationToken = default);
    Task<NurseStatisticsDto> GetNurseStatisticsAsync(Guid userId, CancellationToken cancellationToken = default);
    Task<PatientAppointmentStatisticsDto> GetPatientAppointmentStatisticsAsync(Guid userId, CancellationToken cancellationToken = default);
    Task<AppointmentTrendChartDto> GetAppointmentTrendChartAsync(Guid userId, int daysBack = 30, CancellationToken cancellationToken = default);
    Task<AppointmentStatusChartDto> GetAppointmentStatusChartAsync(Guid userId, CancellationToken cancellationToken = default);
    Task<StaffDistributionChartDto> GetStaffDistributionChartAsync(Guid userId, CancellationToken cancellationToken = default);
    Task<FacilityDistributionChartDto> GetFacilityDistributionChartAsync(Guid userId, CancellationToken cancellationToken = default);
}

public class DashboardService : IDashboardService
{
    private readonly MedizID.API.Data.MedizIDDbContext _dbContext;
    private readonly ILogger<DashboardService> _logger;

    public DashboardService(MedizID.API.Data.MedizIDDbContext dbContext, ILogger<DashboardService> logger)
    {
        _dbContext = dbContext;
        _logger = logger;
    }

    public async Task<DashboardSummaryResponse> GetUserDashboardSummaryAsync(Guid userId, CancellationToken cancellationToken = default)
    {
        var user = await _dbContext.Users.FindAsync(new object[] { userId }, cancellationToken: cancellationToken);
        if (user == null)
            throw new InvalidOperationException($"User not found: {userId}");

        var response = new DashboardSummaryResponse();

        if (user.Role == UserRoleEnum.FacilityOwner || user.Role == UserRoleEnum.SuperAdmin)
        {
            response.FacilityOwnerStatistics = await GetFacilityOwnerStatisticsAsync(userId, cancellationToken);
        }

        if (user.Role == UserRoleEnum.Doctor)
        {
            response.DoctorStatistics = await GetDoctorStatisticsAsync(userId, cancellationToken);
        }

        if (user.Role == UserRoleEnum.Midwife)
        {
            response.MidwifeStatistics = await GetMidwifeStatisticsAsync(userId, cancellationToken);
        }

        if (user.Role == UserRoleEnum.Nurse)
        {
            response.NurseStatistics = await GetNurseStatisticsAsync(userId, cancellationToken);
        }

        if (user.Role == UserRoleEnum.Patient)
        {
            response.PatientAppointmentStatistics = await GetPatientAppointmentStatisticsAsync(userId, cancellationToken);
        }

        return response;
    }

    public async Task<FacilityOwnerStatisticsDto> GetFacilityOwnerStatisticsAsync(Guid userId, CancellationToken cancellationToken = default)
    {
        var today = DateTime.UtcNow.Date;
        var todayEnd = today.AddDays(1);

        var managedFacilities = await _dbContext.Facilities
            .Where(f => f.Users.Any(u => u.Id == userId) && f.IsActive)
            .ToListAsync(cancellationToken);

        var facilityIds = managedFacilities.Select(f => f.Id).ToList();

        var totalPatients = await _dbContext.FacilityPatients
            .Where(fp => facilityIds.Contains(fp.FacilityId) && fp.IsActive)
            .CountAsync(cancellationToken);

        var totalStaff = await _dbContext.FacilityStaffs
            .Where(fs => facilityIds.Contains(fs.FacilityId) && fs.IsActive)
            .CountAsync(cancellationToken);

        var todayAppointments = await _dbContext.Appointments
            .Where(a => facilityIds.Contains(a.FacilityId) && 
                        a.AppointmentDate >= today && 
                        a.AppointmentDate < todayEnd)
            .CountAsync(cancellationToken);

        var pendingAppointments = await _dbContext.Appointments
            .Where(a => facilityIds.Contains(a.FacilityId) && 
                        a.Status == AppointmentStatusEnum.Scheduled)
            .CountAsync(cancellationToken);

        var completedAppointments = await _dbContext.Appointments
            .Where(a => facilityIds.Contains(a.FacilityId) && 
                        a.Status == AppointmentStatusEnum.Completed)
            .CountAsync(cancellationToken);

        var cancelledAppointments = await _dbContext.Appointments
            .Where(a => facilityIds.Contains(a.FacilityId) && 
                        a.Status == AppointmentStatusEnum.Cancelled)
            .CountAsync(cancellationToken);

        var facilityStats = new List<FacilityStatisticsDto>();
        foreach (var facility in managedFacilities)
        {
            var patients = await _dbContext.FacilityPatients
                .Where(fp => fp.FacilityId == facility.Id && fp.IsActive)
                .CountAsync(cancellationToken);

            var staff = await _dbContext.FacilityStaffs
                .Where(fs => fs.FacilityId == facility.Id && fs.IsActive)
                .CountAsync(cancellationToken);

            var todayAppts = await _dbContext.Appointments
                .Where(a => a.FacilityId == facility.Id && 
                            a.AppointmentDate >= today && 
                            a.AppointmentDate < todayEnd)
                .CountAsync(cancellationToken);

            var completedAppts = await _dbContext.Appointments
                .Where(a => a.FacilityId == facility.Id && 
                            a.Status == AppointmentStatusEnum.Completed)
                .CountAsync(cancellationToken);

            var cancelledAppts = await _dbContext.Appointments
                .Where(a => a.FacilityId == facility.Id && 
                            a.Status == AppointmentStatusEnum.Cancelled)
                .CountAsync(cancellationToken);

            facilityStats.Add(new FacilityStatisticsDto
            {
                FacilityId = facility.Id,
                FacilityName = facility.Name,
                PatientsCount = patients,
                StaffCount = staff,
                TodayAppointmentsCount = todayAppts,
                CompletedAppointmentsCount = completedAppts,
                CancelledAppointmentsCount = cancelledAppts
            });
        }

        return new FacilityOwnerStatisticsDto
        {
            ManagedFacilitiesCount = managedFacilities.Count,
            TotalPatientsCount = totalPatients,
            TotalStaffCount = totalStaff,
            TodayAppointmentsCount = todayAppointments,
            PendingAppointmentsCount = pendingAppointments,
            CompletedAppointmentsCount = completedAppointments,
            CancelledAppointmentsCount = cancelledAppointments,
            FacilityStats = facilityStats.ToArray()
        };
    }

    public async Task<DoctorStatisticsDto> GetDoctorStatisticsAsync(Guid userId, CancellationToken cancellationToken = default)
    {
        return await GetStaffStatisticsAsync<DoctorStatisticsDto>(userId, UserRoleEnum.Doctor, cancellationToken);
    }

    public async Task<MidwifeStatisticsDto> GetMidwifeStatisticsAsync(Guid userId, CancellationToken cancellationToken = default)
    {
        return await GetStaffStatisticsAsync<MidwifeStatisticsDto>(userId, UserRoleEnum.Midwife, cancellationToken);
    }

    public async Task<NurseStatisticsDto> GetNurseStatisticsAsync(Guid userId, CancellationToken cancellationToken = default)
    {
        return await GetStaffStatisticsAsync<NurseStatisticsDto>(userId, UserRoleEnum.Nurse, cancellationToken);
    }

    private async Task<T> GetStaffStatisticsAsync<T>(Guid userId, UserRoleEnum role, CancellationToken cancellationToken) 
        where T : class, new()
    {
        var today = DateTime.UtcNow.Date;
        var todayEnd = today.AddDays(1);

        var staffAssignments = await _dbContext.FacilityStaffs
            .Where(fs => fs.StaffId == userId && fs.Role == role && fs.IsActive)
            .ToListAsync(cancellationToken);

        var facilityIds = staffAssignments.Select(fs => fs.FacilityId).ToList();

        var todayAppointments = await _dbContext.Appointments
            .Where(a => facilityIds.Contains(a.FacilityId) && 
                        a.AppointmentDate >= today && 
                        a.AppointmentDate < todayEnd)
            .CountAsync(cancellationToken);

        var completedAppointments = await _dbContext.Appointments
            .Where(a => facilityIds.Contains(a.FacilityId) && 
                        a.Status == AppointmentStatusEnum.Completed)
            .CountAsync(cancellationToken);

        var patientsSeen = await _dbContext.Appointments
            .Where(a => facilityIds.Contains(a.FacilityId) && 
                        a.Status == AppointmentStatusEnum.Completed)
            .Select(a => a.FacilityPatientId)
            .Distinct()
            .CountAsync(cancellationToken);

        var facilityStats = new List<FacilityStaffingStatisticsDto>();
        foreach (var facility in staffAssignments)
        {
            var todayAppts = await _dbContext.Appointments
                .Where(a => a.FacilityId == facility.FacilityId && 
                            a.AppointmentDate >= today && 
                            a.AppointmentDate < todayEnd)
                .CountAsync(cancellationToken);

            var completedAppts = await _dbContext.Appointments
                .Where(a => a.FacilityId == facility.FacilityId && 
                            a.Status == AppointmentStatusEnum.Completed)
                .CountAsync(cancellationToken);

            var cancelledAppts = await _dbContext.Appointments
                .Where(a => a.FacilityId == facility.FacilityId && 
                            a.Status == AppointmentStatusEnum.Cancelled)
                .CountAsync(cancellationToken);

            var facilityName = await _dbContext.Facilities
                .Where(f => f.Id == facility.FacilityId)
                .Select(f => f.Name)
                .FirstAsync(cancellationToken);

            facilityStats.Add(new FacilityStaffingStatisticsDto
            {
                FacilityId = facility.FacilityId,
                FacilityName = facilityName,
                TodayAppointmentsCount = todayAppts,
                CompletedAppointmentsCount = completedAppts,
                CancelledAppointmentsCount = cancelledAppts
            });
        }

        var stats = new T();
        var properties = typeof(T).GetProperties();

        foreach (var prop in properties)
        {
            if (prop.Name == "StaffedFacilitiesCount")
                prop.SetValue(stats, facilityStats.Count);
            else if (prop.Name == "TodayAppointmentsCount")
                prop.SetValue(stats, todayAppointments);
            else if (prop.Name == "CompletedAppointmentsCount")
                prop.SetValue(stats, completedAppointments);
            else if (prop.Name == "PatientsSeenCount")
                prop.SetValue(stats, patientsSeen);
            else if (prop.Name == "FacilityStats")
                prop.SetValue(stats, facilityStats.ToArray());
        }

        return stats;
    }

    public async Task<PatientAppointmentStatisticsDto> GetPatientAppointmentStatisticsAsync(Guid userId, CancellationToken cancellationToken = default)
    {
        var today = DateTime.UtcNow.Date;

        var appointments = await _dbContext.Appointments
            .Where(a => _dbContext.FacilityPatients.Any(fp => fp.PatientId == userId && fp.Id == a.FacilityPatientId))
            .ToListAsync(cancellationToken);

        var upcomingAppointments = appointments
            .Where(a => a.AppointmentDate >= today && a.Status != AppointmentStatusEnum.Cancelled)
            .OrderBy(a => a.AppointmentDate)
            .Take(10)
            .ToList();

        var recentAppointments = appointments
            .Where(a => a.Status == AppointmentStatusEnum.Completed)
            .OrderByDescending(a => a.AppointmentDate)
            .Take(5)
            .ToList();

        var upcomingDtos = new List<AppointmentDetailDto>();
        foreach (var appt in upcomingAppointments)
        {
            var facility = await _dbContext.Facilities
                .Where(f => f.Id == appt.FacilityId)
                .Select(f => f.Name)
                .FirstAsync(cancellationToken);

            upcomingDtos.Add(new AppointmentDetailDto
            {
                AppointmentId = appt.Id,
                FacilityName = facility,
                AppointmentDate = appt.AppointmentDate,
                AppointmentTime = appt.AppointmentTime,
                Status = appt.Status.ToString(),
                Reason = appt.Reason
            });
        }

        var recentDtos = new List<AppointmentDetailDto>();
        foreach (var appt in recentAppointments)
        {
            var facility = await _dbContext.Facilities
                .Where(f => f.Id == appt.FacilityId)
                .Select(f => f.Name)
                .FirstAsync(cancellationToken);

            recentDtos.Add(new AppointmentDetailDto
            {
                AppointmentId = appt.Id,
                FacilityName = facility,
                AppointmentDate = appt.AppointmentDate,
                AppointmentTime = appt.AppointmentTime,
                Status = appt.Status.ToString(),
                Reason = appt.Reason
            });
        }

        var enrolledFacilities = await _dbContext.FacilityPatients
            .Where(fp => fp.PatientId == userId && fp.IsActive)
            .Select(fp => fp.FacilityId)
            .Distinct()
            .CountAsync(cancellationToken);

        return new PatientAppointmentStatisticsDto
        {
            UpcomingAppointmentsCount = appointments.Count(a => a.AppointmentDate >= today && a.Status != AppointmentStatusEnum.Cancelled),
            CompletedAppointmentsCount = appointments.Count(a => a.Status == AppointmentStatusEnum.Completed),
            CancelledAppointmentsCount = appointments.Count(a => a.Status == AppointmentStatusEnum.Cancelled),
            EnrolledFacilitiesCount = enrolledFacilities,
            UpcomingAppointments = upcomingDtos.ToArray(),
            RecentAppointments = recentDtos.ToArray()
        };
    }

    public async Task<AppointmentTrendChartDto> GetAppointmentTrendChartAsync(Guid userId, int daysBack = 30, CancellationToken cancellationToken = default)
    {
        var user = await _dbContext.Users.FindAsync(new object[] { userId }, cancellationToken: cancellationToken);
        if (user == null)
            throw new InvalidOperationException($"User not found: {userId}");

        var startDate = DateTime.UtcNow.AddDays(-daysBack).Date;
        var data = new List<ChartDataPoint>();

        var facilityIds = await GetUserFacilityIdsAsync(userId, user.Role, cancellationToken);

        for (int i = 0; i < daysBack; i++)
        {
            var date = startDate.AddDays(i);
            var dateEnd = date.AddDays(1);

            var count = await _dbContext.Appointments
                .Where(a => facilityIds.Contains(a.FacilityId) && 
                            a.AppointmentDate >= date && 
                            a.AppointmentDate < dateEnd)
                .CountAsync(cancellationToken);

            data.Add(new ChartDataPoint
            {
                Label = date.ToString("MMM dd"),
                Value = count
            });
        }

        return new AppointmentTrendChartDto
        {
            Data = data.ToArray(),
            Title = "Appointments Trend (Last 30 Days)"
        };
    }

    public async Task<AppointmentStatusChartDto> GetAppointmentStatusChartAsync(Guid userId, CancellationToken cancellationToken = default)
    {
        var user = await _dbContext.Users.FindAsync(new object[] { userId }, cancellationToken: cancellationToken);
        if (user == null)
            throw new InvalidOperationException($"User not found: {userId}");

        var facilityIds = await GetUserFacilityIdsAsync(userId, user.Role, cancellationToken);

        var data = new List<ChartDataPoint>();

        var scheduled = await _dbContext.Appointments
            .Where(a => facilityIds.Contains(a.FacilityId) && a.Status == AppointmentStatusEnum.Scheduled)
            .CountAsync(cancellationToken);

        var completed = await _dbContext.Appointments
            .Where(a => facilityIds.Contains(a.FacilityId) && a.Status == AppointmentStatusEnum.Completed)
            .CountAsync(cancellationToken);

        var cancelled = await _dbContext.Appointments
            .Where(a => facilityIds.Contains(a.FacilityId) && a.Status == AppointmentStatusEnum.Cancelled)
            .CountAsync(cancellationToken);

        data.Add(new ChartDataPoint { Label = "Scheduled", Value = scheduled });
        data.Add(new ChartDataPoint { Label = "Completed", Value = completed });
        data.Add(new ChartDataPoint { Label = "Cancelled", Value = cancelled });

        return new AppointmentStatusChartDto
        {
            Data = data.ToArray(),
            Title = "Appointment Status Distribution"
        };
    }

    public async Task<StaffDistributionChartDto> GetStaffDistributionChartAsync(Guid userId, CancellationToken cancellationToken = default)
    {
        var user = await _dbContext.Users.FindAsync(new object[] { userId }, cancellationToken: cancellationToken);
        if (user == null)
            throw new InvalidOperationException($"User not found: {userId}");

        var facilityIds = await GetUserFacilityIdsAsync(userId, user.Role, cancellationToken);

        var data = new List<ChartDataPoint>();

        var roles = new[] { UserRoleEnum.Doctor, UserRoleEnum.Midwife, UserRoleEnum.Nurse, UserRoleEnum.Pharmacist };

        foreach (var role in roles)
        {
            var count = await _dbContext.FacilityStaffs
                .Where(fs => facilityIds.Contains(fs.FacilityId) && fs.Role == role && fs.IsActive)
                .CountAsync(cancellationToken);

            if (count > 0)
            {
                data.Add(new ChartDataPoint
                {
                    Label = role.ToString(),
                    Value = count
                });
            }
        }

        return new StaffDistributionChartDto
        {
            Data = data.ToArray(),
            Title = "Staff Distribution by Role"
        };
    }

    public async Task<FacilityDistributionChartDto> GetFacilityDistributionChartAsync(Guid userId, CancellationToken cancellationToken = default)
    {
        var user = await _dbContext.Users.FindAsync(new object[] { userId }, cancellationToken: cancellationToken);
        if (user == null)
            throw new InvalidOperationException($"User not found: {userId}");

        var facilityIds = await GetUserFacilityIdsAsync(userId, user.Role, cancellationToken);

        var data = new List<ChartDataPoint>();

        var facilities = await _dbContext.Facilities
            .Where(f => facilityIds.Contains(f.Id))
            .ToListAsync(cancellationToken);

        foreach (var facility in facilities)
        {
            var patientCount = await _dbContext.FacilityPatients
                .Where(fp => fp.FacilityId == facility.Id && fp.IsActive)
                .CountAsync(cancellationToken);

            data.Add(new ChartDataPoint
            {
                Label = facility.Name,
                Value = patientCount
            });
        }

        return new FacilityDistributionChartDto
        {
            Data = data.ToArray(),
            Title = "Patient Distribution by Facility"
        };
    }

    private async Task<List<Guid>> GetUserFacilityIdsAsync(Guid userId, UserRoleEnum role, CancellationToken cancellationToken)
    {
        if (role == UserRoleEnum.FacilityOwner || role == UserRoleEnum.SuperAdmin)
        {
            return await _dbContext.Facilities
                .Where(f => f.Users.Any(u => u.Id == userId) && f.IsActive)
                .Select(f => f.Id)
                .ToListAsync(cancellationToken);
        }

        if (role == UserRoleEnum.Doctor || role == UserRoleEnum.Midwife || role == UserRoleEnum.Nurse || role == UserRoleEnum.Pharmacist)
        {
            return await _dbContext.FacilityStaffs
                .Where(fs => fs.StaffId == userId && fs.IsActive)
                .Select(fs => fs.FacilityId)
                .ToListAsync(cancellationToken);
        }

        return new List<Guid>();
    }
}
