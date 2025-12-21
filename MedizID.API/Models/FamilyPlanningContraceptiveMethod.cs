using System;
using MedizID.API.Common.Enums;

namespace MedizID.API.Models;

/// <summary>
/// Represents a contraceptive method used/selected during family planning service
/// </summary>
public class FamilyPlanningContraceptiveMethod
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
    /// Contraceptive method type (IUD, Implant, Pill, Condom, etc.)
    /// </summary>
    public ContraceptiveMethod? MethodType { get; set; }

    /// <summary>
    /// Service date (tanggal pelayanan)
    /// </summary>
    public DateTime? ServiceDate { get; set; }

    /// <summary>
    /// Quantity used/dispensed
    /// </summary>
    public int? Quantity { get; set; }

    /// <summary>
    /// Notes/remarks
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
