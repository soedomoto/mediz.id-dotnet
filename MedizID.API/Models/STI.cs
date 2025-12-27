using MedizID.API.Common.Enums;

namespace MedizID.API.Models;

public class STI
{
    public Guid Id { get; set; }
    public Guid AppointmentId { get; set; }
    
    // Data Kunjungan
    public string? MotherName { get; set; }
    public STIVisitStatusEnum? VisitStatus { get; set; }
    public string? ReferralStatus { get; set; }
    public STIRiskGroupEnum? RiskGroup { get; set; }
    public int? VisitNumber { get; set; }
    public STIVisitReasonEnum? VisitReason { get; set; }
    
    // Anamnesa
    public STIPregnancyStatusEnum? PregnancyStatus { get; set; }
    public int? LastSexualContactDaysAgo { get; set; }
    public bool? CondomLastContact { get; set; }
    public int? SexPartnerCountLastMonth { get; set; }
    public bool? CondomLastMonthContact { get; set; }
    public bool? CondomWithPartner { get; set; }
    public bool? VaginalDouching { get; set; }
    public string? OtherAnamnesisNotes { get; set; }
    
    // Pemeriksaan Fisik / Tanda Klinis
    public string? ClinicalSigns { get; set; } // JSON array of selected signs
    
    // Diagnosis
    public string? Diagnosis { get; set; } // JSON array of selected diagnoses
    
    // Rujuk Laboratorium
    public bool? LaboratoryReferral { get; set; }
    public string? LaboratoryTests { get; set; } // Comma-separated test types
    public string? LaboratoryResults { get; set; }
    
    // Treatment
    public string? Treatment { get; set; }
    public string? Partner { get; set; }
    public string? Notes { get; set; }
    
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? UpdatedAt { get; set; }

    // Relationships
    public Appointment Appointment { get; set; } = null!;
}
