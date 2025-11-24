namespace MedizID.API.DTOs;

// Patient DTOs
public class CreatePatientRequest
{
    public string FirstName { get; set; } = null!;
    public string LastName { get; set; } = null!;
    public string? PhoneNumber { get; set; }
    public string? Email { get; set; }
    public DateTime DateOfBirth { get; set; }
    public string? Gender { get; set; }
    public string? BloodType { get; set; }
    public string? Address { get; set; }
    public string? City { get; set; }
}

public class UpdatePatientRequest
{
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public string? PhoneNumber { get; set; }
    public string? Email { get; set; }
    public string? Gender { get; set; }
    public string? BloodType { get; set; }
    public string? Address { get; set; }
    public string? City { get; set; }
}

public class PatientResponse
{
    public Guid Id { get; set; }
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
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
}

// Appointment DTOs
public class CreateAppointmentRequest
{
    public Guid PatientId { get; set; }
    public Guid? DoctorId { get; set; }
    public DateTime AppointmentDate { get; set; }
    public TimeSpan AppointmentTime { get; set; }
    public string? Reason { get; set; }
    public string? Notes { get; set; }
}

public class UpdateAppointmentRequest
{
    public DateTime? AppointmentDate { get; set; }
    public TimeSpan? AppointmentTime { get; set; }
    public string? Status { get; set; }
    public string? Reason { get; set; }
    public string? Notes { get; set; }
}

public class AppointmentResponse
{
    public Guid Id { get; set; }
    public Guid PatientId { get; set; }
    public string PatientName { get; set; } = null!;
    public Guid? DoctorId { get; set; }
    public string? DoctorName { get; set; }
    public DateTime AppointmentDate { get; set; }
    public TimeSpan AppointmentTime { get; set; }
    public string Status { get; set; } = null!;
    public string? Reason { get; set; }
    public DateTime CreatedAt { get; set; }
}

// Medical Record DTOs
public class CreateMedicalRecordRequest
{
    public Guid PatientId { get; set; }
    public Guid? AppointmentId { get; set; }
    public DateTime VisitDate { get; set; }
    public string? ChiefComplaint { get; set; }
    public string? Diagnosis { get; set; }
    public string? Treatment { get; set; }
    public string? Notes { get; set; }
}

public class UpdateMedicalRecordRequest
{
    public string? ChiefComplaint { get; set; }
    public string? Diagnosis { get; set; }
    public string? Treatment { get; set; }
    public string? Notes { get; set; }
}

public class MedicalRecordResponse
{
    public Guid Id { get; set; }
    public Guid PatientId { get; set; }
    public string PatientName { get; set; } = null!;
    public DateTime VisitDate { get; set; }
    public string? ChiefComplaint { get; set; }
    public string? Diagnosis { get; set; }
    public string? Treatment { get; set; }
    public DateTime CreatedAt { get; set; }
}

// Diagnosis DTOs
public class CreateDiagnosisRequest
{
    public Guid MedicalRecordId { get; set; }
    public string DiagnosisCode { get; set; } = null!;
    public string DiagnosisDescription { get; set; } = null!;
    public string? DiagnosisType { get; set; }
    public int? ConfidencePercentage { get; set; }
    public string? Reason { get; set; }
}

public class DiagnosisResponse
{
    public Guid Id { get; set; }
    public string DiagnosisCode { get; set; } = null!;
    public string DiagnosisDescription { get; set; } = null!;
    public string? DiagnosisType { get; set; }
    public int? ConfidencePercentage { get; set; }
    public DateTime CreatedAt { get; set; }
}

// Prescription DTOs
public class CreatePrescriptionRequest
{
    public Guid MedicalRecordId { get; set; }
    public string MedicationName { get; set; } = null!;
    public string Dosage { get; set; } = null!;
    public string Frequency { get; set; } = null!;
    public int Duration { get; set; }
    public string? Instructions { get; set; }
}

public class PrescriptionResponse
{
    public Guid Id { get; set; }
    public string MedicationName { get; set; } = null!;
    public string Dosage { get; set; } = null!;
    public string Frequency { get; set; } = null!;
    public int Duration { get; set; }
    public DateTime CreatedAt { get; set; }
}

// Laboratorium DTOs
public class CreateLaboratoriumRequest
{
    public Guid MedicalRecordId { get; set; }
    public string TestName { get; set; } = null!;
    public string? TestCode { get; set; }
    public string? Result { get; set; }
    public string? Unit { get; set; }
    public string? ReferenceRange { get; set; }
    public string? Status { get; set; }
}

