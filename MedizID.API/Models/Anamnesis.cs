using MedizID.API.Common.Enums;

namespace MedizID.API.Models;

public class Anamnesis
{
    public Guid Id { get; set; }
    public Guid AppointmentId { get; set; }
    
    #region Chief Complaint and Presenting Illness (Keluhan dan Riwayat Penyakit Sekarang)
    
    /// <summary>Primary complaint/chief symptom (Keluhan Utama)</summary>
    public string? ChiefComplaint { get; set; }
    
    /// <summary>Additional complaints (Keluhan Tambahan)</summary>
    public string? AdditionalComplaints { get; set; }
    
    /// <summary>Duration of illness - Years component (Lama Sakit - Tahun)</summary>
    public int? DurationYears { get; set; }
    
    /// <summary>Duration of illness - Months component (Lama Sakit - Bulan)</summary>
    public int? DurationMonths { get; set; }
    
    /// <summary>Duration of illness - Days component (Lama Sakit - Hari)</summary>
    public int? DurationDays { get; set; }
    
    /// <summary>Present illness history (Riwayat Penyakit Sekarang)</summary>
    public string? PresentIllnessHistory { get; set; }
    
    #endregion
    
    #region Medical History (Riwayat Penyakit)
    
    /// <summary>Past medical history (Riwayat Penyakit Dulu)</summary>
    public string? PastMedicalHistory { get; set; }
    
    /// <summary>Family history of illness (Riwayat Penyakit Keluarga)</summary>
    public string? FamilyHistory { get; set; }
    
    #endregion
    
    #region Allergies (Riwayat Alergi)
    
    /// <summary>Drug/medication allergies (Alergi Obat)</summary>
    public string? DrugAllergies { get; set; }
    
    /// <summary>Food allergies (Alergi Makanan)</summary>
    public string? FoodAllergies { get; set; }
    
    /// <summary>Other allergies (Alergi Lainnya)</summary>
    public string? OtherAllergies { get; set; }
    
    #endregion
    
    #region Medications and Social History (Obat dan Riwayat Sosial)
    
    /// <summary>Current medications being taken (Obat yang Sedang Diminum)</summary>
    public string? CurrentMedications { get; set; }
    
    /// <summary>Social history (Riwayat Sosial)</summary>
    public string? SocialHistory { get; set; }
    
    /// <summary>Smoking status (Merokok)</summary>
    public bool? SmokingStatus { get; set; }
    
    /// <summary>Alcohol consumption (Konsumsi Alkohol)</summary>
    public bool? AlcoholConsumption { get; set; }
    
    /// <summary>Insufficient vegetable/fruit intake (Kurang Sayur/Buah)</summary>
    public bool? InsufficientVegetableFruitIntake { get; set; }
    
    #endregion
    
    #region Physical Examination - Vital Signs (Periksa Fisik - Tanda Vital)
    
    /// <summary>General appearance (Keadaan Umum)</summary>
    public string? GeneralAppearance { get; set; }
    
    /// <summary>Consciousness level (Kesadaran)</summary>
    public ConsciousnessLevel? ConsciousnessLevel { get; set; }
    
    /// <summary>Body temperature in Celsius (Suhu Tubuh - °C)</summary>
    public decimal? BodyTemperature { get; set; }
    
    /// <summary>Respiratory rate per minute (Laju Nafas - /menit)</summary>
    public int? RespiratoryRate { get; set; }
    
    /// <summary>Heart rate/pulse rate per minute (Frekuensi Denyut Nadi - /menit)</summary>
    public int? PulseRate { get; set; }
    
    /// <summary>Heart rhythm regularity (Irama Denyut Jantung)</summary>
    public HeartRhythm? HeartRhythm { get; set; }
    
    /// <summary>Systolic blood pressure in mmHg (Tekanan Darah Sistolik - mmHg)</summary>
    public int? SystolicBloodPressure { get; set; }
    
    /// <summary>Diastolic blood pressure in mmHg (Tekanan Darah Diastolik - mmHg)</summary>
    public int? DiastolicBloodPressure { get; set; }
    
    /// <summary>Body weight in kilograms (Berat Badan - Kg)</summary>
    public decimal? BodyWeight { get; set; }
    
    /// <summary>Height in centimeters (Tinggi Badan - cm)</summary>
    public decimal? Height { get; set; }
    
    /// <summary>Height measurement method (Cara Ukur Tinggi Badan)</summary>
    public HeightMeasurementMethod? HeightMeasurementMethod { get; set; }
    
    /// <summary>Waist circumference in centimeters (Lingkar Perut - cm)</summary>
    public decimal? WaistCircumference { get; set; }
    
    /// <summary>Body mass index (Indeks Masa Tubuh)</summary>
    public decimal? BodyMassIndex { get; set; }
    
    /// <summary>BMI classification result (Hasil Interpretasi IMT)</summary>
    public string? BMIClassification { get; set; }
    
    /// <summary>Oxygen saturation percentage (Saturasi Oksigen - %)</summary>
    public decimal? OxygenSaturation { get; set; }
    
    /// <summary>Pain scale score 0-10 (Skala Nyeri)</summary>
    public int? PainScale { get; set; }
    
    #endregion
    
    #region Physical Activity and Vital Sign Details
    
    /// <summary>Physical activity level (Aktifitas Fisik)</summary>
    public string? PhysicalActivity { get; set; }
    
