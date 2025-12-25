using System.ComponentModel.DataAnnotations;

namespace MedizID.API.Models;

/// <summary>
/// Partograph (Labour Progress Chart) model for monitoring labor progress and maternal/fetal status during delivery.
/// Includes timeline, maternal condition, fetal condition, interventions, and stage IV monitoring.
/// </summary>
public class Partograph
{
    [Key]
    public Guid Id { get; set; }

    /// <summary>Foreign key to Appointment</summary>
    public Guid AppointmentId { get; set; }

    // ===== Timeline Fields =====
    /// <summary>Date of admission to facility</summary>
    public DateTimeOffset? AdmissionDate { get; set; }

    /// <summary>Time of admission to facility</summary>
    public string? AdmissionTime { get; set; }

    /// <summary>Date of onset of labor (contraction)</summary>
    public DateTimeOffset? OnsetOfLaborDate { get; set; }

    /// <summary>Time of onset of labor (contraction)</summary>
    public string? OnsetOfLaborTime { get; set; }

    /// <summary>Date of rupture of membranes</summary>
    public DateTimeOffset? RuptureOfMembranesDate { get; set; }

    /// <summary>Time of rupture of membranes</summary>
    public string? RuptureOfMembranesTime { get; set; }

    // ===== Labor Progress (Kemajuan Persalinan) =====
    /// <summary>Cervical dilation (cm) - multiple time entries stored as JSON</summary>
    public string? CervicalDilation { get; set; }

    /// <summary>Cervical effacement/thinning (%)</summary>
    public string? CervicalEffacement { get; set; }

    /// <summary>Fetal descent (station) - multiple time entries stored as JSON</summary>
    public string? FetalDescent { get; set; }

    /// <summary>Molding status</summary>
    public string? Molding { get; set; }

    // ===== Fetal Condition (Kondisi Janin) =====
    /// <summary>Fetal heart rate (bpm) - multiple time entries stored as JSON</summary>
    public string? FetalHeartRateReadings { get; set; }

    /// <summary>Amniotic fluid color/status - C(clear), T(turbid), M(meconium), B(blood)</summary>
    public string? AmnioticFluidStatus { get; set; }

    /// <summary>Molding at monitoring time - 0(no), +(present), ++(moderate), +++(severe)</summary>
    public string? MoldingMonitoring { get; set; }

    // ===== Maternal Condition (Kondisi Ibu) =====
    /// <summary>Maternal pulse rate (bpm) - multiple time entries stored as JSON</summary>
    public string? PulseRateReadings { get; set; }

    /// <summary>Blood pressure (mmHg) - multiple readings stored as JSON</summary>
    public string? BloodPressureReadings { get; set; }

    /// <summary>Temperature (°C) - multiple readings stored as JSON</summary>
    public string? TemperatureReadings { get; set; }

    /// <summary>Urine output - quantity and presence of protein/glucose</summary>
    public string? UrineOutput { get; set; }

    // ===== Medications and Fluids (Obat dan Cairan) =====
    /// <summary>Oxytocin administration - times and doses</summary>
    public string? OxytocinAdministration { get; set; }

    /// <summary>IV fluid administration - type, volume, rate</summary>
    public string? IVFluidAdministration { get; set; }

    /// <summary>Other medications administered - stored as JSON</summary>
    public string? OtherMedications { get; set; }

    // ===== Labor Notes (Catatan Persalinan) =====
    /// <summary>General notes about labor progress</summary>
    public string? LaborNotes { get; set; }

    /// <summary>Complications encountered during labor</summary>
    public string? Complications { get; set; }

    /// <summary>Actions taken for complications</summary>
    public string? ComplicationActions { get; set; }

    // ===== Stage IV Monitoring (Pemantauan Kala IV) =====
    /// <summary>Date and time of delivery</summary>
    public DateTimeOffset? DeliveryDateTime { get; set; }

    /// <summary>Immediate postpartum maternal condition</summary>
    public string? PostpartumMaternalCondition { get; set; }

    /// <summary>Placenta delivery time and status</summary>
    public string? PlacentaDelivery { get; set; }

    /// <summary>Third stage (Kala III) duration and complications</summary>
    public string? ThirdStageDuration { get; set; }

    /// <summary>Maternal hemorrhage estimation (cc)</summary>
    public int? MaternalHemorrhageEstimate { get; set; }

    /// <summary>Perineal condition - intact, laceration degree</summary>
    public string? PerinealCondition { get; set; }

    /// <summary>Bladder status - empty, distended</summary>
    public string? BladderStatus { get; set; }

    /// <summary>Uterine contraction status post-delivery</summary>
    public string? UterineContractionStatus { get; set; }

    // ===== Audit Fields =====
    public DateTimeOffset CreatedAt { get; set; } = DateTimeOffset.UtcNow;
    public DateTimeOffset? UpdatedAt { get; set; }

    // Navigation properties
    public virtual Appointment? Appointment { get; set; }
}
