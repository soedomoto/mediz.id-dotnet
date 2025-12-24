namespace MedizID.API.Common.Enums;

/// <summary>
/// Enums for Antenatal Care Observation (Pengamatan Kehamilan)
/// </summary>
/// 
public enum DeliveryAssistantTypeEnum
{
    Doctor = 1,
    Midwife = 2,
    Nurse = 3,
    TraditionalBirthAttendant = 4,
    Family = 5,
    Other = 6
}

public enum DeliveryPlaceEnum
{
    Hospital = 1,
    ClinicMaternityUnit = 2,
    Posyandu = 3,
    FamilyHome = 4,
    TraditionalBirthAttendantHome = 5,
    Other = 6
}

public enum CompanionTypeEnum
{
    Spouse = 1,
    Mother = 2,
    MothersInLaw = 3,
    Sister = 4,
    Other = 5,
    Nobody = 6
}

public enum TransportationTypeEnum
{
    OwnVehicle = 1,
    PublicTransport = 2,
    Ambulance = 3,
    Motorcycle = 4,
    Bicycle = 5,
    OnFoot = 6,
    Other = 7
}

public enum BloodDonorStatusEnum
{
    FamilyMember = 1,
    Friend = 2,
    NotIdentified = 3,
    HasDonor = 4,
    NoDonor = 5
}

public enum PregnancyRiskAssessmentEnum
{
    LowRisk = 1,
    ModerateRisk = 2,
    HighRisk = 3
}

public enum KsurRiskScoreEnum
{
    Score2 = 2,
    Score4 = 4,
    Score8 = 8,
    Score10 = 10,
    Score12Plus = 12
}

public enum KsurRiskCategoryEnum
{
    KeyahananRisikoBiasa = 0, // Normal Pregnancy Risk
    KeyahananRisikoRenda = 1, // Low Pregnancy Risk (KRR)
    KeyahananRisikoCukup = 2, // Moderate Pregnancy Risk
    KeyahananRisikoTinggi = 3  // High Pregnancy Risk (KRT)
}

public enum CasuisticRiskTypeEnum
{
    None = 0,
    MaternalAge = 1,
    MaternalHeight = 2,
    PreviousCesareanSection = 3,
    PreviousMiscarriage = 4,
    Anemia = 5,
    HighBloodPressure = 6,
    Diabetes = 7,
    MultipleBirth = 8,
    AbnormalPosition = 9,
    Antepartum = 10,
    Other = 11
}

public enum KiaBookStatusEnum
{
    Available = 1,
    Lost = 2,
    NotProvided = 3
}
