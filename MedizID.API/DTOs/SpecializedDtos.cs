namespace MedizID.API.DTOs;

// Facility DTOs
public class CreateFacilityRequest
{
    public string Name { get; set; } = null!;
    public string Address { get; set; } = null!;
    public string City { get; set; } = null!;
    public string Province { get; set; } = null!;
    public string PostalCode { get; set; } = null!;
    public string? PhoneNumber { get; set; }
    public string? Email { get; set; }
    public int Type { get; set; }
}

public class UpdateFacilityRequest
{
    public string? Name { get; set; }
    public string? Address { get; set; }
    public string? City { get; set; }
    public string? Province { get; set; }
    public string? PostalCode { get; set; }
    public string? PhoneNumber { get; set; }
    public string? Email { get; set; }
    public bool? IsActive { get; set; }
}

public class FacilityResponse
{
    public Guid Id { get; set; }
    public string Name { get; set; } = null!;
    public string Address { get; set; } = null!;
    public string City { get; set; } = null!;
    public string Province { get; set; } = null!;
    public string PostalCode { get; set; } = null!;
    public string? PhoneNumber { get; set; }
    public string? Email { get; set; }
    public string Type { get; set; } = null!;
    public bool IsActive { get; set; }
    public DateTime CreatedAt { get; set; }
}

// User DTOs (defined in MedicalRecordDtos.cs)
public class UpdateUserRequest
{
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public string? Email { get; set; }
    public Guid? FacilityId { get; set; }
    public bool? IsActive { get; set; }
}

// Family Planning DTOs
public class CreateFamilyPlanningRequest
{
    public Guid MedicalRecordId { get; set; }
    public string? CurrentMethod { get; set; }
    public DateTime? StartDate { get; set; }
    public string? Status { get; set; }
    public string? Notes { get; set; }
}

public class FamilyPlanningResponse
{
    public Guid Id { get; set; }
    public string? CurrentMethod { get; set; }
    public DateTime? StartDate { get; set; }
    public string? Status { get; set; }
    public DateTime CreatedAt { get; set; }
}

// MCH DTOs
public class CreateMaternalChildHealthRequest
{
    public Guid MedicalRecordId { get; set; }
    public string? PregnancyWeeks { get; set; }
    public string? BloodPressure { get; set; }
    public decimal? Weight { get; set; }
    public string? FetalHeartRate { get; set; }
    public string? Notes { get; set; }
}

public class MaternalChildHealthResponse
{
    public Guid Id { get; set; }
    public string? PregnancyWeeks { get; set; }
    public string? BloodPressure { get; set; }
    public decimal? Weight { get; set; }
    public string? FetalHeartRate { get; set; }
    public DateTime CreatedAt { get; set; }
}

// Immunization DTOs
public class CreateImmunizationRequest
{
    public Guid MedicalRecordId { get; set; }
    public string VaccineName { get; set; } = null!;
    public DateTime VaccineDate { get; set; }
    public int? DoseNumber { get; set; }
    public string? Lot { get; set; }
    public string? Route { get; set; }
    public string? Site { get; set; }
    public string? Reactions { get; set; }
}

public class ImmunizationResponse
{
    public Guid Id { get; set; }
    public string VaccineName { get; set; } = null!;
    public DateTime VaccineDate { get; set; }
    public int? DoseNumber { get; set; }
    public string? Lot { get; set; }
    public DateTime CreatedAt { get; set; }
}

// Medical Procedure DTOs
public class CreateMedicalProcedureRequest
{
    public Guid MedicalRecordId { get; set; }
    public string ProcedureName { get; set; } = null!;
    public DateTime ProcedureDate { get; set; }
    public string? Indication { get; set; }
    public string? Findings { get; set; }
    public string? Complications { get; set; }
    public string? Notes { get; set; }
}

