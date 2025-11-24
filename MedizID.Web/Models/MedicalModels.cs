namespace MedizID.Web.Models;

public class PatientModel
{
    public Guid Id { get; set; }
    public string MRN { get; set; } = string.Empty;
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string PhoneNumber { get; set; } = string.Empty;
    public DateTime DateOfBirth { get; set; }
    public string Gender { get; set; } = string.Empty;
    public string BloodType { get; set; } = string.Empty;
    public string Address { get; set; } = string.Empty;
    public string City { get; set; } = string.Empty;
    public string State { get; set; } = string.Empty;
    public string ZipCode { get; set; } = string.Empty;
    public string Country { get; set; } = string.Empty;
    public string? EmergencyContactName { get; set; }
    public string? EmergencyContactPhone { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }

    public string FullName => $"{FirstName} {LastName}";
    public int Age => DateTime.Now.Year - DateOfBirth.Year;
}

public class AppointmentModel
{
    public Guid Id { get; set; }
    public Guid PatientId { get; set; }
    public Guid FacilityId { get; set; }
    public Guid DoctorId { get; set; }
    public DateTime ScheduledDate { get; set; }
    public string Status { get; set; } = "Scheduled";
    public string? Reason { get; set; }
    public string? Notes { get; set; }
    public DateTime CreatedAt { get; set; }
}

public class MedicalRecordModel
{
    public Guid Id { get; set; }
    public Guid PatientId { get; set; }
    public Guid FacilityId { get; set; }
    public DateTime RecordDate { get; set; }
    public string? ChiefComplaint { get; set; }
    public string? Diagnosis { get; set; }
    public string? Treatment { get; set; }
    public string? Notes { get; set; }
    public DateTime CreatedAt { get; set; }
}

public class DiagnosisModel
{
    public Guid Id { get; set; }
    public Guid MedicalRecordId { get; set; }
    public string DiseaseName { get; set; } = string.Empty;
    public string ICD10Code { get; set; } = string.Empty;
    public string Status { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
}

public class FacilityModel
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Type { get; set; } = string.Empty;
    public string Address { get; set; } = string.Empty;
    public string City { get; set; } = string.Empty;
    public string PhoneNumber { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
}
