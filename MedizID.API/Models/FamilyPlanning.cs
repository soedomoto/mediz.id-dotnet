namespace MedizID.API.Models;

using MedizID.API.Common.Enums;

/// <summary>
/// Family Planning (Keluarga Berencana) model for contraceptive counseling and services
/// Based on Indonesian health standards (Form Akseptor)
/// </summary>
public class FamilyPlanning
{
    #region Primary Keys and Foreign Keys
    
    public Guid Id { get; set; }
    public Guid AppointmentId { get; set; }
    
    #endregion

    #region Section VI-VII: Spouse Information and Education
    
    /// <summary>Spouse/Partner name (Nama Suami/Istri)</summary>
    public string? SpouseName { get; set; }
    
    /// <summary>Husband education level (Pendidikan Suami)</summary>
    public EducationLevel? HusbandEducation { get; set; }
    
    /// <summary>Wife education level (Pendidikan Istri)</summary>
    public EducationLevel? WifeEducation { get; set; }
    
    #endregion

    #region Section VIII: Employment
    
    /// <summary>Husband's occupation (Pekerjaan Suami)</summary>
    public string? HusbandOccupation { get; set; }
    
    /// <summary>Wife's occupation (Pekerjaan Istri)</summary>
    public string? WifeOccupation { get; set; }
    
    #endregion

    #region Section IX-X: Family Planning Status
    
    /// <summary>Family Planning stage/status (Tahapan KS)</summary>
    public FamilyPlanningStage? FamilyPlanningStage { get; set; }
    
    /// <summary>Number of living children (Jumlah Anak Hidup)</summary>
    public int? NumberOfLivingChildren { get; set; }
    
    #endregion

    #region Section XI: Youngest Child Age
    
    /// <summary>Age of youngest child - years (Umur Anak Terkecil - Tahun)</summary>
    public int? YoungestChildYears { get; set; }
    
    /// <summary>Age of youngest child - months (Umur Anak Terkecil - Bulan)</summary>
    public int? YoungestChildMonths { get; set; }
    
    #endregion

    #region Section XII: KB Participant Status
    
    /// <summary>Current KB participant status (Status Peserta KB)</summary>
    public KBParticipantStatus? KBParticipantStatus { get; set; }
    
    /// <summary>Last contraceptive method used (Cara KB Terakhir)</summary>
    public ContraceptiveMethod? LastContraceptiveMethod { get; set; }
    
    #endregion

    #region Section 1-8: Pre-Insertion Examination for IUD/MOW
    
    /// <summary>Pregnancy signs present (Tanda Kehamilan)</summary>
    public bool? PregnancySigns { get; set; }
    
    /// <summary>Abnormal vaginal discharge (Keputihan Abnormal)</summary>
    public bool? AbnormalVaginalDischarge { get; set; }
    
    /// <summary>Abdominal pain (Nyeri Perut)</summary>
    public bool? AbdominalPain { get; set; }
    
    /// <summary>History of ectopic pregnancy (Riwayat Hamil Ektopik)</summary>
    public bool? EctopicPregnancyHistory { get; set; }
    
    /// <summary>Abnormal uterine bleeding (Perdarahan Uterus Abnormal)</summary>
    public bool? AbnormalUterinebleeding { get; set; }
    
    /// <summary>IUD still in place (IUD Masih Ada)</summary>
    public bool? IUDStillInPlace { get; set; }
    
    /// <summary>Pelvic pain (Nyeri Panggul)</summary>
    public bool? PelvicPain { get; set; }
    
    /// <summary>Dysmenorrhea (Dismenore)</summary>
    public bool? Dysmenorrhea { get; set; }
    
    #endregion

    #region Section 9: Internal Examination Findings
    
    /// <summary>Signs of inflammation present (Tanda - Tanda Radang)</summary>
    public bool? InflammationSigns { get; set; }
    
    /// <summary>Tumor or gynecological malignancy (Tumor/Keganasan Ginekologi)</summary>
    public bool? TumorOrMalignancy { get; set; }
    
    #endregion

    #region Section 10: Uterine Position
    
    /// <summary>Uterine position (Posisi Rahim)</summary>
    public UterinePosition? UterinePosition { get; set; }
    
    #endregion

    #region Section 11: Additional Examination for MOP/MOW
    
    /// <summary>Diabetes signs present (Tanda - Tanda Diabetes)</summary>
    public bool? DiabetesSigns { get; set; }
    
    /// <summary>Blood clotting disorder (Kelainan Pembekuan Darah)</summary>
    public bool? BloodClottingDisorder { get; set; }
    
    /// <summary>Orchitis/Epididymitis signs (Radang Orchitis/Epididymitis)</summary>
    public bool? OrchitisEpididymitis { get; set; }
    
    /// <summary>Tumor or gynecological malignancy (MOP/MOW section) (Tumor/Keganasan Ginekologi)</summary>
    public bool? TumorOrMalignancyMOP { get; set; }
    
    #endregion

    #region Section 12-17: Contraceptive Selection and Service
    
    /// <summary>Allowed contraceptive methods (Alat Kontrasepsi yang Boleh Digunakan)</summary>
    public string? AllowedContraceptiveMethods { get; set; }
    
    /// <summary>Selected contraceptive method (Metode dan Jenis Alat Kontrasepsi yang Dipilih)</summary>
    public string? SelectedContraceptiveMethod { get; set; }
    
    /// <summary>Service date (Tanggal Dilayani)</summary>
    public DateTime? ServiceDate { get; set; }
    
    /// <summary>Follow-up appointment date (Tanggal Dipesan Kembali)</summary>
    public DateTime? FollowUpDate { get; set; }
    
    /// <summary>Removal/Discontinuation date (Tanggal Dicabut)</summary>
    public DateTime? RemovalDate { get; set; }
    
    #endregion

    #region Observation/Monitoring
    
    /// <summary>Observation/Monitoring notes (Pengamatan)</summary>
    public string? ObservationNotes { get; set; }
    
    #endregion

    #region Audit Fields
    
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? UpdatedAt { get; set; }
    
    #endregion

    #region Relationships
    
    public Appointment Appointment { get; set; } = null!;
    
    /// <summary>Collection of contraceptive methods used</summary>
    public virtual ICollection<FamilyPlanningContraceptiveMethod> ContraceptiveMethods { get; set; } = new List<FamilyPlanningContraceptiveMethod>();
    
    /// <summary>Collection of laboratory/examination results</summary>
    public virtual ICollection<FamilyPlanningLaboratoryResult> LaboratoryResults { get; set; } = new List<FamilyPlanningLaboratoryResult>();
    
    /// <summary>Collection of procedures performed</summary>
    public virtual ICollection<FamilyPlanningProcedure> Procedures { get; set; } = new List<FamilyPlanningProcedure>();
    
    #endregion
}
