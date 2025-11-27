namespace MedizID.API.DTOs;

public class CreateDepartmentRequest
{
    public string Name { get; set; } = null!;
    public string? Description { get; set; }
}

public class UpdateDepartmentRequest
{
    public string? Name { get; set; }
    public string? Description { get; set; }
    public bool? IsActive { get; set; }
}

public class DepartmentResponse
{
    public Guid Id { get; set; }
    public Guid FacilityId { get; set; }
    public string Name { get; set; } = null!;
    public string? Description { get; set; }
    public bool IsActive { get; set; }
    public DateTime CreatedAt { get; set; }
}

public class AddStaffToFacilityRequest
{
    public Guid StaffId { get; set; }
    public Guid? DepartmentId { get; set; }
    public string Position { get; set; } = null!;
    public string? Specialization { get; set; }
}

public class UpdateFacilityStaffRequest
{
    public Guid? DepartmentId { get; set; }
    public string? Position { get; set; }
    public string? Specialization { get; set; }
    public bool? IsActive { get; set; }
}

public class FacilityStaffResponse
{
    public Guid Id { get; set; }
    public Guid FacilityId { get; set; }
    public Guid StaffId { get; set; }
    public Guid? DepartmentId { get; set; }
    public string Position { get; set; } = null!;
    public string? Specialization { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime? EndDate { get; set; }
    public bool IsActive { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
}
