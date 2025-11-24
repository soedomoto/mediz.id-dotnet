namespace MedizID.API.DTOs;

// Auth DTOs
public class LoginRequest
{
    public string Email { get; set; } = null!;
    public string Password { get; set; } = null!;
}

public class LoginResponse
{
    public Guid UserId { get; set; }
    public string Email { get; set; } = null!;
    public string FirstName { get; set; } = null!;
    public string LastName { get; set; } = null!;
    public string Token { get; set; } = null!;
    public DateTime TokenExpiration { get; set; }
}

public class RegisterRequest
{
    public string Email { get; set; } = null!;
    public string Password { get; set; } = null!;
    public string FirstName { get; set; } = null!;
    public string LastName { get; set; } = null!;
    public string Role { get; set; } = null!;
}

public class RegisterResponse
{
    public Guid UserId { get; set; }
    public string Email { get; set; } = null!;
    public string Message { get; set; } = null!;
}

// Health check DTOs
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

// Pagination DTOs
public class PagedQuery
{
    public int Page { get; set; } = 1;
    public int PageSize { get; set; } = 10;
}

public class PagedResult<T>
{
    public List<T> Items { get; set; } = new();
    public int Total { get; set; }
    public int Page { get; set; }
    public int PageSize { get; set; }
    public int TotalPages => (Total + PageSize - 1) / PageSize;
}

// Error Response DTOs
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

// Google OAuth DTOs
public class GoogleUserInfo
{
    public string Id { get; set; } = null!;
    public string Email { get; set; } = null!;
    public bool VerifiedEmail { get; set; }
    public string Name { get; set; } = null!;
    public string GivenName { get; set; } = null!;
    public string FamilyName { get; set; } = null!;
    public string Picture { get; set; } = null!;
}

public class GoogleLoginRequest
{
    public string Email { get; set; } = null!;
    public string Name { get; set; } = null!;
    public string Picture { get; set; } = null!;
    public string GoogleId { get; set; } = null!;
}
