namespace MedizID.API.Common.Enums;

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

