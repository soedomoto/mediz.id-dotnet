using System;

namespace MedizID.API.Models;

/// <summary>
/// Represents a procedure performed during family planning service
/// </summary>
public class FamilyPlanningProcedure
{
    /// <summary>
    /// Unique identifier
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// Foreign key to FamilyPlanning record
    /// </summary>
    public Guid FamilyPlanningId { get; set; }

    /// <summary>
    /// Procedure name (e.g., IUD insertion, Implant placement, etc.)
    /// </summary>
    public string? ProcedureName { get; set; }

    /// <summary>
    /// Procedure date
    /// </summary>
    public DateTime? ProcedureDate { get; set; }

    /// <summary>
    /// Health worker who performed the procedure
    /// </summary>
    public string? PerformedBy { get; set; }

    /// <summary>
    /// Procedure outcome/result (Success, Partial, Failed, etc.)
    /// </summary>
    public string? Outcome { get; set; }

    /// <summary>
    /// Complications (if any)
    /// </summary>
    public string? Complications { get; set; }

    /// <summary>
    /// Notes/remarks about the procedure
    /// </summary>
    public string? Notes { get; set; }

    /// <summary>
    /// Record creation timestamp
    /// </summary>
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    /// <summary>
    /// Record last update timestamp
    /// </summary>
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

    // Navigation property
    public virtual FamilyPlanning? FamilyPlanning { get; set; }
}
