namespace MedizID.API.Models;

public class HIVCounseling
{
    public Guid Id { get; set; }
    public Guid AppointmentId { get; set; }
    
    // Main Information
    public string? VisitStatus { get; set; } // DATANG SENDIRI, DIRUJUK
    public string? MotherName { get; set; }
    public string? PregnancyStatus { get; set; } // Trimester I, II, III, Tidak Hamil, Tidak Tahu
    public int? LastChildAge { get; set; }
    public int? NumberOfChildren { get; set; }
    public string? ReferralStatus { get; set; }
    
    // Risk Group
    public string? RiskGroup { get; set; } // PS LANGSUNG, PS TIDAK LANGSUNG, PELANGGAN PS, WARIA, PASANGAN RISTI, PENASUN, LAINNYA, GAY/LSL
    public DateTime? RiskGroupStartDate { get; set; }
    
    // Partner Information
    public bool? HasRegularPartner { get; set; }
    public bool? HasFemalePartner { get; set; }
    public bool? PartnerPregnant { get; set; }
    public string? PartnerName { get; set; }
    public DateTime? PartnerDateOfBirth { get; set; }
    public string? PartnerHIVStatus { get; set; } // HIV POSITIF, HIV NEGATIF, TIDAK DIKETAHUI
    public DateTime? PartnerLastTestDate { get; set; }
    
    // Special Population
    public bool? IsIncarcerated { get; set; } // WBP (Warga Binaan Permasyarakatan)
    
    // Pre-Test Counseling
    public DateTime? PreTestCounselingDate { get; set; }
    public string? ClientStatus { get; set; } // BARU, LAMA
    
    // Test Reasons (serialized as JSON)
    public string? TestReasons { get; set; }
    
    // Test Knowledge Source
    public string? TestKnowledgeSource { get; set; } // BROSUR, KORAN, TV, PETUGAS KESEHATAN, TEMAN, PETUGAS OUTREACH, POSTER, LAY KONSELOR, LAINNYA
    
    // Risk Exposure Assessment
    public bool? VaginalSexRisk { get; set; }
    public DateTime? VaginalSexRiskDate { get; set; }
    public bool? AnalSexRisk { get; set; }
    public DateTime? AnalSexRiskDate { get; set; }
    public bool? SharedNeedlesRisk { get; set; }
    public DateTime? SharedNeedlesRiskDate { get; set; }
    public bool? BloodTransfusionRisk { get; set; }
    public DateTime? BloodTransfusionRiskDate { get; set; }
    public bool? MotherToChildRisk { get; set; }
    public DateTime? MotherToChildRiskDate { get; set; }
    public string? OtherRiskDescription { get; set; }
    public DateTime? OtherRiskDate { get; set; }
    public bool? WindowPeriodRisk { get; set; }
    public DateTime? WindowPeriodRiskDate { get; set; }
    
    // Test Willingness
    public bool? WillingToTest { get; set; }
    
    // Previous Test History
    public bool? PreviouslyTested { get; set; }
    public string? PreviousTestLocation { get; set; }
    public DateTime? PreviousTestDate { get; set; }
    public string? PreviousTestResult { get; set; } // REAKTIF, NON REAKTIF, TIDAK TAHU
    
    // Notes
    public string? ObservationNotes { get; set; }
    
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    // Relationships
    public Appointment Appointment { get; set; } = null!;
}
