namespace MedizID.API.Models;

public class MedicalEquipmentType
{
    public Guid Id { get; set; }
    public string Name { get; set; } = null!;
    public string? Description { get; set; }
    public bool IsActive { get; set; } = true;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? UpdatedAt { get; set; }

    // Relationships
    public ICollection<MedicalEquipment> Equipment { get; set; } = new List<MedicalEquipment>();
}
