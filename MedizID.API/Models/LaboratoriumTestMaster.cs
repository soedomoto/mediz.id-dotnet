using MedizID.API.Common.Enums;

namespace MedizID.API.Models;

public class LaboratoriumTestMaster
{
    public Guid Id { get; set; }
    
    // Test Information
    public string TestName { get; set; } = null!;
    public string? TestCode { get; set; } // BPJS code or internal code
    public LaboratoriumCategoryEnum Category { get; set; }
    
    // Test Details
    public string? Unit { get; set; } // Measurement unit (gr%, mgr/dl, /mm³, u/l, etc.)
    public string? ReferenceRange { get; set; } // Normal reference values
    public string? Description { get; set; } // Test description
    
    // Sample Information
    public SampleTypeEnum? SampleType { get; set; } // Type of sample required
    public string? SamplePreparation { get; set; } // Special preparation instructions
    
    // Status and Metadata
    public bool IsActive { get; set; } = true;
    public int? SortOrder { get; set; } // For UI ordering
    
    // Timestamps
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? UpdatedAt { get; set; }

    // Relationships
    public ICollection<Laboratorium> LaboratoriumTests { get; set; } = new List<Laboratorium>();
}
