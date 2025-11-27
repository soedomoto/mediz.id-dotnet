namespace MedizID.API.Models;

public class Drug
{
    public Guid Id { get; set; }
    public string GenericName { get; set; } = null!;
    public string? BrandName { get; set; }
    public string? Dosage { get; set; }
    public Guid DrugCategoryId { get; set; }
    public string? Manufacturer { get; set; }
    public string? IndividualPrice { get; set; }
    public bool IsActive { get; set; } = true;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? UpdatedAt { get; set; }

    // Relationships
    public DrugCategory Category { get; set; } = null!;
}
