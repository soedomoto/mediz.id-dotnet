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
