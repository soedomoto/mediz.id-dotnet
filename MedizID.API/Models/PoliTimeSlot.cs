namespace MedizID.API.Models;

public class PoliTimeSlot
{
    public Guid Id { get; set; }
    public Guid PoliId { get; set; }
    public Guid? StaffId { get; set; }
    public DayOfWeek DayOfWeek { get; set; }
    public TimeSpan StartTime { get; set; }
    public TimeSpan EndTime { get; set; }
    public int SlotDuration { get; set; } // in minutes
    public int MaxPatients { get; set; }
    public bool IsActive { get; set; } = true;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? UpdatedAt { get; set; }

    // Relationships
    public Poli Poli { get; set; } = null!;
    public ApplicationUser? Staff { get; set; }
}
