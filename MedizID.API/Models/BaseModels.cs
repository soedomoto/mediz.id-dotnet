using Microsoft.AspNetCore.Identity;
using MedizID.API.Common.Enums;

namespace MedizID.API.Models;

public class ApplicationUser : IdentityUser<Guid>
{
    public string FirstName { get; set; } = null!;
    public string LastName { get; set; } = null!;
    public UserRoleEnum Role { get; set; }
    public Guid? FacilityId { get; set; }
    public Facility? Facility { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? UpdatedAt { get; set; }
    public bool IsActive { get; set; } = true;
}

public class Facility
{
    public Guid Id { get; set; }
    public string Name { get; set; } = null!;
    public string Address { get; set; } = null!;
    public string City { get; set; } = null!;
    public string Province { get; set; } = null!;
    public string PostalCode { get; set; } = null!;
    public string? PhoneNumber { get; set; }
    public string? Email { get; set; }
    public FacilityTypeEnum Type { get; set; }
    public bool IsActive { get; set; } = true;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? UpdatedAt { get; set; }

    // Relationships
    public ICollection<ApplicationUser> Users { get; set; } = new List<ApplicationUser>();
    public ICollection<Department> Departments { get; set; } = new List<Department>();
    public ICollection<Patient> Patients { get; set; } = new List<Patient>();
    public ICollection<Appointment> Appointments { get; set; } = new List<Appointment>();
}

public class Department
{
    public Guid Id { get; set; }
    public Guid FacilityId { get; set; }
    public string Name { get; set; } = null!;
    public string? Description { get; set; }
    public bool IsActive { get; set; } = true;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    // Relationships
    public Facility Facility { get; set; } = null!;
}

public class Patient
{
    public Guid Id { get; set; }
    public Guid FacilityId { get; set; }
    public string MedicalRecordNumber { get; set; } = null!;
    public string FirstName { get; set; } = null!;
    public string LastName { get; set; } = null!;
    public string? PhoneNumber { get; set; }
    public string? Email { get; set; }
    public DateTime DateOfBirth { get; set; }
    public string? Gender { get; set; }
    public string? BloodType { get; set; }
    public string? Address { get; set; }
    public string? City { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? UpdatedAt { get; set; }

    // Relationships
    public Facility Facility { get; set; } = null!;
    public ICollection<Appointment> Appointments { get; set; } = new List<Appointment>();
    public ICollection<MedicalRecord> MedicalRecords { get; set; } = new List<MedicalRecord>();
}

public class Appointment
{
    public Guid Id { get; set; }
    public Guid FacilityId { get; set; }
    public Guid PatientId { get; set; }
    public Guid? DoctorId { get; set; }
    public DateTime AppointmentDate { get; set; }
    public TimeSpan AppointmentTime { get; set; }
    public AppointmentStatusEnum Status { get; set; } = AppointmentStatusEnum.Scheduled;
    public string? Reason { get; set; }
    public string? Notes { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? UpdatedAt { get; set; }

    // Relationships
    public Facility Facility { get; set; } = null!;
    public Patient Patient { get; set; } = null!;
    public ApplicationUser? Doctor { get; set; }
    public ICollection<MedicalRecord> MedicalRecords { get; set; } = new List<MedicalRecord>();
}

public class MedicalRecord
{
    public Guid Id { get; set; }
    public Guid PatientId { get; set; }
    public Guid? AppointmentId { get; set; }
    public Guid? DoctorId { get; set; }
    public DateTime VisitDate { get; set; }
    public string? ChiefComplaint { get; set; }
    public string? Diagnosis { get; set; }
    public string? Treatment { get; set; }
    public string? Notes { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? UpdatedAt { get; set; }

    // Relationships
    public Patient Patient { get; set; } = null!;
    public Appointment? Appointment { get; set; }
    public ApplicationUser? Doctor { get; set; }
    public ICollection<Anamnesis> Anamnesis { get; set; } = new List<Anamnesis>();
    public ICollection<Diagnosis> Diagnoses { get; set; } = new List<Diagnosis>();
    public ICollection<Prescription> Prescriptions { get; set; } = new List<Prescription>();
    public ICollection<Laboratorium> LaboratoriumTests { get; set; } = new List<Laboratorium>();
}

public class Anamnesis
{
    public Guid Id { get; set; }
    public Guid MedicalRecordId { get; set; }
    public string? ChiefComplaint { get; set; }
    public string? PresentingIllness { get; set; }
    public string? PastMedicalHistory { get; set; }
    public string? Allergies { get; set; }
    public string? CurrentMedications { get; set; }
    public string? SocialHistory { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    // Relationships
    public MedicalRecord MedicalRecord { get; set; } = null!;
}

public class Diagnosis
{
    public Guid Id { get; set; }
    public Guid MedicalRecordId { get; set; }
    public string DiagnosisCode { get; set; } = null!; // ICD-10 code
    public string DiagnosisDescription { get; set; } = null!;
    public string? DiagnosisType { get; set; } // Primary, Secondary, etc.
    public int? ConfidencePercentage { get; set; }
    public string? Reason { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    // Relationships
    public MedicalRecord MedicalRecord { get; set; } = null!;
}

public class Prescription
{
    public Guid Id { get; set; }
    public Guid MedicalRecordId { get; set; }
    public string MedicationName { get; set; } = null!;
    public string Dosage { get; set; } = null!;
    public string Frequency { get; set; } = null!;
    public int Duration { get; set; } // in days
    public string? Instructions { get; set; }
    public DateTime? ExpiryDate { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    // Relationships
    public MedicalRecord MedicalRecord { get; set; } = null!;
}

public class Laboratorium
{
    public Guid Id { get; set; }
    public Guid MedicalRecordId { get; set; }
    public string TestName { get; set; } = null!;
    public string? TestCode { get; set; }
    public string? Result { get; set; }
    public string? Unit { get; set; }
    public string? ReferenceRange { get; set; }
    public string? Status { get; set; } // Normal, Abnormal
    public DateTime TestDate { get; set; } = DateTime.UtcNow;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    // Relationships
    public MedicalRecord MedicalRecord { get; set; } = null!;
}

// Facilities Management Models
public class Installation
{
    public Guid Id { get; set; }
    public Guid FacilityId { get; set; }
    public string Name { get; set; } = null!;
    public string? Type { get; set; }
    public string? Location { get; set; }
    public string? Condition { get; set; }
    public DateTime? LastMaintenanceDate { get; set; }
    public bool IsActive { get; set; } = true;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? UpdatedAt { get; set; }

