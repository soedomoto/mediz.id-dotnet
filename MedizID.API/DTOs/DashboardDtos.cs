namespace MedizID.API.DTOs;

// Summary Statistics DTOs
public class DashboardSummaryResponse
{
    public FacilityOwnerStatisticsDto? FacilityOwnerStatistics { get; set; }
    public DoctorStatisticsDto? DoctorStatistics { get; set; }
    public MidwifeStatisticsDto? MidwifeStatistics { get; set; }
    public NurseStatisticsDto? NurseStatistics { get; set; }
    public PatientAppointmentStatisticsDto? PatientAppointmentStatistics { get; set; }
}

// Facility Owner / Superuser Statistics
public class FacilityOwnerStatisticsDto
{
    public int ManagedFacilitiesCount { get; set; }
    public int TotalPatientsCount { get; set; }
    public int TotalStaffCount { get; set; }
    public int TodayAppointmentsCount { get; set; }
    public int PendingAppointmentsCount { get; set; }
    public int CompletedAppointmentsCount { get; set; }
    public int CancelledAppointmentsCount { get; set; }
    public FacilityStatisticsDto[] FacilityStats { get; set; } = Array.Empty<FacilityStatisticsDto>();
}

public class FacilityStatisticsDto
{
    public Guid FacilityId { get; set; }
    public string FacilityName { get; set; } = null!;
    public int PatientsCount { get; set; }
    public int StaffCount { get; set; }
    public int TodayAppointmentsCount { get; set; }
    public int CompletedAppointmentsCount { get; set; }
    public int CancelledAppointmentsCount { get; set; }
}

// Doctor Statistics
public class DoctorStatisticsDto
{
    public int StaffedFacilitiesCount { get; set; }
    public int TodayAppointmentsCount { get; set; }
    public int CompletedAppointmentsCount { get; set; }
    public int PatientsSeenCount { get; set; }
    public FacilityStaffingStatisticsDto[] FacilityStats { get; set; } = Array.Empty<FacilityStaffingStatisticsDto>();
}

// Midwife Statistics
public class MidwifeStatisticsDto
{
    public int StaffedFacilitiesCount { get; set; }
    public int TodayAppointmentsCount { get; set; }
    public int CompletedAppointmentsCount { get; set; }
    public int PatientsSeenCount { get; set; }
    public FacilityStaffingStatisticsDto[] FacilityStats { get; set; } = Array.Empty<FacilityStaffingStatisticsDto>();
}

// Nurse Statistics
public class NurseStatisticsDto
{
    public int StaffedFacilitiesCount { get; set; }
    public int TodayAppointmentsCount { get; set; }
    public int CompletedAppointmentsCount { get; set; }
    public int PatientsSeenCount { get; set; }
    public FacilityStaffingStatisticsDto[] FacilityStats { get; set; } = Array.Empty<FacilityStaffingStatisticsDto>();
}

public class FacilityStaffingStatisticsDto
{
    public Guid FacilityId { get; set; }
    public string FacilityName { get; set; } = null!;
    public int TodayAppointmentsCount { get; set; }
    public int CompletedAppointmentsCount { get; set; }
    public int CancelledAppointmentsCount { get; set; }
}

// Patient Appointment Statistics
public class PatientAppointmentStatisticsDto
{
    public int UpcomingAppointmentsCount { get; set; }
    public int CompletedAppointmentsCount { get; set; }
    public int CancelledAppointmentsCount { get; set; }
    public int EnrolledFacilitiesCount { get; set; }
    public AppointmentDetailDto[] UpcomingAppointments { get; set; } = Array.Empty<AppointmentDetailDto>();
    public AppointmentDetailDto[] RecentAppointments { get; set; } = Array.Empty<AppointmentDetailDto>();
}

public class AppointmentDetailDto
{
    public Guid AppointmentId { get; set; }
    public string FacilityName { get; set; } = null!;
    public DateTime AppointmentDate { get; set; }
    public TimeSpan AppointmentTime { get; set; }
    public string Status { get; set; } = null!;
    public string? Reason { get; set; }
}

// Chart Data DTOs
public class ChartDataPoint
{
    public string Label { get; set; } = null!;
    public int Value { get; set; }
}

public class AppointmentTrendChartDto
{
    public ChartDataPoint[] Data { get; set; } = Array.Empty<ChartDataPoint>();
    public string Title { get; set; } = "Appointments Trend";
}

public class FacilityDistributionChartDto
{
    public ChartDataPoint[] Data { get; set; } = Array.Empty<ChartDataPoint>();
    public string Title { get; set; } = "Facility Distribution";
}

public class AppointmentStatusChartDto
{
    public ChartDataPoint[] Data { get; set; } = Array.Empty<ChartDataPoint>();
    public string Title { get; set; } = "Appointment Status";
}

public class StaffDistributionChartDto
{
    public ChartDataPoint[] Data { get; set; } = Array.Empty<ChartDataPoint>();
    public string Title { get; set; } = "Staff Distribution by Role";
}
