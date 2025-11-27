namespace MedizID.API.DTOs;

public class CreateAppointmentRequest
{
    public Guid PatientId { get; set; }
    public Guid? DoctorId { get; set; }
    public DateTime AppointmentDate { get; set; }
    public TimeSpan AppointmentTime { get; set; }
    public string? Reason { get; set; }
    public string? Notes { get; set; }
}

public class UpdateAppointmentRequest
{
    public DateTime? AppointmentDate { get; set; }
    public TimeSpan? AppointmentTime { get; set; }
    public string? Status { get; set; }
    public string? Reason { get; set; }
    public string? Notes { get; set; }
}

public class AppointmentResponse
{
    public Guid Id { get; set; }
    public Guid PatientId { get; set; }
    public string PatientName { get; set; } = null!;
    public Guid? DoctorId { get; set; }
    public string? DoctorName { get; set; }
    public DateTime AppointmentDate { get; set; }
    public TimeSpan AppointmentTime { get; set; }
    public string Status { get; set; } = null!;
    public string? Reason { get; set; }
    public DateTime CreatedAt { get; set; }
}

public class AppointmentDetailResponse
{
    public Guid Id { get; set; }
    public Guid PatientId { get; set; }
    public string PatientName { get; set; } = null!;
    public string? PatientIdNumber { get; set; }
    public Guid? DoctorId { get; set; }
    public string? DoctorName { get; set; }
    public DateTime AppointmentDate { get; set; }
    public TimeSpan AppointmentTime { get; set; }
    public string Status { get; set; } = null!;
    public string? Reason { get; set; }
    public string? Notes { get; set; }
    public string? PoliName { get; set; }
    public string? InstallationName { get; set; }
    public string? PatientRoomBed { get; set; }
    public string? Insurance { get; set; }
    public string? ReferringDoctor { get; set; }
    public DateTime CreatedAt { get; set; }
}

public class AppointmentMedicalHistoryResponse
{
    public string Waktu { get; set; } = null!;
    public string KodeICD10 { get; set; } = null!;
    public string NamaICD10 { get; set; } = null!;
    public string Diagnosa { get; set; } = null!;
    public string ObatObatan { get; set; } = null!;
    public string Status { get; set; } = null!;
}

public class AppointmentCreateResponse
{
    public Guid Id { get; set; }
    public string Message { get; set; } = "Appointment created successfully";
}

public class AppointmentsResponse
{
    public List<AppointmentResponse> Items { get; set; } = new();
    public int Total { get; set; }
}
