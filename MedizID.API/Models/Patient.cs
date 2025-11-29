namespace MedizID.API.Models;

public class FacilityPatient
{
    public Guid Id { get; set; }
    public Guid FacilityId { get; set; }
    public Guid PatientId { get; set; }
    public string MedicalRecordNumber { get; set; } = null!;
    public bool IsActive { get; set; } = true;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? UpdatedAt { get; set; }

    // Relationships
    public Facility Facility { get; set; } = null!;
    public ApplicationUser Patient { get; set; } = null!;
}

// Legacy Patient model - kept for backward compatibility during transition
public class Patient
{
    public Guid Id { get; set; }
    public Guid FacilityId { get; set; }
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
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? UpdatedAt { get; set; }

    // Relationships
    public Facility Facility { get; set; } = null!;
}
