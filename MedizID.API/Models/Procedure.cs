using MedizID.API.Common.Enums;

namespace MedizID.API.Models;

public class Procedure
{
    public Guid Id { get; set; }
    public Guid AppointmentId { get; set; }
    public string ICD10PCSCode { get; set; } = null!; // ICD-10-PCS procedure code
    public string ICD10PCSDescription { get; set; } = null!; // ICD-10-PCS Medical/scientific terminology
    public string? Notes { get; set; }
    public ProcedureTypeEnum Type { get; set; } = ProcedureTypeEnum.Regular;
    public DateTime PlannedAt { get; set; }
    public DateTime? StartedAt { get; set; }
    public DateTime? FinishedAt { get; set; }
    public bool? IsRecommendedByAI { get; set; }
    public int? AIRecommendationConfidence { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? UpdatedAt { get; set; }

    // Relationships
    public Appointment Appointment { get; set; } = null!;
}
