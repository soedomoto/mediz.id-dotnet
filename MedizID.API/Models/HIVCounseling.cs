using MedizID.API.Common.Enums;

namespace MedizID.API.Models;

public class HIVCounseling
{
    public Guid Id { get; set; }
    public Guid AppointmentId { get; set; }
    public HIVCounselingVisitStatusEnum VisitStatus { get; set; }
    public HIVCounselingClientStatusEnum ClientStatus { get; set; }
    public HIVCounselingRiskGroupEnum? RiskGroup { get; set; }
    public HIVCounselingPregnancyStatusEnum? PregnancyStatus { get; set; }
    public HIVCounselingPartnerHIVStatusEnum? PartnerHIVStatus { get; set; }
    public HIVCounselingTestReasonEnum? TestReason { get; set; }
    public HIVCounselingTestSourceEnum? TestSource { get; set; }
    public HIVCounselingPreviousTestResultEnum? PreviousTestResult { get; set; }
    public DateTime? LastTestDate { get; set; }
    public string? TestResult { get; set; }
    public string? Notes { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    // Relationships
    public Appointment Appointment { get; set; } = null!;
}
