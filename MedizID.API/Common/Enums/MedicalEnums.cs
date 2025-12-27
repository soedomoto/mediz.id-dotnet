namespace MedizID.API.Common.Enums;

// Laboratory Test Enums
public enum LaboratoriumCategoryEnum
{
    Diagnostik = 1,
    Hematologi = 2,
    KimiaKlinik = 3,
    LainLain = 4,
    Imunologi = 5,
    Mikrobiologi = 6,
    Serologi = 7,
    Pathologi = 8
}

public enum LaboratoriumStatusEnum
{
    Normal = 1,
    Abnormal = 2,
    Critical = 3,
    Pending = 4,
    Inconclusive = 5,
    NotPerformed = 6
}

public enum SampleTypeEnum
{
    Blood = 1,
    Serum = 2,
    Plasma = 3,
    Urine = 4,
    Saliva = 5,
    CerebrospinalFluid = 6,
    Sputum = 7,
    Stool = 8,
    Tissue = 9,
    Other = 10
}

// HIV Counseling Enums
public enum HIVCounselingVisitStatusEnum
{
    SelfVisit,
    Referred
}

public enum HIVCounselingPregnancyStatusEnum
{
    Trimester1,
    Trimester2,
    Trimester3,
    NotPregnant,
    Unknown
}

public enum HIVCounselingRiskGroupEnum
{
    PsDirect,
    PsIndirect,
    PsCustomer,
    Transgender,
    RiskPartner,
    DrugUser,
    Other,
    GayMsm
}

public enum HIVCounselingPartnerHIVStatusEnum
{
    Positive,
    Negative,
    Unknown
}

public enum HIVCounselingClientStatusEnum
{
    New,
    Returning
}

public enum HIVCounselingTestReasonEnum
{
    Curious,
    FeelAtRisk,
    FreeTest,
    RetestWindow,
    ForWork,
    Other,
    Symptoms,
    Marriage
}

public enum HIVCounselingTestSourceEnum
{
    Brochure,
    Newspaper,
    TV,
    HealthWorker,
    Friend,
    OutreachWorker,
    Poster,
    LayCounselor,
    Other
}

public enum HIVCounselingPreviousTestResultEnum
{
    Reactive,
    NonReactive,
    Unknown
}

// STI Enums
public enum STIVisitStatusEnum
{
    DatangSendiri,
    Dirujuk
}

public enum STIRiskGroupEnum
{
    WPS,
    PPS,
    Waria,
    LSL,
    Penasun,
    PasanganRisti,
    PelangganPS,
    LainLain
}

public enum STIVisitReasonEnum
{
    Penapisan,
    Sakit
}

public enum STIPregnancyStatusEnum
{
    Hamil,
    TidakHamil
}

public enum AppointmentStatusEnum
{
    Scheduled,
    Completed,
    Cancelled,
    NoShow
}

// Diagnosis Enums - ICD-10 Standard
public enum DiagnosisTypeEnum
{
    Primary,
    Secondary,
    Complication
}

public enum DiagnosisCaseTypeEnum
{
    New,
    Existing
}

