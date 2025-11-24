namespace MedizID.Web.Models;

/// <summary>Facility response model (generated from OpenAPI)</summary>
public class FacilityResponse
{
    public int Id { get; set; }
    public string? Name { get; set; }
    public string? Address { get; set; }
    public string? City { get; set; }
    public string? PhoneNumber { get; set; }
    public string? Email { get; set; }
    public bool IsActive { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
}

/// <summary>Facility list response</summary>
public class FacilityListResponse
{
    public List<FacilityResponse>? Facilities { get; set; }
    public int Total { get; set; }
}

/// <summary>Create facility request</summary>
public class CreateFacilityRequest
{
    public required string Name { get; set; }
    public string? Address { get; set; }
    public string? City { get; set; }
    public string? PhoneNumber { get; set; }
    public string? Email { get; set; }
}

/// <summary>Update facility request</summary>
public class UpdateFacilityRequest
{
    public string? Name { get; set; }
    public string? Address { get; set; }
    public string? City { get; set; }
    public string? PhoneNumber { get; set; }
    public string? Email { get; set; }
    public bool? IsActive { get; set; }
}

/// <summary>Context passed to child components (equivalent to React's useOutletContext)</summary>
public class FacilityContextValue
{
    public required List<FacilityResponse> Facilities { get; set; }
    public required Func<Task> RefreshFacilities { get; set; }
}
