using MedizID.API.Common.Enums;

namespace MedizID.API.Models;

/// <summary>
/// Model for Antenatal Care Observation (Pengamatan Kehamilan)
/// One-to-one relationship with Appointment
/// </summary>
public class AntenatalCareObservation
{
    public Guid Id { get; set; }
    public Guid AppointmentId { get; set; }
    
    // Personnel Information
    public Guid? MedicalPersonnelId { get; set; }      // Dokter / Tenaga Medis
    public string? MedicalPersonnelName { get; set; }
    public Guid? NurseId { get; set; }                  // Perawat / Bidan / Nutrisionist / Sanitarian
    public string? NurseName { get; set; }
    public string? MaternityHealthPostName { get; set; } // Posyandu
    public string? CadreName { get; set; }              // Nama Kader
    public string? TraditionalBirthAttendantName { get; set; } // Nama Dukun
    
    // Patient History
    public string? ObstetricComplicationHistory { get; set; }  // Riwayat Komplikasi Kebidanan
    public string? ChronicDiseaseAndAllergy { get; set; }      // Penyakit Kronis dan Alergi
    public string? DiseaseHistory { get; set; }                // Riwayat Penyakit
    
    // Obstetric History
    public int? Gravida { get; set; }        // Total number of pregnancies
    public int? Partus { get; set; }         // Number of deliveries
    public int? Abortus { get; set; }        // Number of miscarriages
    public int? AliveChildren { get; set; } // Number of living children
    
    // Delivery Planning (Rencana Persalinan)
    public DateTime? PlannedDeliveryDate { get; set; }
    public DeliveryAssistantTypeEnum? PlannedDeliveryAssistant { get; set; }
    public DeliveryPlaceEnum? PlannedDeliveryPlace { get; set; }
    public CompanionTypeEnum? PlannedCompanion { get; set; }
    public TransportationTypeEnum? PlannedTransportation { get; set; }
    public BloodDonorStatusEnum? BloodDonorStatus { get; set; }
    
    // Midwife Examination (Pemeriksaan Bidan)
    public DateTime? LastMenstrualPeriodDate { get; set; }      // Tanggal HPHT
    public DateTime? EstimatedDeliveryDate { get; set; }        // Taksiran Persalinan
    public DateTime? PreviousDeliveryDate { get; set; }         // Persalinan Sebelumnya
    public KiaBookStatusEnum? KiaBookStatus { get; set; }       // Status Buku KIA
    public decimal? PrePregnancyWeight { get; set; }            // BB sebelum hamil (kg)
    public decimal? Height { get; set; }                        // Tinggi Badan (cm)
    
    // Risk Assessment
    public int? MotherKsurScore { get; set; }                   // Skor Ibu (KSPR)
    public KsurRiskCategoryEnum? PregnancyRiskCategory { get; set; } // Risiko
    public string? HighRiskDescription { get; set; }            // Sebutkan Jenis Risiko Tinggi
    public CasuisticRiskTypeEnum? CasuisticRiskType { get; set; } // Risiko Kasuistik
    
    // Metadata
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? UpdatedAt { get; set; }
    
    // Relationships
    public Appointment Appointment { get; set; } = null!;
}
