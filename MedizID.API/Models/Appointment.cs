using MedizID.API.Common.Enums;

namespace MedizID.API.Models;

public class Appointment
{
    public Guid Id { get; set; }
    public Guid FacilityId { get; set; }
    public Guid FacilityPatientId { get; set; }
    public Guid? FacilityDoctorId { get; set; }
    public DateTime AppointmentDate { get; set; }
    public TimeSpan AppointmentTime { get; set; }
    public AppointmentStatusEnum Status { get; set; } = AppointmentStatusEnum.Scheduled;
    public string? Reason { get; set; }
    public string? Notes { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? UpdatedAt { get; set; }

    // Relationships
    public Facility Facility { get; set; } = null!;
    public FacilityPatient FacilityPatient { get; set; } = null!;
    public FacilityStaff? FacilityDoctor { get; set; }
    public ICollection<Anamnesis> Anamnesis { get; set; } = new List<Anamnesis>();
    public ICollection<Diagnosis> Diagnoses { get; set; } = new List<Diagnosis>();
    public ICollection<Prescription> Prescriptions { get; set; } = new List<Prescription>();
    public ICollection<Procedure> Procedures { get; set; } = new List<Procedure>();
    public ICollection<Laboratorium> LaboratoriumTests { get; set; } = new List<Laboratorium>();
}
