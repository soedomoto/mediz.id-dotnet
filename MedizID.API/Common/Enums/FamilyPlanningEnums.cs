namespace MedizID.API.Common.Enums;

using System.ComponentModel.DataAnnotations;

/// <summary>
/// Education level enum
/// </summary>
public enum EducationLevel
{
    [Display(Name = "Tidak Tamat SD")]
    NoSchool = 1,
    
    [Display(Name = "SD")]
    Elementary = 2,
    
    [Display(Name = "SMP")]
    JuniorHighSchool = 3,
    
    [Display(Name = "SMA")]
    HighSchool = 4,
    
    [Display(Name = "Diploma")]
    Diploma = 5,
    
    [Display(Name = "S1")]
    Bachelor = 6,
    
    [Display(Name = "S2")]
    Master = 7,
    
    [Display(Name = "S3")]
    Doctorate = 8
}

/// <summary>
/// Family Planning Stage enum
/// </summary>
public enum FamilyPlanningStage
{
    [Display(Name = "Pra Usia Subur")]
    PreReproductiveAge = 1,
    
    [Display(Name = "Usia Subur")]
    ReproductiveAge = 2,
    
    [Display(Name = "Pasca Usia Subur")]
    PostReproductiveAge = 3
}

/// <summary>
/// KB Participant Status enum
/// </summary>
public enum KBParticipantStatus
{
    [Display(Name = "Baru Pertama Kali")]
    NewAcceptor = 1,
    
    [Display(Name = "KB Lama")]
    ContinuingAcceptor = 2,
    
    [Display(Name = "Pernah Memakai Alat KB Beberapa Bersaudara/Kelompok")]
    FormerAcceptor = 3,
    
    [Display(Name = "Ganti Cara KB")]
    MethodSwitcher = 4,
    
    [Display(Name = "Sedang KB")]
    CurrentAcceptor = 5
}

/// <summary>
/// Contraceptive Method enum
/// </summary>
public enum ContraceptiveMethod
{
    [Display(Name = "IUD (Alat Kontrasepsi Dalam Rahim)")]
    IUD = 1,
    
    [Display(Name = "Implan")]
    Implant = 2,
    
    [Display(Name = "Pil")]
    Pill = 3,
    
    [Display(Name = "Suntik")]
    Injectable = 4,
    
    [Display(Name = "Kondom")]
    Condom = 5,
    
    [Display(Name = "MOP (Vasektomi)")]
    MOP = 6,
    
    [Display(Name = "MOW (Tubektomi)")]
    MOW = 7,
    
    [Display(Name = "Metode Kalender")]
    CalendarMethod = 8,
    
    [Display(Name = "Senggama Terputus")]
    WithdrawalMethod = 9,
    
    [Display(Name = "Lainnya")]
    Other = 10
}

/// <summary>
/// Uterine Position enum
/// </summary>
public enum UterinePosition
{
    [Display(Name = "Retrofleksi")]
    Retroflexion = 1,
    
    [Display(Name = "Atefleksi")]
    Anteflexion = 2,
    
    [Display(Name = "Lainnya")]
    Other = 3
}

/// <summary>
/// Acceptor Type enum
/// </summary>
public enum AcceptorType
{
    [Display(Name = "Baru")]
    New = 1,
    
    [Display(Name = "Lama")]
    Existing = 2
}
