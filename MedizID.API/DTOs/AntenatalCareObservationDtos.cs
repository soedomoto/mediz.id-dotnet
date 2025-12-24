namespace MedizID.API.DTOs;

using System.Text.Json.Serialization;

public class CreateAntenatalCareObservationRequest
{
    public Guid AppointmentId { get; set; }
    
    // Personnel Information
    public Guid? MedicalPersonnelId { get; set; }
    public string? MedicalPersonnelName { get; set; }
    public Guid? NurseId { get; set; }
    public string? NurseName { get; set; }
    public string? MaternityHealthPostName { get; set; }
    public string? CadreName { get; set; }
    public string? TraditionalBirthAttendantName { get; set; }
    
    // Patient History
    public string? ObstetricComplicationHistory { get; set; }
    public string? ChronicDiseaseAndAllergy { get; set; }
    public string? DiseaseHistory { get; set; }
    
    // Obstetric History
    public int? Gravida { get; set; }
    public int? Partus { get; set; }
    public int? Abortus { get; set; }
    public int? AliveChildren { get; set; }
    
    // Delivery Planning
    public DateTime? PlannedDeliveryDate { get; set; }
    public string? PlannedDeliveryAssistant { get; set; }
    public string? PlannedDeliveryPlace { get; set; }
    public string? PlannedCompanion { get; set; }
    public string? PlannedTransportation { get; set; }
    public string? BloodDonorStatus { get; set; }
    
    // Midwife Examination
    public DateTime? LastMenstrualPeriodDate { get; set; }
    public DateTime? EstimatedDeliveryDate { get; set; }
    public DateTime? PreviousDeliveryDate { get; set; }
    public string? KiaBookStatus { get; set; }
    public decimal? PrePregnancyWeight { get; set; }
    public decimal? Height { get; set; }
    
    // Risk Assessment
    public int? MotherKsurScore { get; set; }
    public string? PregnancyRiskCategory { get; set; }
    public string? HighRiskDescription { get; set; }
    public string? CasuisticRiskType { get; set; }
}

public class AntenatalCareObservationResponse
{
    public Guid Id { get; set; }
    public Guid AppointmentId { get; set; }
    
    // Personnel Information
    public Guid? MedicalPersonnelId { get; set; }
    public string? MedicalPersonnelName { get; set; }
    public Guid? NurseId { get; set; }
    public string? NurseName { get; set; }
    public string? MaternityHealthPostName { get; set; }
    public string? CadreName { get; set; }
    public string? TraditionalBirthAttendantName { get; set; }
    
    // Patient History
    public string? ObstetricComplicationHistory { get; set; }
    public string? ChronicDiseaseAndAllergy { get; set; }
    public string? DiseaseHistory { get; set; }
    
    // Obstetric History
    public int? Gravida { get; set; }
    public int? Partus { get; set; }
    public int? Abortus { get; set; }
    public int? AliveChildren { get; set; }
    
    // Delivery Planning
    public DateTime? PlannedDeliveryDate { get; set; }
    public string? PlannedDeliveryAssistant { get; set; }
    public string? PlannedDeliveryPlace { get; set; }
    public string? PlannedCompanion { get; set; }
    public string? PlannedTransportation { get; set; }
    public string? BloodDonorStatus { get; set; }
    
    // Midwife Examination
    public DateTime? LastMenstrualPeriodDate { get; set; }
    public DateTime? EstimatedDeliveryDate { get; set; }
    public DateTime? PreviousDeliveryDate { get; set; }
    public string? KiaBookStatus { get; set; }
    public decimal? PrePregnancyWeight { get; set; }
    public decimal? Height { get; set; }
    
    // Risk Assessment
    public int? MotherKsurScore { get; set; }
    public string? PregnancyRiskCategory { get; set; }
    public string? HighRiskDescription { get; set; }
    public string? CasuisticRiskType { get; set; }
    
    // Metadata
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
}

public class AntenatalCareObservationDetailResponse
{
    public Guid Id { get; set; }
    public Guid AppointmentId { get; set; }
    
    // Personnel Information
    public Guid? MedicalPersonnelId { get; set; }
    public string? MedicalPersonnelName { get; set; }
    public Guid? NurseId { get; set; }
    public string? NurseName { get; set; }
    public string? MaternityHealthPostName { get; set; }
    public string? CadreName { get; set; }
    public string? TraditionalBirthAttendantName { get; set; }
    
    // Patient History
    public string? ObstetricComplicationHistory { get; set; }
    public string? ChronicDiseaseAndAllergy { get; set; }
    public string? DiseaseHistory { get; set; }
    
    // Obstetric History
    public int? Gravida { get; set; }
    public int? Partus { get; set; }
    public int? Abortus { get; set; }
    public int? AliveChildren { get; set; }
    
    // Delivery Planning
    public DateTime? PlannedDeliveryDate { get; set; }
    public string? PlannedDeliveryAssistant { get; set; }
    public string? PlannedDeliveryPlace { get; set; }
    public string? PlannedCompanion { get; set; }
    public string? PlannedTransportation { get; set; }
    public string? BloodDonorStatus { get; set; }
    
    // Midwife Examination
    public DateTime? LastMenstrualPeriodDate { get; set; }
    public DateTime? EstimatedDeliveryDate { get; set; }
    public DateTime? PreviousDeliveryDate { get; set; }
    public string? KiaBookStatus { get; set; }
    public decimal? PrePregnancyWeight { get; set; }
    public decimal? Height { get; set; }
    
    // Risk Assessment
    public int? MotherKsurScore { get; set; }
    public string? PregnancyRiskCategory { get; set; }
    public string? HighRiskDescription { get; set; }
    public string? CasuisticRiskType { get; set; }
    
    // Metadata
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
}