    // Relationships
    public Facility Facility { get; set; } = null!;
}

public class Poli
{
    public Guid Id { get; set; }
    public Guid FacilityId { get; set; }
    public Guid DepartmentId { get; set; }
    public string Name { get; set; } = null!;
    public string? Description { get; set; }
    public bool IsActive { get; set; } = true;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? UpdatedAt { get; set; }

    // Relationships
    public Facility Facility { get; set; } = null!;
    public Department Department { get; set; } = null!;
    public ICollection<PoliTimeSlot> TimeSlots { get; set; } = new List<PoliTimeSlot>();
}

public class PoliTimeSlot
{
    public Guid Id { get; set; }
    public Guid PoliId { get; set; }
    public Guid? StaffId { get; set; }
    public DayOfWeek DayOfWeek { get; set; }
    public TimeSpan StartTime { get; set; }
    public TimeSpan EndTime { get; set; }
    public int SlotDuration { get; set; } // in minutes
    public int MaxPatients { get; set; }
    public bool IsActive { get; set; } = true;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? UpdatedAt { get; set; }

    // Relationships
    public Poli Poli { get; set; } = null!;
    public ApplicationUser? Staff { get; set; }
}

public class FacilityStaff
{
    public Guid Id { get; set; }
    public Guid FacilityId { get; set; }
    public Guid StaffId { get; set; }
    public Guid? DepartmentId { get; set; }
    public string Position { get; set; } = null!;
    public string? Specialization { get; set; }
    public DateTime StartDate { get; set; } = DateTime.UtcNow;
    public DateTime? EndDate { get; set; }
    public bool IsActive { get; set; } = true;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? UpdatedAt { get; set; }