public class LaboratoriumResponse
{
    public Guid Id { get; set; }
    public string TestName { get; set; } = null!;
    public string? Result { get; set; }
    public string? Unit { get; set; }
    public string? Status { get; set; }
    public DateTime TestDate { get; set; }
    public DateTime CreatedAt { get; set; }
}

// Appointment Detail Response DTO
public class AppointmentDetailResponse
{
    public Guid Id { get; set; }
    public Guid PatientId { get; set; }
    public string PatientName { get; set; } = null!;
    public string? PatientIdNumber { get; set; }
    public Guid? DoctorId { get; set; }
    public string? DoctorName { get; set; }
    public DateTime AppointmentDate { get; set; }
    public TimeSpan AppointmentTime { get; set; }
    public string Status { get; set; } = null!;
    public string? Reason { get; set; }
    public string? Notes { get; set; }
    public string? PoliName { get; set; }
    public string? InstallationName { get; set; }
    public string? PatientRoomBed { get; set; }
    public string? Insurance { get; set; }
    public string? ReferringDoctor { get; set; }
    public DateTime CreatedAt { get; set; }
}

// Appointment Medical History Response DTO
public class AppointmentMedicalHistoryResponse
{
    public string Waktu { get; set; } = null!;
    public string KodeICD10 { get; set; } = null!;
    public string NamaICD10 { get; set; } = null!;
    public string Diagnosa { get; set; } = null!;
    public string ObatObatan { get; set; } = null!;
    public string Status { get; set; } = null!;
}

// User DTOs
public class UserResponse
{
    public Guid Id { get; set; }
    public string Email { get; set; } = null!;
    public string FirstName { get; set; } = null!;
    public string LastName { get; set; } = null!;
    public string Role { get; set; } = null!;
    public string? PhoneNumber { get; set; }
    public string? LicenseNumber { get; set; }
    public string? Specialization { get; set; }
    public bool IsActive { get; set; }
    public DateTime CreatedAt { get; set; }
}

public class CreateUserRequest
{
    public string Email { get; set; } = null!;
    public string Password { get; set; } = null!;
    public string FirstName { get; set; } = null!;
    public string LastName { get; set; } = null!;
    public string Role { get; set; } = null!;
    public string? PhoneNumber { get; set; }
    public string? LicenseNumber { get; set; }
    public string? Specialization { get; set; }
    public bool IsActive { get; set; } = true;
}

// Anamnesis DTOs
public class CreateAnamnesisRequest
{
    public Guid MedicalRecordId { get; set; }
    public string? MedicalHistory { get; set; }
    public string? AllergiesHistory { get; set; }
    public string? MedicationHistory { get; set; }
    public string? SurgicalHistory { get; set; }
    public string? FamilyHistory { get; set; }
    public string? SocialHistory { get; set; }
    public string? ChiefComplaint { get; set; }
    public string? PresentIllness { get; set; }
}

public class AnamnesisResponse
{
    public Guid Id { get; set; }
    public Guid MedicalRecordId { get; set; }
    public string? MedicalHistory { get; set; }
    public string? AllergiesHistory { get; set; }
    public string? ChiefComplaint { get; set; }
    public DateTime CreatedAt { get; set; }
}

// Diagnosis DTOs - Enhanced
public class DiagnosisDetailResponse
{
    public Guid Id { get; set; }
    public string DiagnosisCode { get; set; } = null!;
    public string DiagnosisDescription { get; set; } = null!;
    public string? DiagnosisType { get; set; }
    public int? ConfidencePercentage { get; set; }
    public string? Reason { get; set; }
    public Guid MedicalRecordId { get; set; }
    public DateTime CreatedAt { get; set; }
}

// Prescription DTOs - Enhanced
public class PrescriptionDetailResponse
{
    public Guid Id { get; set; }
    public string MedicationName { get; set; } = null!;
    public string Dosage { get; set; } = null!;
    public string Frequency { get; set; } = null!;
    public int Duration { get; set; }
    public string? Instructions { get; set; }
    public Guid MedicalRecordId { get; set; }
    public DateTime CreatedAt { get; set; }
}

// Drug DTOs
public class CreateDrugRequest
{
    public string GenericName { get; set; } = null!;
    public string? BrandName { get; set; }
    public string? Dosage { get; set; }
    public Guid DrugCategoryId { get; set; }
    public string? Manufacturer { get; set; }
    public string? IndividualPrice { get; set; }
}

