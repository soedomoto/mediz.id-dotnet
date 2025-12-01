using MedizID.API.Common.Enums;

namespace MedizID.API.Models;

public class STI
{
    public Guid Id { get; set; }
    public Guid AppointmentId { get; set; }
    public STIVisitStatusEnum VisitStatus { get; set; }
    public STIRiskGroupEnum? RiskGroup { get; set; }
    public STIVisitReasonEnum? VisitReason { get; set; }
    public string? Symptoms { get; set; }
    public string? DiagnosisSTI { get; set; }
    public string? Treatment { get; set; }
    public string? Partner { get; set; }
    public string? Notes { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    // Relationships
    public Appointment Appointment { get; set; } = null!;
}
