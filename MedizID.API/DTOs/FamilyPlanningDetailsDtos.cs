namespace MedizID.API.DTOs;

public class CreateFamilyPlanningContraceptiveMethodRequest
{
    public string? MethodName { get; set; }
    public DateTime? ServiceDate { get; set; }
    public int? Quantity { get; set; }
    public string? Notes { get; set; }
}

public class FamilyPlanningContraceptiveMethodResponse
{
    public Guid Id { get; set; }
    public Guid FamilyPlanningId { get; set; }
    public string? MethodName { get; set; }
    public DateTime? ServiceDate { get; set; }
    public int? Quantity { get; set; }
    public string? Notes { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
    
    /// <summary>
    /// Temporary ID for client-side operations (not persisted)
    /// </summary>
    [System.Text.Json.Serialization.JsonIgnore]
    public Guid? TempId { get; set; }
}

public class CreateFamilyPlanningLaboratoryResultRequest
{
    public string? TestName { get; set; }
    public string? Result { get; set; }
    public string? ReferenceValue { get; set; }
    public DateTime? TestDate { get; set; }
    public string? Notes { get; set; }
}

public class FamilyPlanningLaboratoryResultResponse
{
    public Guid Id { get; set; }
    public Guid FamilyPlanningId { get; set; }
    public string? TestName { get; set; }
    public string? Result { get; set; }
    public string? ReferenceValue { get; set; }
    public DateTime? TestDate { get; set; }
    public string? Notes { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
}

public class CreateFamilyPlanningProcedureRequest
{
    public string? ProcedureName { get; set; }
    public DateTime? ProcedureDate { get; set; }
    public string? PerformedBy { get; set; }
    public string? Outcome { get; set; }
    public string? Complications { get; set; }
    public string? Notes { get; set; }
}

public class FamilyPlanningProcedureResponse
{
    public Guid Id { get; set; }
    public Guid FamilyPlanningId { get; set; }
    public string? ProcedureName { get; set; }
    public DateTime? ProcedureDate { get; set; }
    public string? PerformedBy { get; set; }
    public string? Outcome { get; set; }
    public string? Complications { get; set; }
    public string? Notes { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
}