public class MedicalProcedureResponse
{
    public Guid Id { get; set; }
    public string ProcedureName { get; set; } = null!;
    public DateTime ProcedureDate { get; set; }
    public string? Indication { get; set; }
    public string? Findings { get; set; }
    public DateTime CreatedAt { get; set; }
}

// Odontogram DTOs
public class CreateOdontogramRequest
{
    public Guid MedicalRecordId { get; set; }
    public string? ToothNumber { get; set; }
    public string? Surface { get; set; }
    public string? Status { get; set; }
    public string? Treatment { get; set; }
}

public class OdontogramResponse
{
    public Guid Id { get; set; }
    public string? ToothNumber { get; set; }
    public string? Status { get; set; }
    public string? Treatment { get; set; }
    public DateTime CreatedAt { get; set; }
}

// Installation DTOs
public class CreateInstallationRequest
{
    public string Name { get; set; } = null!;
    public string? Type { get; set; }
    public string? Location { get; set; }
    public string? Condition { get; set; }
    public DateTime? LastMaintenanceDate { get; set; }
    public bool? IsActive { get; set; }
}

public class UpdateInstallationRequest
{
    public string? Name { get; set; }
    public string? Type { get; set; }
    public string? Location { get; set; }
    public string? Condition { get; set; }
    public DateTime? LastMaintenanceDate { get; set; }
    public bool? IsActive { get; set; }
}

