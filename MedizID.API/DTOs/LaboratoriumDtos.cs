namespace MedizID.API.DTOs;

public class CreateLaboratoriumRequest
{
    public Guid MedicalRecordId { get; set; }
    public string TestName { get; set; } = null!;
    public string? TestCode { get; set; }
    public string? Result { get; set; }
    public string? Unit { get; set; }
    public string? ReferenceRange { get; set; }
    public string? Status { get; set; }
}

public class LaboratoriumResponse
{
    public Guid Id { get; set; }
    public string TestName { get; set; } = null!;
    public string? Result { get; set; }
    public string? Unit { get; set; }
    public string? Status { get; set; }
    public DateTime TestDate { get; set; }
    public DateTime CreatedAt { get; set; }
}

public class PatientLaboratoriumSchema
{
    public Guid Id { get; set; }
    public Guid MedicalRecordId { get; set; }
    public string TestName { get; set; } = null!;
    public string? TestCode { get; set; }
    public string? Result { get; set; }
    public string? Unit { get; set; }
    public string? ReferenceRange { get; set; }
    public string? Status { get; set; }
    public DateTime TestDate { get; set; }
    public DateTime CreatedAt { get; set; }
}
