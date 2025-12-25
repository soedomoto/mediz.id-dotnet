using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MedizID.API.Models;

/// <summary>
/// Represents postpartum (nifas) observation data
/// PNC = Postnatal Care
/// Nifas = Indonesian term for postpartum period
/// </summary>
[Table("postpartum_observations")]
public class PostpartumObservation
{
    [Key]
    public Guid Id { get; set; }

    [Required]
    public Guid AppointmentId { get; set; }

    // PNC (Postnatal Care) Examination Section
    /// <summary>Date of PNC examination</summary>
    public DateTimeOffset? PncDate { get; set; }

    /// <summary>Blood pressure at examination (e.g., "116/76" mmHg)</summary>
    public string? BloodPressure { get; set; }

    /// <summary>Temperature at examination (°C)</summary>
    public double? Temperature { get; set; }

    /// <summary>Complications: PPP (Postpartum Hemorrhage), Infection, HDK, Others</summary>
    public string? Complications { get; set; }

    /// <summary>Respiratory rate (breaths/minute)</summary>
    public string? RespiratoryRate { get; set; }

    /// <summary>Pulse rate (beats/minute)</summary>
    public string? PulseRate { get; set; }

    /// <summary>Vaginal bleeding (lochia) assessment</summary>
    public string? VaginalBleeding { get; set; }

    /// <summary>Perineum condition assessment</summary>
    public string? PerinealCondition { get; set; }

    /// <summary>Signs of infection observed</summary>
    public string? InfectionSigns { get; set; }

    /// <summary>Uterine contraction assessment</summary>
    public string? UterineContraction { get; set; }

    /// <summary>Birth canal examination findings</summary>
    public string? BirthCanalExamination { get; set; }

    /// <summary>Breast examination findings</summary>
    public string? BreastExamination { get; set; }

    /// <summary>Milk production assessment</summary>
    public string? MilkProduction { get; set; }

    /// <summary>Management of high-risk complications during postpartum</summary>
    public string? HighRiskComplicationManagement { get; set; }

    /// <summary>Bowel movements (BAB) status</summary>
    public string? BowelMovements { get; set; }

    /// <summary>Urination (BAK) status</summary>
    public string? Urination { get; set; }

    /// <summary>Postpartum day number (KF = Hari ke-)</summary>
    public int? PostpartumDay { get; set; }

    /// <summary>Recorded in KIA book (Y/T)</summary>
    public string? RecordedInKiaBook { get; set; }

    /// <summary>Iron supplementation (Tab/Bottle)</summary>
    public string? IronSupplementation { get; set; }

    /// <summary>Vitamin A administration (Y/N)</summary>
    public string? VitaminA { get; set; }

    /// <summary>Referral destination: Puskesmas, RB (Birth Clinic), RSIA/RSB (Maternal Hospital), RS (Hospital), Others</summary>
    public string? ReferralDestination { get; set; }

    // Program Integration Section
    /// <summary>ART (Antiretroviral Therapy) status (+/-)</summary>
    public string? ArtStatus { get; set; }

    /// <summary>Anti-malaria medication notes</summary>
    public string? AntiMalariaInfo { get; set; }

    /// <summary>Anti-tuberculosis medication notes</summary>
    public string? AntiTbcInfo { get; set; }

    /// <summary>Thorax photo status (+/-)</summary>
    public string? ThoraxPhotoStatus { get; set; }

    /// <summary>CD4 count if complications</summary>
    public string? Cd4IfComplications { get; set; }

    /// <summary>Condition at arrival: Hidup (Alive), Mati (Deceased)</summary>
    public string? ConditionAtArrival { get; set; }

    /// <summary>Condition at discharge: Hidup (Alive), Mati (Deceased)</summary>
    public string? ConditionAtDischarge { get; set; }

    // Contraception Section
    /// <summary>Contraception method: AKDR, MOP, MOW, MAL, Kondom, Implan, Suntikan, Pil, Konseling</summary>
    public string? ContraceptionMethod { get; set; }

    /// <summary>Planned date for contraception</summary>
    public DateTimeOffset? ContraceptionPlannedDate { get; set; }

    /// <summary>Actual implementation date of contraception</summary>
    public DateTimeOffset? ContraceptionImplementationDate { get; set; }

    // Audit fields
    public DateTimeOffset CreatedAt { get; set; } = DateTimeOffset.UtcNow;
    public DateTimeOffset? UpdatedAt { get; set; }

    // Navigation
    public Appointment? Appointment { get; set; }
}
