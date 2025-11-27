namespace MedizID.API.Models;

public class MedicalEquipment
{
    public Guid Id { get; set; }
    public string Name { get; set; } = null!;
    public Guid EquipmentTypeId { get; set; }
    public string? SerialNumber { get; set; }
    public Guid FacilityId { get; set; }
    public DateTime? PurchaseDate { get; set; }
    public DateTime? LastServiceDate { get; set; }
    public bool IsActive { get; set; } = true;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? UpdatedAt { get; set; }

    // Relationships
    public MedicalEquipmentType EquipmentType { get; set; } = null!;
    public Facility Facility { get; set; } = null!;
}