public class InstallationResponse
{
    public Guid Id { get; set; }
    public Guid FacilityId { get; set; }
    public string Name { get; set; } = null!;
    public string? Type { get; set; }
    public string? Location { get; set; }
    public string? Condition { get; set; }
    public DateTime? LastMaintenanceDate { get; set; }
    public bool IsActive { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
}

// Poli DTOs
public class CreatePoliRequest
{
    public Guid DepartmentId { get; set; }
    public string Name { get; set; } = null!;
    public string? Description { get; set; }
}

public class UpdatePoliRequest
{
    public string? Name { get; set; }
    public string? Description { get; set; }
    public bool? IsActive { get; set; }
}

public class PoliResponse
{
    public Guid Id { get; set; }
    public Guid FacilityId { get; set; }
    public Guid DepartmentId { get; set; }
    public string Name { get; set; } = null!;
    public string? Description { get; set; }
    public bool IsActive { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
}

// Poli Time Slot DTOs
public class CreatePoliTimeSlotRequest
{
    public Guid? StaffId { get; set; }
    public int DayOfWeek { get; set; }
    public TimeSpan StartTime { get; set; }
    public TimeSpan EndTime { get; set; }
    public int SlotDuration { get; set; }
    public int MaxPatients { get; set; }
}

public class UpdatePoliTimeSlotRequest
{
    public Guid? StaffId { get; set; }
    public int? DayOfWeek { get; set; }
    public TimeSpan? StartTime { get; set; }
    public TimeSpan? EndTime { get; set; }
    public int? SlotDuration { get; set; }
    public int? MaxPatients { get; set; }
    public bool? IsActive { get; set; }
}

public class PoliTimeSlotResponse
{
    public Guid Id { get; set; }
    public Guid PoliId { get; set; }
    public Guid? StaffId { get; set; }
    public int DayOfWeek { get; set; }
    public TimeSpan StartTime { get; set; }
    public TimeSpan EndTime { get; set; }
    public int SlotDuration { get; set; }
    public int MaxPatients { get; set; }
    public bool IsActive { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
}

// Facility Staff DTOs
public class AddStaffToFacilityRequest
{
    public Guid StaffId { get; set; }
    public Guid? DepartmentId { get; set; }
    public string Position { get; set; } = null!;
    public string? Specialization { get; set; }
}

public class UpdateFacilityStaffRequest
{
    public Guid? DepartmentId { get; set; }
    public string? Position { get; set; }
    public string? Specialization { get; set; }
    public bool? IsActive { get; set; }
}

public class FacilityStaffResponse
{
    public Guid Id { get; set; }
    public Guid FacilityId { get; set; }
    public Guid StaffId { get; set; }
    public Guid? DepartmentId { get; set; }
    public string Position { get; set; } = null!;
    public string? Specialization { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime? EndDate { get; set; }
    public bool IsActive { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
}

// Department DTOs
public class CreateDepartmentRequest
{
    public string Name { get; set; } = null!;
    public string? Description { get; set; }
}

public class UpdateDepartmentRequest
{
    public string? Name { get; set; }
    public string? Description { get; set; }
    public bool? IsActive { get; set; }
}

public class DepartmentResponse
{
    public Guid Id { get; set; }
    public Guid FacilityId { get; set; }
    public string Name { get; set; } = null!;
    public string? Description { get; set; }
    public bool IsActive { get; set; }
    public DateTime CreatedAt { get; set; }
}

// Facility Appointment DTOs
public class CreateFacilityAppointmentRequest
{
    public Guid PatientId { get; set; }
    public Guid? DoctorId { get; set; }
    public DateTime AppointmentDate { get; set; }
    public TimeSpan AppointmentTime { get; set; }
    public string? Reason { get; set; }
    public string? Notes { get; set; }
}

public class FacilityAppointmentResponse
{
    public Guid Id { get; set; }
    public Guid FacilityId { get; set; }
    public Guid PatientId { get; set; }
    public Guid? DoctorId { get; set; }
    public DateTime AppointmentDate { get; set; }
    public TimeSpan AppointmentTime { get; set; }
    public string Status { get; set; } = null!;
    public string? Reason { get; set; }
    public string? Notes { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
}

// Facility Patient DTOs
public class CreateFacilityPatientRequest
{
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
}

public class FacilityPatientResponse
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
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
}

public class FacilityPatientDetailResponse : FacilityPatientResponse
{
    public int TotalAppointments { get; set; }
    public int TotalMedicalRecords { get; set; }
    public DateTime? LastVisitDate { get; set; }
}

// STI DTOs
public class CreateSTIRequest
{
    public Guid MedicalRecordId { get; set; }
    public Guid? AppointmentId { get; set; }
    public string VisitStatus { get; set; } = null!;
    public string RiskGroup { get; set; } = null!;
    public string? Screening { get; set; }
    public string? Diagnosis { get; set; }
    public string? Treatment { get; set; }
}

public class UpdateSTIRequest
{
    public string? VisitStatus { get; set; }
    public string? RiskGroup { get; set; }
    public string? Screening { get; set; }
    public string? Diagnosis { get; set; }
    public string? Treatment { get; set; }
}

public class STIResponse
{
    public Guid Id { get; set; }
    public Guid MedicalRecordId { get; set; }
    public Guid? AppointmentId { get; set; }
    public string VisitStatus { get; set; } = null!;
    public string RiskGroup { get; set; } = null!;
    public string? Screening { get; set; }
    public string? Diagnosis { get; set; }
    public string? Treatment { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
}

public class STIListResponse
{
    public List<STIResponse> Items { get; set; } = new();
    public int Total { get; set; }
}

// Adolescent Health DTOs
public class CreateAdolescentHealthRequest
{
    public Guid MedicalRecordId { get; set; }
    public Guid? AppointmentId { get; set; }
    public string? HealthConcern { get; set; }
    public string? SexualActivity { get; set; }
    public string? Contraception { get; set; }
    public string? Counseling { get; set; }
}

public class UpdateAdolescentHealthRequest
{
    public string? HealthConcern { get; set; }
    public string? SexualActivity { get; set; }
    public string? Contraception { get; set; }
    public string? Counseling { get; set; }
}

public class AdolescentHealthResponse
{
    public Guid Id { get; set; }
    public Guid MedicalRecordId { get; set; }
    public Guid? AppointmentId { get; set; }
    public string? HealthConcern { get; set; }
    public string? SexualActivity { get; set; }
    public string? Contraception { get; set; }
    public string? Counseling { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
}