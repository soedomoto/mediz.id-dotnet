using System;

namespace MedizID.API.Models;

/// <summary>
/// Represents a laboratory/examination result during family planning service
/// </summary>
public class FamilyPlanningLaboratoryResult
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
    /// Test/examination name (e.g., Blood Test, Urine Test, etc.)
    /// </summary>
    public string? TestName { get; set; }

    /// <summary>
    /// Test result/value
    /// </summary>
    public string? Result { get; set; }

    /// <summary>
    /// Normal range or reference value
    /// </summary>
    public string? ReferenceValue { get; set; }

    /// <summary>
    /// Test date
    /// </summary>
    public DateTime? TestDate { get; set; }

    /// <summary>
    /// Notes/remarks about the result
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
