namespace MedizID.API.DTOs;

public class ToothStateDto
{
    public int Number { get; set; }
    public string Surface { get; set; } = null!;
    public string ConditionCode { get; set; } = null!;
}

public class SaveOdontogramRequest
{
    public Guid AppointmentId { get; set; }
    public List<ToothStateDto> ToothStates { get; set; } = new();
}

public class OdontogramSurfaceResponse
{
    public Guid Id { get; set; }
    public int ToothNumber { get; set; }
    public string Surface { get; set; } = null!;
    public string ConditionCode { get; set; } = null!;
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
}

public class OdontogramResponse
{
    public Guid Id { get; set; }
    public Guid AppointmentId { get; set; }
    public List<OdontogramSurfaceResponse> Surfaces { get; set; } = new();
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
}

public class CreateOdontogramRequest
{
    public Guid AppointmentId { get; set; }
    public string? ToothNumber { get; set; }
    public string? Surface { get; set; }
    public string? Status { get; set; }
    public string? Treatment { get; set; }
}
