namespace MedizID.API.DTOs;

public class CreateAdolescentHealthRequest
{
    public Guid AppointmentId { get; set; }
    public string? HealthConcern { get; set; }
    public string? SexualActivity { get; set; }
    public string? Contraception { get; set; }
    public string? Counseling { get; set; }
}

public class UpdateAdolescentHealthRequest
{
    public string? HealthConcern { get; set; }
    public string? SexualActivity { get; set; }
    public string? Contraception { get; set; }
    public string? Counseling { get; set; }
}

public class AdolescentHealthResponse
{
    public Guid Id { get; set; }
    public Guid AppointmentId { get; set; }
    public string? HealthConcern { get; set; }
    public string? SexualActivity { get; set; }
    public string? Contraception { get; set; }
    public string? Counseling { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
}
