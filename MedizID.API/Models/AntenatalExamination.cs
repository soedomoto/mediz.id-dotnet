namespace MedizID.API.Models;

/// <summary>
/// Model for Antenatal Clinical Examination
/// Detailed clinical examination findings during antenatal care
/// One-to-one relationship with Appointment
/// </summary>
public class AntenatalExamination
{
    public Guid Id { get; set; }
    public Guid AppointmentId { get; set; }
    
    // Register Information
    public int? PregnancyWeeks { get; set; }           // Gestational age in weeks
    public int? Trimester { get; set; }                // Trimester number
    
    // Fetal Examination
    public string? FetalHeartRate { get; set; }        // Fetal heart rate (bpm)
    public string? HeadPosition { get; set; }          // Fetal head position relative to pelvic brim
    public double? EstimatedFetalWeight { get; set; }  // Estimated fetal weight (grams)
    public string? Presentation { get; set; }          // Fetal presentation (Cephalic, Breech, Transverse)
    public int? FetalCount { get; set; }               // Number of fetuses
    
    // Maternal Examination
    public string? ClinicalHistory { get; set; }       // Chief complaint and history of present illness
    public double? Height { get; set; }                // Maternal height (cm)
    public double? Weight { get; set; }                // Maternal weight (kg)
    public string? BloodPressure { get; set; }         // Systolic/Diastolic pressure (mmHg)
    public double? UpperArmCircumference { get; set; } // Mid-upper arm circumference (cm)
    public string? NutritionStatus { get; set; }       // Nutritional status assessment (Normal/Malnutrition)
    public double? FundusHeight { get; set; }          // Fundal height measurement (cm)
    public string? PatellaReflex { get; set; }         // Patellar reflex result (Present/Absent/Hyperactive)
    
    // Laboratory Results
    public double? Hemoglobin { get; set; }            // Hemoglobin concentration (g/dL)
    public string? Anemia { get; set; }                // Anemia status (Positive/Negative)
    public int? ProteinUrine { get; set; }             // Urinary protein level
    public int? UrineReducingSubstances { get; set; }  // Reducing substances in urine
    public int? BloodGlucose { get; set; }             // Blood glucose level (mg/dL)
    
    // Metadata
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? UpdatedAt { get; set; }
    
    // Relationships
    public Appointment Appointment { get; set; } = null!;
}
