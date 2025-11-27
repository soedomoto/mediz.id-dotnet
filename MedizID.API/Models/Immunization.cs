namespace MedizID.API.Models;

public class Immunization
{
    public Guid Id { get; set; }
    public Guid MedicalRecordId { get; set; }
    public string VaccineName { get; set; } = null!;
    public DateTime VaccineDate { get; set; }
    public int? DoseNumber { get; set; }
    public string? Lot { get; set; }
    public string? Route { get; set; }
    public string? Site { get; set; }
    public string? Reactions { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    // Relationships
    public MedicalRecord MedicalRecord { get; set; } = null!;
}
