namespace MedizID.API.DTOs;

public class CreateAnamnesisRequest
{
    public Guid MedicalRecordId { get; set; }
    public string? MedicalHistory { get; set; }
    public string? AllergiesHistory { get; set; }
    public string? MedicationHistory { get; set; }
    public string? SurgicalHistory { get; set; }
    public string? FamilyHistory { get; set; }
    public string? SocialHistory { get; set; }
    public string? ChiefComplaint { get; set; }
    public string? PresentIllness { get; set; }
}

public class AnamnesisResponse
{
    public Guid Id { get; set; }
    public Guid MedicalRecordId { get; set; }
    public string? MedicalHistory { get; set; }
    public string? AllergiesHistory { get; set; }
    public string? ChiefComplaint { get; set; }
    public DateTime CreatedAt { get; set; }
}

public class AnamnesisCreateResponse
{
    public Guid Id { get; set; }
    public Guid MedicalRecordId { get; set; }
    public string Message { get; set; } = "Anamnesis created successfully";
}

public class AnamnesisUpdateResponse
{
    public Guid Id { get; set; }
    public string Message { get; set; } = "Anamnesis updated successfully";
}

public class PatientAnamnesisPublic
{
    public Guid Id { get; set; }
    public Guid MedicalRecordId { get; set; }
    public string? ChiefComplaint { get; set; }
    public string? PresentingIllness { get; set; }
    public DateTime CreatedAt { get; set; }
}

public class PatientAnamnesisDetail
{
    public Guid Id { get; set; }
    public Guid MedicalRecordId { get; set; }
    public string? ChiefComplaint { get; set; }
    public string? PresentingIllness { get; set; }
    public string? PastMedicalHistory { get; set; }
    public string? Allergies { get; set; }
    public string? CurrentMedications { get; set; }
    public string? SocialHistory { get; set; }
    public DateTime CreatedAt { get; set; }
}

public class CreateAnamnesisTemplateRequest
{
    public string Name { get; set; } = null!;
    public string? Description { get; set; }
    public string? TemplateContent { get; set; }
}

public class UpdateAnamnesisTemplateRequest
{
    public string? Name { get; set; }
    public string? Description { get; set; }
    public string? TemplateContent { get; set; }
    public bool? IsActive { get; set; }
}

public class AnamnesisTemplateResponse
{
    public Guid Id { get; set; }
    public Guid FacilityId { get; set; }
    public string Name { get; set; } = null!;
    public string? Description { get; set; }
    public string? TemplateContent { get; set; }
    public bool IsActive { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
}
