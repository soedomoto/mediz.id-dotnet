using MedizID.API.Common.Enums;

namespace MedizID.API.DTOs;

public class AddStaffToFacilityRequest
{
    public Guid StaffId { get; set; }
    public UserRoleEnum Role { get; set; }
    public string? Specialization { get; set; }
}

public class UpdateFacilityStaffRequest
{
    public UserRoleEnum? Role { get; set; }
    public string? Specialization { get; set; }
    public bool? IsActive { get; set; }
}

public class FacilityStaffResponse
{
    public Guid Id { get; set; }
    public Guid FacilityId { get; set; }
    public Guid StaffId { get; set; }
    public string? StaffName { get; set; }
    public UserRoleEnum Role { get; set; }
    public string? Specialization { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime? EndDate { get; set; }
    public bool IsActive { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
}
