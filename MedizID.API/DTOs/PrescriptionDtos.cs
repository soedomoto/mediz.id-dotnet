namespace MedizID.API.DTOs;

public class CreatePrescriptionRequest
{
    public Guid AppointmentId { get; set; }
    public string MedicationName { get; set; } = null!;
    public string Dosage { get; set; } = null!;
    public string Frequency { get; set; } = null!;
    public int Duration { get; set; }
    public string? Instructions { get; set; }
}

public class PrescriptionResponse
{
    public Guid Id { get; set; }
    public string MedicationName { get; set; } = null!;
    public string Dosage { get; set; } = null!;
    public string Frequency { get; set; } = null!;
    public int Duration { get; set; }
    public DateTime CreatedAt { get; set; }
}

public class PrescriptionDetailResponse
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
