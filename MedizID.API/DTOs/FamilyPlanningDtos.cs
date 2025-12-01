namespace MedizID.API.DTOs;

public class CreateFamilyPlanningRequest
{
    public Guid AppointmentId { get; set; }
    public string? CurrentMethod { get; set; }
    public DateTime? StartDate { get; set; }
    public string? Status { get; set; }
    public string? Notes { get; set; }
}

public class FamilyPlanningResponse
{
    public Guid Id { get; set; }
    public string? CurrentMethod { get; set; }
    public DateTime? StartDate { get; set; }
    public string? Status { get; set; }
    public DateTime CreatedAt { get; set; }
}
