namespace MedizID.API.Common.Enums;

using System.ComponentModel.DataAnnotations;

/// <summary>
/// Vaccine types enum
/// </summary>
public enum VaccineType
{
    [Display(Name = "BCG")]
    BCG = 1,
    
    [Display(Name = "CAMPAK-1")]
    Measles1 = 2,
    
    [Display(Name = "CAMPAK-2")]
    Measles2 = 3,
    
    [Display(Name = "DPT-HB-Hib 1")]
    DPT_HB_Hib_1 = 4,
    
    [Display(Name = "DPT-HB-Hib 2")]
    DPT_HB_Hib_2 = 5,
    
    [Display(Name = "DPT-HB-Hib 3")]
    DPT_HB_Hib_3 = 6,
    
    [Display(Name = "DPT-HB-Hib 4")]
    DPT_HB_Hib_4 = 7,
    
    [Display(Name = "DTP-1")]
    DTP_1 = 8,
    
    [Display(Name = "DTP-2")]
    DTP_2 = 9,
    
    [Display(Name = "DTP-3")]
    DTP_3 = 10,
    
    [Display(Name = "DTP-4")]
    DTP_4 = 11,
    
    [Display(Name = "HEPATITIS A-1")]
    HepatitisA_1 = 12,
    
    [Display(Name = "HEPATITIS A-2")]
    HepatitisA_2 = 13,
    
    [Display(Name = "HEPATITIS B-0")]
    HepatitisB_0 = 14,
    
    [Display(Name = "HEPATITIS B-1")]
    HepatitisB_1 = 15,
    
    [Display(Name = "HEPATITIS B-2")]
    HepatitisB_2 = 16,
    
    [Display(Name = "HEPATITIS B-3")]
    HepatitisB_3 = 17,
    
    [Display(Name = "HiB-1")]
    Hib_1 = 18,
    
    [Display(Name = "HiB-2")]
    Hib_2 = 19,
    
    [Display(Name = "HiB-3")]
    Hib_3 = 20,
    
    [Display(Name = "HiB-4")]
    Hib_4 = 21,
    
    [Display(Name = "INFLUENZA")]
    Influenza = 22,
    
    [Display(Name = "IPV")]
    IPV = 23,
    
    [Display(Name = "MMR-1")]
    MMR_1 = 24,
    
    [Display(Name = "MMR-2")]
    MMR_2 = 25,
    
    [Display(Name = "MR")]
    MR = 26,
    
    [Display(Name = "MR-2")]
    MR_2 = 27,
    
    [Display(Name = "OPV-1")]
    OPV_1 = 28,
    
    [Display(Name = "OPV-2")]
    OPV_2 = 29,
    
    [Display(Name = "OPV-3")]
    OPV_3 = 30,
    
    [Display(Name = "OPV-4")]
    OPV_4 = 31,
    
    [Display(Name = "PCV-1")]
    PCV_1 = 32,
    
    [Display(Name = "PCV-2")]
    PCV_2 = 33,
    
    [Display(Name = "PCV-3")]
    PCV_3 = 34,
    
    [Display(Name = "PCV Booster")]
    PCV_Booster = 35,
    
    [Display(Name = "Rotavirus-1")]
    Rotavirus_1 = 36,
    
    [Display(Name = "Rotavirus-2")]
    Rotavirus_2 = 37,
    
    [Display(Name = "Rotavirus-3")]
    Rotavirus_3 = 38,
    
    [Display(Name = "Japanese Encephalitis")]
    JapaneseEncephalitis = 39,
    
    [Display(Name = "Typhoid")]
    Typhoid = 40,
    
    [Display(Name = "Varicella")]
    Varicella = 41,
    
    [Display(Name = "Yellow Fever")]
    YellowFever = 42
}

/// <summary>
/// Vaccine administration route enum
/// </summary>
public enum VaccineRoute
{
    [Display(Name = "Intramuscular")]
    Intramuscular = 1,
    
    [Display(Name = "Subcutaneous")]
    Subcutaneous = 2,
    
    [Display(Name = "Intradermal")]
    Intradermal = 3,
    
    [Display(Name = "Oral")]
    Oral = 4,
    
    [Display(Name = "Intravenous")]
    Intravenous = 5
}

