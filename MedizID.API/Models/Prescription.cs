namespace MedizID.API.Models;

public class Prescription
{
    public Guid Id { get; set; }
    public Guid AppointmentId { get; set; }
    public bool? IsRecommendedByAI { get; set; }
    public int? AIRecommendationConfidence { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? UpdatedAt { get; set; }

    // Relationships
    public Appointment Appointment { get; set; } = null!;
    public ICollection<PrescriptionDetail> PrescriptionDetails { get; set; } = new List<PrescriptionDetail>();
}

public class PrescriptionDetail
{
    public Guid Id { get; set; }
    public Guid PrescriptionId { get; set; }
    public Guid? DrugId { get; set; }
    public Guid? MedicalEquipmentId { get; set; }
    public string? MedicationName { get; set; }
    public string? Dosage { get; set; }
    public string? Signa { get; set; } // e.g., "2x1", "3x1"
    public string? Frequency { get; set; }
    public int? Quantity { get; set; }
    public string? Instructions { get; set; } // Aturan Pakai
    public string? Notes { get; set; } // Keterangan
    public decimal? Price { get; set; }
    public decimal? Packaging { get; set; } // Embalase
    public string? RecipeType { get; set; } // Racikan (R1, R2, etc.)
    public int? RequestedQuantity { get; set; } // Jumlah Permintaan
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? UpdatedAt { get; set; }

    // Relationships
    public Prescription Prescription { get; set; } = null!;
    public Drug? Drug { get; set; }
    public MedicalEquipment? MedicalEquipment { get; set; }
}
