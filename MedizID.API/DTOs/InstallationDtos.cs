using MedizID.API.Common.Enums;

namespace MedizID.API.DTOs;

public class CreateInstallationRequest
{
    public string Name { get; set; } = null!;
    public InstallationType? Type { get; set; }
    public string? Location { get; set; }
    public string? Condition { get; set; }
    public DateTime? LastMaintenanceDate { get; set; }
    public bool? IsActive { get; set; }
}

public class UpdateInstallationRequest
{
    public string? Name { get; set; }
    public InstallationType? Type { get; set; }
    public string? Location { get; set; }
    public string? Condition { get; set; }
    public DateTime? LastMaintenanceDate { get; set; }
    public bool? IsActive { get; set; }
}

public class InstallationResponse
{
    public Guid Id { get; set; }
    public Guid FacilityId { get; set; }
    public string Name { get; set; } = null!;
    public InstallationType? Type { get; set; }
    public string? Location { get; set; }
    public string? Condition { get; set; }
    public DateTime? LastMaintenanceDate { get; set; }
    public bool IsActive { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
}
