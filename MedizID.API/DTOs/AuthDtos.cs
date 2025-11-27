namespace MedizID.API.DTOs;

// Login DTOs
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

// Register DTOs
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
