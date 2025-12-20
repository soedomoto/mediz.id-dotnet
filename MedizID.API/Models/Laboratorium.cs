using MedizID.API.Common.Enums;

namespace MedizID.API.Models;

public class Laboratorium
{
    public Guid Id { get; set; }
    public Guid AppointmentId { get; set; }
    public Guid LaboratoriumTestMasterId { get; set; }
    
    // Test Execution Results
    public string? Result { get; set; } // Hasil (test result value)
    public LaboratoriumStatusEnum? Status { get; set; } // Status: Normal, Abnormal, Critical, etc.
    public DateTime TestDate { get; set; } = DateTime.UtcNow;
    
    // Sample Information
    public DateTime? SampleCollectionDate { get; set; }
    public string? SampleCollectionLocation { get; set; } // Where sample was collected (lab, ward, etc.)
    
    // Test Execution Details
    public string? LabTechnician { get; set; } // Person who conducted the test
    public string? TestNotes { get; set; } // Additional notes about the test
    
    // AI Recommendations
    public bool? IsRecommendedByAI { get; set; }
    public int? AIRecommendationConfidence { get; set; }
    public string? AIClinicalNotes { get; set; } // AI reasoning for the test recommendation
    
    // Timestamps
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? UpdatedAt { get; set; }

    // Relationships
    public Appointment Appointment { get; set; } = null!;
    public LaboratoriumTestMaster LaboratoriumTestMaster { get; set; } = null!;
}
