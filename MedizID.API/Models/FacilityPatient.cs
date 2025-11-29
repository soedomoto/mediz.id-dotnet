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
