namespace MedizID.API.DTOs;

public class CreateOdontogramRequest
{
    public Guid AppointmentId { get; set; }
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
