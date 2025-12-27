namespace MedizID.API.DTOs;

public class CreateSTIRequest
{
    public Guid AppointmentId { get; set; }
    
    // Data Kunjungan
    public string? MotherName { get; set; }
    public string? VisitStatus { get; set; }
    public string? ReferralStatus { get; set; }
    public string? RiskGroup { get; set; }
    public int? VisitNumber { get; set; }
    public string? VisitReason { get; set; }
    
    // Anamnesa
    public string? PregnancyStatus { get; set; }
    public int? LastSexualContactDaysAgo { get; set; }
    public bool? CondomLastContact { get; set; }
    public int? SexPartnerCountLastMonth { get; set; }
    public bool? CondomLastMonthContact { get; set; }
    public bool? CondomWithPartner { get; set; }
    public bool? VaginalDouching { get; set; }
    public string? OtherAnamnesisNotes { get; set; }
    
    // Pemeriksaan Fisik
    public string? ClinicalSigns { get; set; }
    
    // Diagnosis
    public string? Diagnosis { get; set; }
    
    // Laboratorium
    public bool? LaboratoryReferral { get; set; }
    public string? LaboratoryTests { get; set; }
    public string? LaboratoryResults { get; set; }
    
    // Treatment & Follow-up
    public string? Treatment { get; set; }
    public string? Partner { get; set; }
    public string? Notes { get; set; }
}

public class UpdateSTIRequest
{
    // Data Kunjungan
    public string? MotherName { get; set; }
    public string? VisitStatus { get; set; }
    public string? ReferralStatus { get; set; }
    public string? RiskGroup { get; set; }
    public int? VisitNumber { get; set; }
    public string? VisitReason { get; set; }
    
    // Anamnesa
    public string? PregnancyStatus { get; set; }
    public int? LastSexualContactDaysAgo { get; set; }
    public bool? CondomLastContact { get; set; }
    public int? SexPartnerCountLastMonth { get; set; }
    public bool? CondomLastMonthContact { get; set; }
    public bool? CondomWithPartner { get; set; }
    public bool? VaginalDouching { get; set; }
    public string? OtherAnamnesisNotes { get; set; }
    
    // Pemeriksaan Fisik
    public string? ClinicalSigns { get; set; }
    
    // Diagnosis
    public string? Diagnosis { get; set; }
    
    // Laboratorium
    public bool? LaboratoryReferral { get; set; }
    public string? LaboratoryTests { get; set; }
    public string? LaboratoryResults { get; set; }
    
    // Treatment & Follow-up
    public string? Treatment { get; set; }
    public string? Partner { get; set; }
    public string? Notes { get; set; }
}

public class STIResponse
{
    public Guid Id { get; set; }
    public Guid AppointmentId { get; set; }
    
    // Data Kunjungan
    public string? MotherName { get; set; }
    public string? VisitStatus { get; set; }
    public string? ReferralStatus { get; set; }
    public string? RiskGroup { get; set; }
    public int? VisitNumber { get; set; }
    public string? VisitReason { get; set; }
    
    // Anamnesa
    public string? PregnancyStatus { get; set; }
    public int? LastSexualContactDaysAgo { get; set; }
    public bool? CondomLastContact { get; set; }
    public int? SexPartnerCountLastMonth { get; set; }
    public bool? CondomLastMonthContact { get; set; }
    public bool? CondomWithPartner { get; set; }
    public bool? VaginalDouching { get; set; }
    public string? OtherAnamnesisNotes { get; set; }
    
    // Pemeriksaan Fisik
    public string? ClinicalSigns { get; set; }
    
    // Diagnosis
    public string? Diagnosis { get; set; }
    
    // Laboratorium
    public bool? LaboratoryReferral { get; set; }
    public string? LaboratoryTests { get; set; }
    public string? LaboratoryResults { get; set; }
    
    // Treatment & Follow-up
    public string? Treatment { get; set; }
    public string? Partner { get; set; }
    public string? Notes { get; set; }
    
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
}

public class STIListResponse
{
    public List<STIResponse> Items { get; set; } = new();
    public int Total { get; set; }
}
