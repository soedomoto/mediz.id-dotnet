namespace MedizID.API.DTOs;

public class HealthCheckResponse
{
    public string Status { get; set; } = "healthy";
    public string Service { get; set; } = "mediz.id API";
    public DateTime Timestamp { get; set; } = DateTime.UtcNow;
}

public class ApiResponse
{
    public string Message { get; set; } = null!;
    public string Version { get; set; } = null!;
    public string Docs { get; set; } = "/swagger/ui";
}
