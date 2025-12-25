namespace MedizID.API.DTOs;

public class CreateMaternalChildHealthRequest
{
    public Guid AppointmentId { get; set; }
    public string? PregnancyWeeks { get; set; }
    public string? BloodPressure { get; set; }
    public decimal? Weight { get; set; }
    public string? FetalHeartRate { get; set; }
    public string? Notes { get; set; }
}

public class MaternalChildHealthResponse
{
    public Guid Id { get; set; }
    public string? PregnancyWeeks { get; set; }
    public string? BloodPressure { get; set; }
    public decimal? Weight { get; set; }
    public string? FetalHeartRate { get; set; }
    public DateTime CreatedAt { get; set; }
}

// Antenatal Examination DTOs
public class CreateAntenatalExaminationRequest
{
    public Guid AppointmentId { get; set; }
    public int? PregnancyWeeks { get; set; }
    public int? Trimester { get; set; }
    public string? FetalHeartRate { get; set; }
    public string? HeadPosition { get; set; }
    public double? EstimatedFetalWeight { get; set; }
    public string? Presentation { get; set; }
    public int? FetalCount { get; set; }
    public string? ClinicalHistory { get; set; }
    public double? Height { get; set; }
    public double? Weight { get; set; }
    public string? BloodPressure { get; set; }
    public double? UpperArmCircumference { get; set; }
    public string? NutritionStatus { get; set; }
    public double? FundusHeight { get; set; }
    public string? PatellaReflex { get; set; }
    public double? Hemoglobin { get; set; }
    public string? Anemia { get; set; }
    public int? ProteinUrine { get; set; }
    public int? UrineReducingSubstances { get; set; }
    public int? BloodGlucose { get; set; }
}

public class UpdateAntenatalExaminationRequest
{
    public int? PregnancyWeeks { get; set; }
    public int? Trimester { get; set; }
    public string? FetalHeartRate { get; set; }
    public string? HeadPosition { get; set; }
    public double? EstimatedFetalWeight { get; set; }
    public string? Presentation { get; set; }
    public int? FetalCount { get; set; }
    public string? ClinicalHistory { get; set; }
    public double? Height { get; set; }
    public double? Weight { get; set; }
    public string? BloodPressure { get; set; }
    public double? UpperArmCircumference { get; set; }
    public string? NutritionStatus { get; set; }
    public double? FundusHeight { get; set; }
    public string? PatellaReflex { get; set; }
    public double? Hemoglobin { get; set; }
    public string? Anemia { get; set; }
    public int? ProteinUrine { get; set; }
    public int? UrineReducingSubstances { get; set; }
    public int? BloodGlucose { get; set; }
}

public class AntenatalExaminationResponse
{
    public Guid Id { get; set; }
    public Guid AppointmentId { get; set; }
    public int? PregnancyWeeks { get; set; }
    public int? Trimester { get; set; }
    public string? FetalHeartRate { get; set; }
    public string? HeadPosition { get; set; }
    public double? EstimatedFetalWeight { get; set; }
    public string? Presentation { get; set; }
    public int? FetalCount { get; set; }
    public string? ClinicalHistory { get; set; }
    public double? Height { get; set; }
    public double? Weight { get; set; }
    public string? BloodPressure { get; set; }
    public double? UpperArmCircumference { get; set; }
    public string? NutritionStatus { get; set; }
    public double? FundusHeight { get; set; }
    public string? PatellaReflex { get; set; }
    public double? Hemoglobin { get; set; }
    public string? Anemia { get; set; }
    public int? ProteinUrine { get; set; }
    public int? UrineReducingSubstances { get; set; }
    public int? BloodGlucose { get; set; }
    public DateTimeOffset CreatedAt { get; set; }
    public DateTimeOffset? UpdatedAt { get; set; }
}

// Delivery Observation DTOs
public class CreateDeliveryObservationRequest
{
    public Guid AppointmentId { get; set; }
    public int? GestationalAge { get; set; }
    public int? GestationalAgeFromLmp { get; set; }
    public string? MaternalCondition { get; set; }
    public string? MaternalDischargingCondition { get; set; }
    public string? NeonatalCondition { get; set; }
    public double? NeonatalWeight { get; set; }
    public string? Presentation { get; set; }
    public string? DeliveryLocation { get; set; }
    public string? BirthAttendant { get; set; }
    public string? DeliveryMode { get; set; }
    public bool? OxytocinAdministered { get; set; }
    public bool? ControlledCordTraction { get; set; }
    public bool? UterineMassage { get; set; }
    public bool? BloodTransfusion { get; set; }
    public bool? AntibioticTherapy { get; set; }
    public bool? NeonatalResuscitation { get; set; }
    public string? ProgramIntegration { get; set; }
    public string? AntiretroviralProphylaxis { get; set; }
    public bool? HasComplications { get; set; }
    public string? ComplicationsDescription { get; set; }
    public bool? WasReferred { get; set; }
    public string? ReferralDestination { get; set; }
    public string? MaternalConditionAtReferral { get; set; }
    public string? DeliveryAddress { get; set; }
}

