using MedizID.API.Common.Enums;

namespace MedizID.API.Models;

// Facilities Management Models
public class Installation
{
    public Guid Id { get; set; }
    public Guid FacilityId { get; set; }
    public string Name { get; set; } = null!;
    public InstallationType? Type { get; set; }
    public string? Location { get; set; }
    public string? Condition { get; set; }
    public DateTime? LastMaintenanceDate { get; set; }
    public bool IsActive { get; set; } = true;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? UpdatedAt { get; set; }

    // Relationships
    public Facility Facility { get; set; } = null!;
}
