using MedizID.API.Common.Enums;

namespace MedizID.API.Models;

public class FacilityStaff
{
    public Guid Id { get; set; }
    public Guid FacilityId { get; set; }
    public Guid StaffId { get; set; }
    public UserRoleEnum Role { get; set; }
    public string? Specialization { get; set; }
    public DateTime StartDate { get; set; } = DateTime.UtcNow;
    public DateTime? EndDate { get; set; }
    public bool IsActive { get; set; } = true;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? UpdatedAt { get; set; }

    // Relationships
    public Facility Facility { get; set; } = null!;
    public ApplicationUser Staff { get; set; } = null!;
}
