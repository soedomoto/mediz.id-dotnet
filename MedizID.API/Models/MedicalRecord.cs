namespace MedizID.API.Models;

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
