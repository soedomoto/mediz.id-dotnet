namespace MedizID.API.DTOs;

public class CreateMaternalChildHealthRequest
{
    public Guid AppointmentId { get; set; }
    public string? PregnancyWeeks { get; set; }
    public string? BloodPressure { get; set; }
    public decimal? Weight { get; set; }
    public string? FetalHeartRate { get; set; }
    public string? Notes { get; set; }
}

public class MaternalChildHealthResponse
{
    public Guid Id { get; set; }
    public string? PregnancyWeeks { get; set; }
    public string? BloodPressure { get; set; }
    public decimal? Weight { get; set; }
    public string? FetalHeartRate { get; set; }
    public DateTime CreatedAt { get; set; }
}