public class UpdateDeliveryObservationRequest
{
    public int? GestationalAge { get; set; }
    public int? GestationalAgeFromLmp { get; set; }
    public string? MaternalCondition { get; set; }
    public string? MaternalDischargingCondition { get; set; }
    public string? NeonatalCondition { get; set; }
    public double? NeonatalWeight { get; set; }
    public string? Presentation { get; set; }
    public string? DeliveryLocation { get; set; }
    public string? BirthAttendant { get; set; }
    public string? DeliveryMode { get; set; }
    public bool? OxytocinAdministered { get; set; }
    public bool? ControlledCordTraction { get; set; }
    public bool? UterineMassage { get; set; }
    public bool? BloodTransfusion { get; set; }
    public bool? AntibioticTherapy { get; set; }
    public bool? NeonatalResuscitation { get; set; }
    public string? ProgramIntegration { get; set; }
    public string? AntiretroviralProphylaxis { get; set; }
    public bool? HasComplications { get; set; }
    public string? ComplicationsDescription { get; set; }
    public bool? WasReferred { get; set; }
    public string? ReferralDestination { get; set; }
    public string? MaternalConditionAtReferral { get; set; }
    public string? DeliveryAddress { get; set; }
}

public class DeliveryObservationResponse
{
    public Guid Id { get; set; }
    public Guid AppointmentId { get; set; }
    public int? GestationalAge { get; set; }
    public int? GestationalAgeFromLmp { get; set; }
    public string? MaternalCondition { get; set; }
    public string? MaternalDischargingCondition { get; set; }
    public string? NeonatalCondition { get; set; }
    public double? NeonatalWeight { get; set; }
    public string? Presentation { get; set; }
    public string? DeliveryLocation { get; set; }
    public string? BirthAttendant { get; set; }
    public string? DeliveryMode { get; set; }
    public bool? OxytocinAdministered { get; set; }
    public bool? ControlledCordTraction { get; set; }
    public bool? UterineMassage { get; set; }
    public bool? BloodTransfusion { get; set; }
    public bool? AntibioticTherapy { get; set; }
    public bool? NeonatalResuscitation { get; set; }
    public string? ProgramIntegration { get; set; }
    public string? AntiretroviralProphylaxis { get; set; }
    public bool? HasComplications { get; set; }
    public string? ComplicationsDescription { get; set; }
    public bool? WasReferred { get; set; }
    public string? ReferralDestination { get; set; }
    public string? MaternalConditionAtReferral { get; set; }
    public string? DeliveryAddress { get; set; }
    public DateTimeOffset CreatedAt { get; set; }
    public DateTimeOffset? UpdatedAt { get; set; }
}

// Postpartum Observation DTOs
public class CreatePostpartumObservationRequest
{
    public Guid AppointmentId { get; set; }
    public DateTimeOffset? PncDate { get; set; }
    public string? BloodPressure { get; set; }
    public double? Temperature { get; set; }
    public string? Complications { get; set; }
    public string? RespiratoryRate { get; set; }
    public string? PulseRate { get; set; }
    public string? VaginalBleeding { get; set; }
    public string? PerinealCondition { get; set; }
    public string? InfectionSigns { get; set; }
    public string? UterineContraction { get; set; }
    public string? BirthCanalExamination { get; set; }
    public string? BreastExamination { get; set; }
    public string? MilkProduction { get; set; }
    public string? HighRiskComplicationManagement { get; set; }
    public string? BowelMovements { get; set; }
    public string? Urination { get; set; }
    public int? PostpartumDay { get; set; }
    public string? RecordedInKiaBook { get; set; }
    public string? IronSupplementation { get; set; }
    public string? VitaminA { get; set; }
    public string? ReferralDestination { get; set; }
    public string? ArtStatus { get; set; }
    public string? AntiMalariaInfo { get; set; }
    public string? AntiTbcInfo { get; set; }
    public string? ThoraxPhotoStatus { get; set; }
    public string? Cd4IfComplications { get; set; }
    public string? ConditionAtArrival { get; set; }
    public string? ConditionAtDischarge { get; set; }
    public string? ContraceptionMethod { get; set; }
    public DateTimeOffset? ContraceptionPlannedDate { get; set; }
    public DateTimeOffset? ContraceptionImplementationDate { get; set; }
}