    // Relationships
    public Facility Facility { get; set; } = null!;
    public ApplicationUser Staff { get; set; } = null!;
    public Department? Department { get; set; }
}

// Specialized Medical Records
public class AnamnesisTemplate
{
    public Guid Id { get; set; }
    public Guid FacilityId { get; set; }
    public string Name { get; set; } = null!;
    public string? Description { get; set; }
    public string? TemplateContent { get; set; }
    public bool IsActive { get; set; } = true;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? UpdatedAt { get; set; }

    // Relationships
    public Facility Facility { get; set; } = null!;
}

public class ICD10Code
{
    public Guid Id { get; set; }
    public string Code { get; set; } = null!;
    public string Description { get; set; } = null!;
    public string? Category { get; set; }
    public bool IsActive { get; set; } = true;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}

public class Symptom
{
    public Guid Id { get; set; }
    public string Name { get; set; } = null!;
    public string? Description { get; set; }
    public string? Category { get; set; }
    public bool IsActive { get; set; } = true;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}

public class Drug
{
    public Guid Id { get; set; }
    public string GenericName { get; set; } = null!;
    public string? BrandName { get; set; }
    public string? Dosage { get; set; }
    public Guid DrugCategoryId { get; set; }
    public string? Manufacturer { get; set; }
    public string? IndividualPrice { get; set; }
    public bool IsActive { get; set; } = true;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? UpdatedAt { get; set; }

    // Relationships
    public DrugCategory Category { get; set; } = null!;
}

public class DrugCategory
{
    public Guid Id { get; set; }
    public string Name { get; set; } = null!;
    public string? Description { get; set; }
    public bool IsActive { get; set; } = true;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? UpdatedAt { get; set; }

    // Relationships
    public ICollection<Drug> Drugs { get; set; } = new List<Drug>();
}

public class MedicalEquipment
{
    public Guid Id { get; set; }
    public string Name { get; set; } = null!;
    public Guid EquipmentTypeId { get; set; }
    public string? SerialNumber { get; set; }
    public Guid FacilityId { get; set; }
    public DateTime? PurchaseDate { get; set; }
    public DateTime? LastServiceDate { get; set; }
    public bool IsActive { get; set; } = true;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? UpdatedAt { get; set; }

    // Relationships
    public MedicalEquipmentType EquipmentType { get; set; } = null!;
    public Facility Facility { get; set; } = null!;
}

public class MedicalEquipmentType
{
    public Guid Id { get; set; }
    public string Name { get; set; } = null!;
    public string? Description { get; set; }
    public bool IsActive { get; set; } = true;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? UpdatedAt { get; set; }

    // Relationships
    public ICollection<MedicalEquipment> Equipment { get; set; } = new List<MedicalEquipment>();
}

public class DrugInteraction
{
    public Guid Id { get; set; }
    public Guid Drug1Id { get; set; }
    public Guid Drug2Id { get; set; }
    public string? InteractionSeverity { get; set; } // Minor, Moderate, Major
    public string? Description { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}

public class AIRecommendation
{
    public Guid Id { get; set; }
    public Guid MedicalRecordId { get; set; }
    public string RecommendationType { get; set; } = null!; // Diagnosis, Prescription, etc.
    public string Content { get; set; } = null!;
    public float ConfidenceScore { get; set; }
    public string? FeedbackStatus { get; set; } // Accepted, Rejected, Pending
    public string? FeedbackNotes { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? UpdatedAt { get; set; }

    // Relationships
    public MedicalRecord MedicalRecord { get; set; } = null!;
}
