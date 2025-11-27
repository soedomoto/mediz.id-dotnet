namespace MedizID.API.DTOs;

public class CreatePoliRequest
{
    public Guid InstallationId { get; set; }
    public string Name { get; set; } = null!;
    public string? Description { get; set; }
}

public class UpdatePoliRequest
{
    public Guid? InstallationId { get; set; }
    public string? Name { get; set; }
    public string? Description { get; set; }
    public bool? IsActive { get; set; }
}

public class PoliResponse
{
    public Guid Id { get; set; }
    public Guid FacilityId { get; set; }
    public Guid InstallationId { get; set; }
    public string Name { get; set; } = null!;
    public string? Description { get; set; }
    public bool IsActive { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
}

public class CreatePoliTimeSlotRequest
{
    public Guid? StaffId { get; set; }
    public int DayOfWeek { get; set; }
    public TimeSpan StartTime { get; set; }
    public TimeSpan EndTime { get; set; }
    public int SlotDuration { get; set; }
    public int MaxPatients { get; set; }
}

public class UpdatePoliTimeSlotRequest
{
    public Guid? StaffId { get; set; }
    public int? DayOfWeek { get; set; }
    public TimeSpan? StartTime { get; set; }
    public TimeSpan? EndTime { get; set; }
    public int? SlotDuration { get; set; }
    public int? MaxPatients { get; set; }
    public bool? IsActive { get; set; }
}

public class PoliTimeSlotResponse
{
    public Guid Id { get; set; }
    public Guid PoliId { get; set; }
    public Guid? StaffId { get; set; }
    public int DayOfWeek { get; set; }
    public TimeSpan StartTime { get; set; }
    public TimeSpan EndTime { get; set; }
    public int SlotDuration { get; set; }
    public int MaxPatients { get; set; }
    public bool IsActive { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
}