    #endregion
    
    #region Detailed System Examination (Pemeriksaan Fisik Sistem)
    
    /// <summary>Head examination findings (Pemeriksaan Kepala)</summary>
    public string? HeadExamination { get; set; }
    
    /// <summary>Eye examination findings (Pemeriksaan Mata)</summary>
    public string? EyeExamination { get; set; }
    
    /// <summary>Ear examination findings (Pemeriksaan Telinga)</summary>
    public string? EarExamination { get; set; }
    
    /// <summary>Nose and sinus examination findings (Pemeriksaan Hidung dan Sinus)</summary>
    public string? NoseExamination { get; set; }
    
    /// <summary>Mouth and lip examination findings (Pemeriksaan Mulut dan Bibir)</summary>
    public string? MouthExamination { get; set; }
    
    /// <summary>Throat/pharyngeal examination findings (Pemeriksaan Faring)</summary>
    public string? ThroatExamination { get; set; }
    
    /// <summary>Lung/respiratory examination findings (Pemeriksaan Paru-paru)</summary>
    public string? LungExamination { get; set; }
    
    /// <summary>Cardiovascular examination findings (Pemeriksaan Kardiovaskuler)</summary>
    public string? CardiovascularExamination { get; set; }
    
    /// <summary>Chest and axilla examination findings (Pemeriksaan Dada dan Aksila)</summary>
    public string? ChestAxillaExamination { get; set; }
    
    /// <summary>Abdominal examination findings (Pemeriksaan Abdomen)</summary>
    public string? AbdominalExamination { get; set; }
    
    /// <summary>Upper extremity examination findings (Pemeriksaan Ekstremitas Atas)</summary>
    public string? UpperExtremityExamination { get; set; }
    
    /// <summary>Lower extremity examination findings (Pemeriksaan Ekstremitas Bawah)</summary>
    public string? LowerExtremityExamination { get; set; }
    
    /// <summary>Genitalia examination findings (Pemeriksaan Genitalia)</summary>
    public string? GenitaliaExamination { get; set; }
    
    /// <summary>Skin examination findings (Pemeriksaan Kulit)</summary>
    public string? SkinExamination { get; set; }
    
    /// <summary>Nail examination findings (Pemeriksaan Kuku)</summary>
    public string? NailExamination { get; set; }
    
    /// <summary>Neck examination findings (Pemeriksaan Leher)</summary>
    public string? NeckExamination { get; set; }
    
    /// <summary>Neurological examination findings (Pemeriksaan Neurologis)</summary>
    public string? NeurologicalExamination { get; set; }
    
    #endregion
    
    #region Assessment and Triage (Penilaian dan Triase)
    
    /// <summary>Triage classification level (Tingkat Triase)</summary>
    public TriageLevel? TriageLevel { get; set; }
    
    #endregion
    
    #region Body Anatomy Findings (Anatomi Tubuh)
    
    /// <summary>Anatomical location and findings recorded on body diagram (Temuan Anatomi Tubuh)</summary>
    public string? BodyAnatomyFindings { get; set; }
    
    #endregion
    
    #region Clinical Documentation (Dokumentasi Klinis)
    
    /// <summary>Clinical notes and findings (Catatan Klinis)</summary>
    public string? ClinicalNotes { get; set; }
    
    /// <summary>Additional clinical findings (Temuan Tambahan)</summary>
    public string? AdditionalFindings { get; set; }
    
    /// <summary>Other notes/observations (Observasi Lainnya)</summary>
    public string? ObservationNotes { get; set; }
    
    /// <summary>Additional information/remarks (Keterangan Tambahan)</summary>
    public string? AdditionalRemarks { get; set; }
    
    /// <summary>Biopsychosocial assessment (Penilaian Biopsikososial)</summary>
    public string? BiopsychosocialAssessment { get; set; }
    
    #endregion
    
    #region Assessment and Plan (Asuhan dan Rencana)
    
    /// <summary>Subjective assessment (Subjektif)</summary>
    public string? SubjectiveAssessment { get; set; }
    
    /// <summary>Objective findings (Objektif)</summary>
    public string? ObjectiveFindings { get; set; }
    
    /// <summary>Assessment/diagnosis (Penilaian/Diagnosis)</summary>
    public string? Assessment { get; set; }
    
    /// <summary>Plan of care/management (Rencana Tindakan)</summary>
    public string? PlanOfCare { get; set; }
    
    /// <summary>Nursing care type (Tipe Asuhan Keperawatan)</summary>
    public NursingCareType? NursingCareType { get; set; }
    
    /// <summary>Nursing care description/implementation (Deskripsi Asuhan Keperawatan)</summary>
    public string? NursingCareDescription { get; set; }
    
    /// <summary>Nursing interventions performed (Tindakan Keperawatan)</summary>
    public string? NursingInterventions { get; set; }
    
    /// <summary>Patient education provided (Edukasi Pasien)</summary>
    public string? PatientEducation { get; set; }
    
    /// <summary>Treatment/therapy provided (Terapi)</summary>
    public string? Treatment { get; set; }
    
    #endregion
    
    #region Audit Trail (Jejak Audit)
    
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? UpdatedAt { get; set; }

    #endregion
    
    #region Relationships

    public Appointment Appointment { get; set; } = null!;

    #endregion
}
