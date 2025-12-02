using System.Text.Json.Serialization;

namespace MedizID.API.DTOs;

public class CreatePrescriptionRequest
{
    public Guid AppointmentId { get; set; }
    public List<CreatePrescriptionDetailRequest> Details { get; set; } = new();
}

public class CreatePrescriptionDetailRequest
{
    public Guid? DrugId { get; set; }
    public Guid? MedicalEquipmentId { get; set; }
    public string? MedicationName { get; set; }
    public string? Dosage { get; set; }
    public string? Signa { get; set; }
    public string? Frequency { get; set; }
    public int? Quantity { get; set; }
    public string? Instructions { get; set; }
    public string? Notes { get; set; }
    public decimal? Price { get; set; }
    public decimal? Packaging { get; set; }
    public string? RecipeType { get; set; }
    public int? RequestedQuantity { get; set; }
}

public class PrescriptionResponse
{
    public Guid Id { get; set; }
    public Guid AppointmentId { get; set; }
    public bool? IsRecommendedByAI { get; set; }
    public int? AIRecommendationConfidence { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
    public List<PrescriptionDetailResponse> Details { get; set; } = new();
}

public class PrescriptionDetailResponse
{
    public Guid Id { get; set; }
    public Guid PrescriptionId { get; set; }
    public Guid? DrugId { get; set; }
    public Guid? MedicalEquipmentId { get; set; }
    public string? MedicationName { get; set; }
    public string? Dosage { get; set; }
    public string? Signa { get; set; }
    public string? Frequency { get; set; }
    public int? Quantity { get; set; }
    public string? Instructions { get; set; }
    public string? Notes { get; set; }
    public decimal? Price { get; set; }
    public decimal? Packaging { get; set; }
    public string? RecipeType { get; set; }
    public int? RequestedQuantity { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
}

public class PrescriptionDetailResponse_Old
{
    public Guid Id { get; set; }
    public string MedicationName { get; set; } = null!;
    public string Dosage { get; set; } = null!;
    public string Frequency { get; set; } = null!;
    public int Duration { get; set; }
    public string? Instructions { get; set; }
    public Guid AppointmentId { get; set; }
    public DateTime CreatedAt { get; set; }
}

public class PatientPrescriptionHistoryResponse
{
    public Guid PatientId { get; set; }
    public List<PrescriptionHistoryItem> Prescriptions { get; set; } = new();
}

public class PrescriptionHistoryItem
{
    public Guid Id { get; set; }
    public string MedicationName { get; set; } = null!;
    public string Dosage { get; set; } = null!;
    public string Frequency { get; set; } = null!;
    public DateTime PrescribedDate { get; set; }
    public int DurationDays { get; set; }
}

public class AddPrescriptionEquipmentRequest
{
    public Guid EquipmentId { get; set; }
}

public class DispensePrescriptionRequest
{
    public DateTime DispenseDate { get; set; }
    public string? Notes { get; set; }
}

public class PatientPrescriptionPublic
{
    public Guid Id { get; set; }
    public string MedicationName { get; set; } = null!;
    public string Dosage { get; set; } = null!;
    public string Frequency { get; set; } = null!;
    public int Duration { get; set; }
    public DateTime CreatedAt { get; set; }
}

public class PatientPrescriptionDetail
{
    public Guid Id { get; set; }
    public Guid MedicalRecordId { get; set; }
    public string MedicationName { get; set; } = null!;
    public string Dosage { get; set; } = null!;
    public string Frequency { get; set; } = null!;
    public int Duration { get; set; }
    public string? Instructions { get; set; }
    public DateTime? ExpiryDate { get; set; }
    public DateTime CreatedAt { get; set; }
}

public class PrescriptionStatisticsRequest
{
    public DateTime? StartDate { get; set; }
    public DateTime? EndDate { get; set; }
}

public class PrescriptionStatisticsResponse
{
    public int TotalPrescriptions { get; set; }
    public int TotalDispensed { get; set; }
    public int TotalPending { get; set; }
    public int MostCommonMedications { get; set; }
    public List<string> TopDrugs { get; set; } = new();
}

public class PrescriptionItemDetailResponse
{
    public Guid Id { get; set; }
    public string? MedicationName { get; set; }
    public string? Dosage { get; set; }
    public string? Signa { get; set; }
    public string? Frequency { get; set; }
    public int? Quantity { get; set; }
    public string? Instructions { get; set; }
    public string? Notes { get; set; }
    public decimal? Price { get; set; }
    public decimal? Packaging { get; set; }
    public string? RecipeType { get; set; }
    public int? RequestedQuantity { get; set; }
    public int? ConfidenceScore { get; set; }
}

public class PrescriptionRecommendationItem
{
    [JsonPropertyName("medicationName")]
    public string MedicationName { get; set; } = null!;
    [JsonPropertyName("dosage")]
    public string Dosage { get; set; } = null!;
    [JsonPropertyName("signa")]
    public string Signa { get; set; } = null!;
    [JsonPropertyName("frequency")]
    public string Frequency { get; set; } = null!;
    [JsonPropertyName("quantity")]
    public int Quantity { get; set; }
    [JsonPropertyName("instructions")]
    public string? Instructions { get; set; }
    [JsonPropertyName("notes")]
    public string? Notes { get; set; }
    [JsonPropertyName("confidenceScore")]
    public int ConfidenceScore { get; set; }
}

public class PrescriptionRecommendationResponse
{
    [JsonPropertyName("recommendations")]
    public List<PrescriptionRecommendationItem>? Recommendations { get; set; }
}