public class UpdatePostpartumObservationRequest
{
    public DateTimeOffset? PncDate { get; set; }
    public string? BloodPressure { get; set; }
    public double? Temperature { get; set; }
    public string? Complications { get; set; }
    public string? RespiratoryRate { get; set; }
    public string? PulseRate { get; set; }
    public string? VaginalBleeding { get; set; }
    public string? PerinealCondition { get; set; }
    public string? InfectionSigns { get; set; }
    public string? UterineContraction { get; set; }
    public string? BirthCanalExamination { get; set; }
    public string? BreastExamination { get; set; }
    public string? MilkProduction { get; set; }
    public string? HighRiskComplicationManagement { get; set; }
    public string? BowelMovements { get; set; }
    public string? Urination { get; set; }
    public int? PostpartumDay { get; set; }
    public string? RecordedInKiaBook { get; set; }
    public string? IronSupplementation { get; set; }
    public string? VitaminA { get; set; }
    public string? ReferralDestination { get; set; }
    public string? ArtStatus { get; set; }
    public string? AntiMalariaInfo { get; set; }
    public string? AntiTbcInfo { get; set; }
    public string? ThoraxPhotoStatus { get; set; }
    public string? Cd4IfComplications { get; set; }
    public string? ConditionAtArrival { get; set; }
    public string? ConditionAtDischarge { get; set; }
    public string? ContraceptionMethod { get; set; }
    public DateTimeOffset? ContraceptionPlannedDate { get; set; }
    public DateTimeOffset? ContraceptionImplementationDate { get; set; }
}

public class PostpartumObservationResponse
{
    public Guid Id { get; set; }
    public Guid AppointmentId { get; set; }
    public DateTimeOffset? PncDate { get; set; }
    public string? BloodPressure { get; set; }
    public double? Temperature { get; set; }
    public string? Complications { get; set; }
    public string? RespiratoryRate { get; set; }
    public string? PulseRate { get; set; }
    public string? VaginalBleeding { get; set; }
    public string? PerinealCondition { get; set; }
    public string? InfectionSigns { get; set; }
    public string? UterineContraction { get; set; }
    public string? BirthCanalExamination { get; set; }
    public string? BreastExamination { get; set; }
    public string? MilkProduction { get; set; }
    public string? HighRiskComplicationManagement { get; set; }
    public string? BowelMovements { get; set; }
    public string? Urination { get; set; }
    public int? PostpartumDay { get; set; }
    public string? RecordedInKiaBook { get; set; }
    public string? IronSupplementation { get; set; }
    public string? VitaminA { get; set; }
    public string? ReferralDestination { get; set; }
    public string? ArtStatus { get; set; }
    public string? AntiMalariaInfo { get; set; }
    public string? AntiTbcInfo { get; set; }
    public string? ThoraxPhotoStatus { get; set; }
    public string? Cd4IfComplications { get; set; }
    public string? ConditionAtArrival { get; set; }
    public string? ConditionAtDischarge { get; set; }
    public string? ContraceptionMethod { get; set; }
    public DateTimeOffset? ContraceptionPlannedDate { get; set; }
    public DateTimeOffset? ContraceptionImplementationDate { get; set; }
    public DateTimeOffset CreatedAt { get; set; }
    public DateTimeOffset? UpdatedAt { get; set; }
}

