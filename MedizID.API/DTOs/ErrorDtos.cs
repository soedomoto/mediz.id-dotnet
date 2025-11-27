namespace MedizID.API.DTOs;

public class ErrorResponse
{
    public string ErrorCode { get; set; } = null!;
    public string Message { get; set; } = null!;
    public string? Details { get; set; }
    public DateTime Timestamp { get; set; } = DateTime.UtcNow;
}

public class ValidationErrorResponse : ErrorResponse
{
    public Dictionary<string, string[]> Errors { get; set; } = new();
}
