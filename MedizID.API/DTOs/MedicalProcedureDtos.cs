namespace MedizID.API.DTOs;

public class CreateMedicalProcedureRequest
{
    public Guid MedicalRecordId { get; set; }
    public string ProcedureName { get; set; } = null!;
    public DateTime ProcedureDate { get; set; }
    public string? Indication { get; set; }
    public string? Findings { get; set; }
    public string? Complications { get; set; }
    public string? Notes { get; set; }
}

public class MedicalProcedureResponse
{
    public Guid Id { get; set; }
    public string ProcedureName { get; set; } = null!;
    public DateTime ProcedureDate { get; set; }
    public string? Indication { get; set; }
    public string? Findings { get; set; }
    public DateTime CreatedAt { get; set; }
}

public class CreateOdontogramRequest
{
    public Guid MedicalRecordId { get; set; }
    public string? ToothNumber { get; set; }
    public string? Surface { get; set; }
    public string? Status { get; set; }
    public string? Treatment { get; set; }
}

public class OdontogramResponse
{
    public Guid Id { get; set; }
    public string? ToothNumber { get; set; }
    public string? Status { get; set; }
    public string? Treatment { get; set; }
    public DateTime CreatedAt { get; set; }
}
