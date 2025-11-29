namespace MedizID.API.DTOs;

public class CreateFacilityPatientRequest
{
    public Guid PatientId { get; set; }
}

public class UpdateFacilityPatientRequest
{
    public bool? IsActive { get; set; }
}

public class FacilityPatientResponse
{
    public Guid Id { get; set; }
    public Guid FacilityId { get; set; }
    public Guid PatientId { get; set; }
    public string? PatientName { get; set; }
    public string MedicalRecordNumber { get; set; } = null!;
    public bool IsActive { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
}

public class FacilityPatientsResponse
{
    public List<FacilityPatientResponse> Items { get; set; } = new();
    public int Total { get; set; }
}

// Legacy DTOs - kept for backward compatibility during transition
public class CreatePatientRequest
{
    public string FirstName { get; set; } = null!;
    public string LastName { get; set; } = null!;
    public string? PhoneNumber { get; set; }
    public string? Email { get; set; }
    public DateTime DateOfBirth { get; set; }
    public string? Gender { get; set; }
    public string? BloodType { get; set; }
    public string? Address { get; set; }
    public string? City { get; set; }
}

public class UpdatePatientRequest
{
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public string? PhoneNumber { get; set; }
    public string? Email { get; set; }
    public string? Gender { get; set; }
    public string? BloodType { get; set; }
    public string? Address { get; set; }
    public string? City { get; set; }
}

public class PatientResponse
{
    public Guid Id { get; set; }
    public string MedicalRecordNumber { get; set; } = null!;
    public string FirstName { get; set; } = null!;
    public string LastName { get; set; } = null!;
    public string? PhoneNumber { get; set; }
    public string? Email { get; set; }
    public DateTime DateOfBirth { get; set; }
    public string? Gender { get; set; }
    public string? BloodType { get; set; }
    public string? Address { get; set; }
    public string? City { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
}

public class PatientCreateResponse
{
    public Guid Id { get; set; }
    public string MedicalRecordNumber { get; set; } = null!;
    public string Message { get; set; } = "Patient created successfully";
}

public class PatientDetail
{
    public Guid Id { get; set; }
    public string MedicalRecordNumber { get; set; } = null!;
    public string FirstName { get; set; } = null!;
    public string LastName { get; set; } = null!;
    public string? PhoneNumber { get; set; }
    public string? Email { get; set; }
    public DateTime DateOfBirth { get; set; }
    public string? Gender { get; set; }
    public string? BloodType { get; set; }
    public string? Address { get; set; }
    public string? City { get; set; }
    public int TotalAppointments { get; set; }
    public int TotalMedicalRecords { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
}

public class PatientsResponse
{
    public List<PatientResponse> Items { get; set; } = new();
    public int Total { get; set; }
}

public class PatientAllergiesResponse
{
    public Guid PatientId { get; set; }
    public List<string> Allergies { get; set; } = new();
}