/// <summary>
/// Vaccine administration site enum
/// </summary>
public enum VaccineSite
{
    [Display(Name = "Right Upper Arm")]
    RightUpperArm = 1,
    
    [Display(Name = "Left Upper Arm")]
    LeftUpperArm = 2,
    
    [Display(Name = "Right Thigh")]
    RightThigh = 3,
    
    [Display(Name = "Left Thigh")]
    LeftThigh = 4,
    
    [Display(Name = "Right Buttock")]
    RightButtock = 5,
    
    [Display(Name = "Left Buttock")]
    LeftButtock = 6
}

/// <summary>
/// Immunization Age Category enum
/// </summary>
public enum ImmunizationAgeCategory
{
    [Display(Name = "Newborn")]
    Newborn = 1,
    
    [Display(Name = "Infant")]
    Infant = 2,
    
    [Display(Name = "Toddler")]
    Toddler = 3,
    
    [Display(Name = "Preschool")]
    Preschool = 4,
    
    [Display(Name = "School Age")]
    SchoolAge = 5,
    
    [Display(Name = "Adolescent")]
    Adolescent = 6,
    
    [Display(Name = "Adult")]
    Adult = 7,
    
    [Display(Name = "Elderly")]
    Elderly = 8
}

/// <summary>
/// Vaccine reaction severity enum
/// </summary>
public enum VaccineReactionSeverity
{
    [Display(Name = "None")]
    None = 1,
    
    [Display(Name = "Mild")]
    Mild = 2,
    
    [Display(Name = "Moderate")]
    Moderate = 3,
    
    [Display(Name = "Severe")]
    Severe = 4
}

/// <summary>
/// Delivery type enum
/// </summary>
public enum DeliveryType
{
    [Display(Name = "Normal")]
    Normal = 1,
    
    [Display(Name = "Kelainan Letak")]
    AbnormalPresentation = 2,
    
    [Display(Name = "CPD")]
    CPD = 3,
    
    [Display(Name = "Cacat Bawaan")]
    CongenitalAbnormality = 4,
    
    [Display(Name = "Caesar")]
    Cesarean = 5
}

/// <summary>
/// Person who conducted delivery enum
/// </summary>
public enum DeliveryPersonnel
{
    [Display(Name = "Dokter")]
    Doctor = 1,
    
    [Display(Name = "Bidan")]
    Midwife = 2,
    
    [Display(Name = "Tenaga Medis (selain Bidan)")]
    MedicalStaff = 3,
    
    [Display(Name = "Dukun Terlatih")]
    TrainedTraditionalBirth = 4,
    
    [Display(Name = "Tenaga Tak Terlatih")]
    Untrained = 5
}

/// <summary>
/// Place of delivery enum
/// </summary>
public enum DeliveryPlace
{
    [Display(Name = "Rumah")]
    Home = 1,
    
    [Display(Name = "Rumah Sakit")]
    Hospital = 2,
    
    [Display(Name = "Puskesmas")]
    HealthCenter = 3,
    
    [Display(Name = "R. Bersalin")]
    BirthClinic = 4,
    
    [Display(Name = "R. Bidan")]
    MidwifeClinic = 5,
    
    [Display(Name = "Klinik")]
    Clinic = 6
}

/// <summary>
/// Breastfeeding status enum
/// </summary>
public enum BreastfeedingStatus
{
    [Display(Name = "ASI")]
    BreastMilk = 1,
    
    [Display(Name = "Bukan ASI")]
    NotBreastMilk = 2
}

/// <summary>
/// Vitamin A status enum
/// </summary>
public enum VitaminAStatus
{
    [Display(Name = "Ya")]
    Yes = 1,
    
    [Display(Name = "Tidak")]
    No = 2
}

/// <summary>
/// Neonatal visit time period enum
/// </summary>
public enum NeonatalVisitPeriod
{
    [Display(Name = "Lahir - 5 Jam")]
    BirthTo5Hours = 1,
    
    [Display(Name = "6 - 48 Jam")]
    SixTo48Hours = 2,
    
    [Display(Name = "Hari ke 3-7")]
    Day3To7 = 3,
    
    [Display(Name = "Hari ke 8-28")]
    Day8To28 = 4
}
