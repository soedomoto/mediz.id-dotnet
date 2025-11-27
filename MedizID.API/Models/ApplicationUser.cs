using Microsoft.AspNetCore.Identity;
using MedizID.API.Common.Enums;

namespace MedizID.API.Models;

public class ApplicationUser : IdentityUser<Guid>
{
    public string FirstName { get; set; } = null!;
    public string LastName { get; set; } = null!;
    public UserRoleEnum Role { get; set; }
    public Guid? FacilityId { get; set; }
    public Facility? Facility { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? UpdatedAt { get; set; }
    public bool IsActive { get; set; } = true;
}
