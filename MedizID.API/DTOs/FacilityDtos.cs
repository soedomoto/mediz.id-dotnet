using MedizID.API.Common.Enums;

namespace MedizID.API.DTOs;

public class CreateFacilityRequest
{
    public string Name { get; set; } = null!;
    public string Address { get; set; } = null!;
    public string City { get; set; } = null!;
    public string Province { get; set; } = null!;
    public string PostalCode { get; set; } = null!;
    public string? PhoneNumber { get; set; }
    public string? Email { get; set; }
    public int Type { get; set; }
}

public class UpdateFacilityRequest
{
    public string? Name { get; set; }
    public int? Type { get; set; }
    public string? Address { get; set; }
    public string? City { get; set; }
    public string? Province { get; set; }
    public string? PostalCode { get; set; }
    public string? PhoneNumber { get; set; }
    public string? Email { get; set; }
    public bool? IsActive { get; set; }
}

public class FacilityResponse
{
    public Guid Id { get; set; }
    public string Name { get; set; } = null!;
    public string Address { get; set; } = null!;
    public string City { get; set; } = null!;
    public string Province { get; set; } = null!;
    public string PostalCode { get; set; } = null!;
    public string? PhoneNumber { get; set; }
    public string? Email { get; set; }
    public FacilityTypeEnum? Type { get; set; }
    public bool IsActive { get; set; }
    public DateTime CreatedAt { get; set; }
}

public class CreateFacilityAppointmentRequest
{
    public Guid FacilityPatientId { get; set; }
    public Guid? FacilityDoctorId { get; set; }
    public DateTime AppointmentDate { get; set; }
    public TimeSpan AppointmentTime { get; set; }
    public string? Reason { get; set; }
    public string? Notes { get; set; }
}

public class FacilityAppointmentResponse
{
    public Guid Id { get; set; }
    public Guid FacilityId { get; set; }
    public Guid FacilityPatientId { get; set; }
    public Guid? FacilityDoctorId { get; set; }
    public DateTime AppointmentDate { get; set; }
    public TimeSpan AppointmentTime { get; set; }
    public string Status { get; set; } = null!;
    public string? Reason { get; set; }
    public string? Notes { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
}
