namespace MedizID.API.DTOs;

public class CreateMedicalRecordRequest
{
    public Guid PatientId { get; set; }
    public Guid? AppointmentId { get; set; }
    public DateTime VisitDate { get; set; }
    public string? ChiefComplaint { get; set; }
    public string? Diagnosis { get; set; }
    public string? Treatment { get; set; }
    public string? Notes { get; set; }
}

public class UpdateMedicalRecordRequest
{
    public string? ChiefComplaint { get; set; }
    public string? Diagnosis { get; set; }
    public string? Treatment { get; set; }
    public string? Notes { get; set; }
}

public class MedicalRecordResponse
{
    public Guid Id { get; set; }
    public Guid PatientId { get; set; }
    public string PatientName { get; set; } = null!;
    public DateTime VisitDate { get; set; }
    public string? ChiefComplaint { get; set; }
    public string? Diagnosis { get; set; }
    public string? Treatment { get; set; }
    public DateTime CreatedAt { get; set; }
}

public class MedicalRecordDetail
{
    public Guid Id { get; set; }
    public Guid PatientId { get; set; }
    public string PatientName { get; set; } = null!;
    public DateTime VisitDate { get; set; }
    public string? ChiefComplaint { get; set; }
    public string? Diagnosis { get; set; }
    public string? Treatment { get; set; }
    public List<DiagnosisResponse> Diagnoses { get; set; } = new();
    public List<PrescriptionResponse> Prescriptions { get; set; } = new();
    public List<LaboratoriumResponse> LaboratoriumTests { get; set; } = new();
    public DateTime CreatedAt { get; set; }
}