// Partograph DTOs
public class CreatePartographRequest
{
    public Guid AppointmentId { get; set; }
    public DateTimeOffset? AdmissionDate { get; set; }
    public string? AdmissionTime { get; set; }
    public DateTimeOffset? OnsetOfLaborDate { get; set; }
    public string? OnsetOfLaborTime { get; set; }
    public DateTimeOffset? RuptureOfMembranesDate { get; set; }
    public string? RuptureOfMembranesTime { get; set; }
    public string? CervicalDilation { get; set; }
    public string? CervicalEffacement { get; set; }
    public string? FetalDescent { get; set; }
    public string? Molding { get; set; }
    public string? FetalHeartRateReadings { get; set; }
    public string? AmnioticFluidStatus { get; set; }
    public string? MoldingMonitoring { get; set; }
    public string? PulseRateReadings { get; set; }
    public string? BloodPressureReadings { get; set; }
    public string? TemperatureReadings { get; set; }
    public string? UrineOutput { get; set; }
    public string? OxytocinAdministration { get; set; }
    public string? IVFluidAdministration { get; set; }
    public string? OtherMedications { get; set; }
    public string? LaborNotes { get; set; }
    public string? Complications { get; set; }
    public string? ComplicationActions { get; set; }
    public DateTimeOffset? DeliveryDateTime { get; set; }
    public string? PostpartumMaternalCondition { get; set; }
    public string? PlacentaDelivery { get; set; }
    public string? ThirdStageDuration { get; set; }
    public int? MaternalHemorrhageEstimate { get; set; }
    public string? PerinealCondition { get; set; }
    public string? BladderStatus { get; set; }
    public string? UterineContractionStatus { get; set; }
}

public class UpdatePartographRequest
{
    public DateTimeOffset? AdmissionDate { get; set; }
    public string? AdmissionTime { get; set; }
    public DateTimeOffset? OnsetOfLaborDate { get; set; }
    public string? OnsetOfLaborTime { get; set; }
    public DateTimeOffset? RuptureOfMembranesDate { get; set; }
    public string? RuptureOfMembranesTime { get; set; }
    public string? CervicalDilation { get; set; }
    public string? CervicalEffacement { get; set; }
    public string? FetalDescent { get; set; }
    public string? Molding { get; set; }
    public string? FetalHeartRateReadings { get; set; }
    public string? AmnioticFluidStatus { get; set; }
    public string? MoldingMonitoring { get; set; }
    public string? PulseRateReadings { get; set; }
    public string? BloodPressureReadings { get; set; }
    public string? TemperatureReadings { get; set; }
    public string? UrineOutput { get; set; }
    public string? OxytocinAdministration { get; set; }
    public string? IVFluidAdministration { get; set; }
    public string? OtherMedications { get; set; }
    public string? LaborNotes { get; set; }
    public string? Complications { get; set; }
    public string? ComplicationActions { get; set; }
    public DateTimeOffset? DeliveryDateTime { get; set; }
    public string? PostpartumMaternalCondition { get; set; }
    public string? PlacentaDelivery { get; set; }
    public string? ThirdStageDuration { get; set; }
    public int? MaternalHemorrhageEstimate { get; set; }
    public string? PerinealCondition { get; set; }
    public string? BladderStatus { get; set; }
    public string? UterineContractionStatus { get; set; }
}

public class PartographResponse
{
    public Guid Id { get; set; }
    public Guid AppointmentId { get; set; }
    public DateTimeOffset? AdmissionDate { get; set; }
    public string? AdmissionTime { get; set; }
    public DateTimeOffset? OnsetOfLaborDate { get; set; }
    public string? OnsetOfLaborTime { get; set; }
    public DateTimeOffset? RuptureOfMembranesDate { get; set; }
    public string? RuptureOfMembranesTime { get; set; }
    public string? CervicalDilation { get; set; }
    public string? CervicalEffacement { get; set; }
    public string? FetalDescent { get; set; }
    public string? Molding { get; set; }
    public string? FetalHeartRateReadings { get; set; }
    public string? AmnioticFluidStatus { get; set; }
    public string? MoldingMonitoring { get; set; }
    public string? PulseRateReadings { get; set; }
    public string? BloodPressureReadings { get; set; }
    public string? TemperatureReadings { get; set; }
    public string? UrineOutput { get; set; }
    public string? OxytocinAdministration { get; set; }
    public string? IVFluidAdministration { get; set; }
    public string? OtherMedications { get; set; }
    public string? LaborNotes { get; set; }
    public string? Complications { get; set; }
    public string? ComplicationActions { get; set; }
    public DateTimeOffset? DeliveryDateTime { get; set; }
    public string? PostpartumMaternalCondition { get; set; }
    public string? PlacentaDelivery { get; set; }
    public string? ThirdStageDuration { get; set; }
    public int? MaternalHemorrhageEstimate { get; set; }
    public string? PerinealCondition { get; set; }
    public string? BladderStatus { get; set; }
    public string? UterineContractionStatus { get; set; }
    public DateTimeOffset CreatedAt { get; set; }
    public DateTimeOffset? UpdatedAt { get; set; }
}
