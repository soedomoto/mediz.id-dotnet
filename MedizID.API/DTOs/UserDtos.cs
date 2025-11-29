namespace MedizID.API.DTOs;

public class ApplicationUserResponse
{
    public Guid Id { get; set; }
    public string FirstName { get; set; } = null!;
    public string LastName { get; set; } = null!;
    public string? Email { get; set; }
}

public class UserResponse
{
    public Guid Id { get; set; }
    public string Email { get; set; } = null!;
    public string FirstName { get; set; } = null!;
    public string LastName { get; set; } = null!;
    public string Role { get; set; } = null!;
    public string? PhoneNumber { get; set; }
    public string? LicenseNumber { get; set; }
    public string? Specialization { get; set; }
    public bool IsActive { get; set; }
    public DateTime CreatedAt { get; set; }
}

public class CreateUserRequest
{
    public string Email { get; set; } = null!;
    public string Password { get; set; } = null!;
    public string FirstName { get; set; } = null!;
    public string LastName { get; set; } = null!;
    public string Role { get; set; } = null!;
    public string? PhoneNumber { get; set; }
    public string? LicenseNumber { get; set; }
    public string? Specialization { get; set; }
    public bool IsActive { get; set; } = true;
}

public class UpdateUserRequest
{
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public string? Email { get; set; }
    public Guid? FacilityId { get; set; }
    public bool? IsActive { get; set; }
}

public class UserDetail
{
    public Guid Id { get; set; }
    public string Email { get; set; } = null!;
    public string FirstName { get; set; } = null!;
    public string LastName { get; set; } = null!;
    public string Role { get; set; } = null!;
    public string? PhoneNumber { get; set; }
    public string? LicenseNumber { get; set; }
    public string? Specialization { get; set; }
    public Guid? FacilityId { get; set; }
    public bool IsActive { get; set; }
    public DateTime CreatedAt { get; set; }
}