public class UpdateDrugRequest
{
    public string? GenericName { get; set; }
    public string? BrandName { get; set; }
    public string? Dosage { get; set; }
    public Guid? DrugCategoryId { get; set; }
    public string? Manufacturer { get; set; }
    public string? IndividualPrice { get; set; }
    public bool? IsActive { get; set; }
}

public class DrugResponse
{
    public Guid Id { get; set; }
    public string GenericName { get; set; } = null!;
    public string? BrandName { get; set; }
    public string? Dosage { get; set; }
    public Guid DrugCategoryId { get; set; }
    public string? Manufacturer { get; set; }
    public string? IndividualPrice { get; set; }
    public bool IsActive { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
}

// Drug Category DTOs
public class CreateDrugCategoryRequest
{
    public string Name { get; set; } = null!;
    public string? Description { get; set; }
}

public class UpdateDrugCategoryRequest
{
    public string? Name { get; set; }
    public string? Description { get; set; }
    public bool? IsActive { get; set; }
}

public class DrugCategoryResponse
{
    public Guid Id { get; set; }
    public string Name { get; set; } = null!;
    public string? Description { get; set; }
    public bool IsActive { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
}

// Medical Equipment DTOs
public class CreateMedicalEquipmentRequest
{
    public string Name { get; set; } = null!;
    public Guid EquipmentTypeId { get; set; }
    public string? SerialNumber { get; set; }
    public DateTime? PurchaseDate { get; set; }
    public DateTime? LastServiceDate { get; set; }
}

public class UpdateMedicalEquipmentRequest
{
    public string? Name { get; set; }
    public Guid? EquipmentTypeId { get; set; }
    public string? SerialNumber { get; set; }
    public DateTime? PurchaseDate { get; set; }
    public DateTime? LastServiceDate { get; set; }
    public bool? IsActive { get; set; }
}

public class MedicalEquipmentResponse
{
    public Guid Id { get; set; }
    public string Name { get; set; } = null!;
    public Guid EquipmentTypeId { get; set; }
    public string? SerialNumber { get; set; }
    public Guid FacilityId { get; set; }
    public DateTime? PurchaseDate { get; set; }
    public DateTime? LastServiceDate { get; set; }
    public bool IsActive { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
}

// Medical Equipment Type DTOs
public class CreateMedicalEquipmentTypeRequest
{
    public string Name { get; set; } = null!;
    public string? Description { get; set; }
}

public class UpdateMedicalEquipmentTypeRequest
{
    public string? Name { get; set; }
    public string? Description { get; set; }
    public bool? IsActive { get; set; }
}

public class MedicalEquipmentTypeResponse
{
    public Guid Id { get; set; }
    public string Name { get; set; } = null!;
    public string? Description { get; set; }
    public bool IsActive { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
}

// Drug Interaction DTOs
public class CheckDrugInteractionRequest
{
    public List<Guid> DrugIds { get; set; } = new();
}

public class DrugInteractionCheckResponse
{
    public List<DrugInteractionDetail> Interactions { get; set; } = new();
    public bool HasInteractions { get; set; }
}

public class DrugInteractionDetail
{
    public Guid Drug1Id { get; set; }
    public Guid Drug2Id { get; set; }
    public string? Drug1Name { get; set; }
    public string? Drug2Name { get; set; }
    public string? InteractionSeverity { get; set; }
    public string? Description { get; set; }
}

// ICD10 Code DTOs
public class CreateICD10CodeRequest
{
    public string Code { get; set; } = null!;
    public string Description { get; set; } = null!;
    public string? Category { get; set; }
}

public class ICD10CodeResponse
{
    public Guid Id { get; set; }
    public string Code { get; set; } = null!;
    public string Description { get; set; } = null!;
    public string? Category { get; set; }
    public bool IsActive { get; set; }
    public DateTime CreatedAt { get; set; }
}

// Symptom DTOs
public class CreateSymptomRequest
{
    public string Name { get; set; } = null!;
    public string? Description { get; set; }
    public string? Category { get; set; }
}

public class SymptomResponse
{
    public Guid Id { get; set; }
    public string Name { get; set; } = null!;
    public string? Description { get; set; }
    public string? Category { get; set; }
    public bool IsActive { get; set; }
    public DateTime CreatedAt { get; set; }
}

// Anamnesis Template DTOs
public class CreateAnamnesisTemplateRequest
{
    public string Name { get; set; } = null!;
    public string? Description { get; set; }
    public string? TemplateContent { get; set; }
}

public class UpdateAnamnesisTemplateRequest
{
    public string? Name { get; set; }
    public string? Description { get; set; }
    public string? TemplateContent { get; set; }
    public bool? IsActive { get; set; }
}

public class AnamnesisTemplateResponse
{
    public Guid Id { get; set; }
    public Guid FacilityId { get; set; }
    public string Name { get; set; } = null!;
    public string? Description { get; set; }
    public string? TemplateContent { get; set; }
    public bool IsActive { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
}

// AI Recommendation DTOs
public class CreateAIRecommendationRequest
{
    public Guid MedicalRecordId { get; set; }
    public string RecommendationType { get; set; } = null!;
    public string Content { get; set; } = null!;
    public float ConfidenceScore { get; set; }
}

public class UpdateAIRecommendationFeedbackRequest
{
    public string? FeedbackStatus { get; set; }
    public string? FeedbackNotes { get; set; }
}

public class AIRecommendationResponse
{
    public Guid Id { get; set; }
    public Guid MedicalRecordId { get; set; }
    public string RecommendationType { get; set; } = null!;
    public string Content { get; set; } = null!;
    public float ConfidenceScore { get; set; }
    public string? FeedbackStatus { get; set; }
    public string? FeedbackNotes { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
}

public class AIRecommendationPrescriptionRequest
{
    public Guid AppointmentId { get; set; }
}

public class PrescriptionAIResponse
{
    public List<AIRecommendationResponse> Recommendations { get; set; } = new();
}

public class AIDiagnosisResponse
{
    public List<AIRecommendationResponse> Recommendations { get; set; } = new();
}

// Diagnosis Search DTOs
public class DiagnosisSearchRequest
{
    public string? Query { get; set; }
    public string? Category { get; set; }
    public int Page { get; set; } = 1;
    public int PageSize { get; set; } = 10;
}

public class DiagnosisSearchResponse
{
    public List<ICD10CodeResponse> Results { get; set; } = new();
    public int Total { get; set; }
    public int Page { get; set; }
    public int PageSize { get; set; }
}

// Drug Search DTOs
public class DrugSearchRequest
{
    public string? Query { get; set; }
    public Guid? CategoryId { get; set; }
}

public class DrugSearchResponse
{
    public List<DrugResponse> Results { get; set; } = new();
}

// Equipment Search DTOs
public class EquipmentSearchRequest
{
    public string? Query { get; set; }
    public Guid? EquipmentTypeId { get; set; }
}

public class EquipmentSearchResponse
{
    public List<MedicalEquipmentResponse> Results { get; set; } = new();
}

// Prescription Statistics DTOs
public class PrescriptionStatisticsRequest
{
    public DateTime? StartDate { get; set; }
    public DateTime? EndDate { get; set; }
}

public class PrescriptionStatisticsResponse
{
    public int TotalPrescriptions { get; set; }
    public int TotalDispensed { get; set; }
    public int TotalPending { get; set; }
    public int MostCommonMedications { get; set; }
    public List<string> TopDrugs { get; set; } = new();
}

// Patient Allergies DTOs
public class PatientAllergiesResponse
{
    public Guid PatientId { get; set; }
    public List<string> Allergies { get; set; } = new();
}

// Patient Prescription History DTOs
public class PatientPrescriptionHistoryResponse
{
    public Guid PatientId { get; set; }
    public List<PrescriptionHistoryItem> Prescriptions { get; set; } = new();
}

public class PrescriptionHistoryItem
{
    public Guid Id { get; set; }
    public string MedicationName { get; set; } = null!;
    public string Dosage { get; set; } = null!;
    public string Frequency { get; set; } = null!;
    public DateTime PrescribedDate { get; set; }
    public int DurationDays { get; set; }
}

// Prescription Equipment DTOs
public class AddPrescriptionEquipmentRequest
{
    public Guid EquipmentId { get; set; }
}

// Prescription Dispense DTOs
public class DispensePrescriptionRequest
{
    public DateTime DispenseDate { get; set; }
    public string? Notes { get; set; }
}

// Patient Prescription Public DTOs
public class PatientPrescriptionPublic
{
    public Guid Id { get; set; }
    public string MedicationName { get; set; } = null!;
    public string Dosage { get; set; } = null!;
    public string Frequency { get; set; } = null!;
    public int Duration { get; set; }
    public DateTime CreatedAt { get; set; }
}

// Medical Record Detail Response
public class MedicalRecordDetail
{
    public Guid Id { get; set; }
    public Guid PatientId { get; set; }
    public string PatientName { get; set; } = null!;
    public DateTime VisitDate { get; set; }
    public string? ChiefComplaint { get; set; }
    public string? Diagnosis { get; set; }
    public string? Treatment { get; set; }
    public List<DiagnosisResponse> Diagnoses { get; set; } = new();
    public List<PrescriptionResponse> Prescriptions { get; set; } = new();
    public List<LaboratoriumResponse> LaboratoriumTests { get; set; } = new();
    public DateTime CreatedAt { get; set; }
}

// Patient Detail Response
public class PatientDetail
{
    public Guid Id { get; set; }
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
    public int TotalAppointments { get; set; }
    public int TotalMedicalRecords { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
}

// User Detail Response
public class UserDetail
{
    public Guid Id { get; set; }
    public string Email { get; set; } = null!;
    public string FirstName { get; set; } = null!;
    public string LastName { get; set; } = null!;
    public string Role { get; set; } = null!;
    public string? PhoneNumber { get; set; }
    public string? LicenseNumber { get; set; }
    public string? Specialization { get; set; }
    public Guid? FacilityId { get; set; }
    public bool IsActive { get; set; }
    public DateTime CreatedAt { get; set; }
}

// Patients Response
public class PatientsResponse
{
    public List<PatientResponse> Items { get; set; } = new();
    public int Total { get; set; }
}

// Appointments Response
public class AppointmentsResponse
{
    public List<AppointmentResponse> Items { get; set; } = new();
    public int Total { get; set; }
}

// Patient Anamnesis Public
public class PatientAnamnesisPublic
{
    public Guid Id { get; set; }
    public Guid MedicalRecordId { get; set; }
    public string? ChiefComplaint { get; set; }
    public string? PresentingIllness { get; set; }
    public DateTime CreatedAt { get; set; }
}

// Patient Anamnesis Detail
public class PatientAnamnesisDetail
{
    public Guid Id { get; set; }
    public Guid MedicalRecordId { get; set; }
    public string? ChiefComplaint { get; set; }
    public string? PresentingIllness { get; set; }
    public string? PastMedicalHistory { get; set; }
    public string? Allergies { get; set; }
    public string? CurrentMedications { get; set; }
    public string? SocialHistory { get; set; }
    public DateTime CreatedAt { get; set; }
}

// Anamnesis Create Response
public class AnamnesisCreateResponse
{
    public Guid Id { get; set; }
    public Guid MedicalRecordId { get; set; }
    public string Message { get; set; } = "Anamnesis created successfully";
}

// Anamnesis Update Response
public class AnamnesisUpdateResponse
{
    public Guid Id { get; set; }
    public string Message { get; set; } = "Anamnesis updated successfully";
}

// Appointment Create Response
public class AppointmentCreateResponse
{
    public Guid Id { get; set; }
    public string Message { get; set; } = "Appointment created successfully";
}

// Patient Create Response
public class PatientCreateResponse
{
    public Guid Id { get; set; }
    public string MedicalRecordNumber { get; set; } = null!;
    public string Message { get; set; } = "Patient created successfully";
}

// Patient Diagnosis Detail
public class PatientDiagnosisDetail
{
    public Guid Id { get; set; }
    public Guid MedicalRecordId { get; set; }
    public string DiagnosisCode { get; set; } = null!;
    public string DiagnosisDescription { get; set; } = null!;
    public string? DiagnosisType { get; set; }
    public int? ConfidencePercentage { get; set; }
    public DateTime CreatedAt { get; set; }
}

// Patient Laboratorium Schema
public class PatientLaboratoriumSchema
{
    public Guid Id { get; set; }
    public Guid MedicalRecordId { get; set; }
    public string TestName { get; set; } = null!;
    public string? TestCode { get; set; }
    public string? Result { get; set; }
    public string? Unit { get; set; }
    public string? ReferenceRange { get; set; }
    public string? Status { get; set; }
    public DateTime TestDate { get; set; }
    public DateTime CreatedAt { get; set; }
}

// Patient Prescription Public
public class PatientPrescriptionDetail
{
    public Guid Id { get; set; }
    public Guid MedicalRecordId { get; set; }
    public string MedicationName { get; set; } = null!;
    public string Dosage { get; set; } = null!;
    public string Frequency { get; set; } = null!;
    public int Duration { get; set; }
    public string? Instructions { get; set; }
    public DateTime? ExpiryDate { get; set; }
    public DateTime CreatedAt { get; set; }
}