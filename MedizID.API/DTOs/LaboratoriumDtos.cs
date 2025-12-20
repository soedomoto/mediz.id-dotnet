namespace MedizID.API.DTOs;

using System.Text.Json.Serialization;
using MedizID.API.Common.Enums;

// ========== MASTER TEST DTOs ==========

public class CreateLaboratoriumTestMasterRequest
{
    public string TestName { get; set; } = null!;
    public string? TestCode { get; set; }
    public LaboratoriumCategoryEnum Category { get; set; }
    public string? Unit { get; set; }
    public string? ReferenceRange { get; set; }
    public string? Description { get; set; }
    public SampleTypeEnum? SampleType { get; set; }
    public string? SamplePreparation { get; set; }
    public int? SortOrder { get; set; }
}

public class UpdateLaboratoriumTestMasterRequest
{
    public string? TestName { get; set; }
    public string? TestCode { get; set; }
    public LaboratoriumCategoryEnum? Category { get; set; }
    public string? Unit { get; set; }
    public string? ReferenceRange { get; set; }
    public string? Description { get; set; }
    public SampleTypeEnum? SampleType { get; set; }
    public string? SamplePreparation { get; set; }
    public int? SortOrder { get; set; }
    public bool? IsActive { get; set; }
}

public class LaboratoriumTestMasterResponse
{
    public Guid Id { get; set; }
    public string TestName { get; set; } = null!;
    public string? TestCode { get; set; }
    public string Category { get; set; } = null!;
    public string? Unit { get; set; }
    public string? ReferenceRange { get; set; }
    public string? Description { get; set; }
    public string? SampleType { get; set; }
    public string? SamplePreparation { get; set; }
    public bool IsActive { get; set; }
    public int? SortOrder { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
}

public class LaboratoriumTestMasterDetailResponse
{
    public Guid Id { get; set; }
    public string TestName { get; set; } = null!;
    public string? TestCode { get; set; }
    public LaboratoriumCategoryEnum Category { get; set; }
    public string? Unit { get; set; }
    public string? ReferenceRange { get; set; }
    public string? Description { get; set; }
    public SampleTypeEnum? SampleType { get; set; }
    public string? SamplePreparation { get; set; }
    public bool IsActive { get; set; }
    public int? SortOrder { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
}

// ========== LABORATORIUM EXECUTION DTOs ==========

// Create Request
public class CreateLaboratoriumRequest
{
    public Guid AppointmentId { get; set; }
    public Guid LaboratoriumTestMasterId { get; set; }
    public string? Result { get; set; }
    public string? Status { get; set; } // Will be converted to enum
    public DateTime? TestDate { get; set; }
    public DateTime? SampleCollectionDate { get; set; }
    public string? SampleCollectionLocation { get; set; }
    public string? LabTechnician { get; set; }
    public string? TestNotes { get; set; }
}

// Update Request
public class UpdateLaboratoriumRequest
{
    public string? Result { get; set; }
    public string? Status { get; set; }
    public DateTime? SampleCollectionDate { get; set; }
    public string? SampleCollectionLocation { get; set; }
    public string? LabTechnician { get; set; }
    public string? TestNotes { get; set; }
}

// Response
public class LaboratoriumResponse
{
    public Guid Id { get; set; }
    public Guid AppointmentId { get; set; }
    public Guid LaboratoriumTestMasterId { get; set; }
    public string TestName { get; set; } = null!;
    public string? TestCode { get; set; }
    public string Category { get; set; } = null!;
    public string? Result { get; set; }
    public string? Unit { get; set; }
    public string? ReferenceRange { get; set; }
    public string? Status { get; set; }
    public string? SampleType { get; set; }
    public DateTime? SampleCollectionDate { get; set; }
    public string? SampleCollectionLocation { get; set; }
    public string? LabTechnician { get; set; }
    public string? TestNotes { get; set; }
    public DateTime TestDate { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
}

// Detail Response with AI info
public class LaboratoriumDetailResponse
{
    public Guid Id { get; set; }
    public Guid AppointmentId { get; set; }
    public Guid LaboratoriumTestMasterId { get; set; }
    public string TestName { get; set; } = null!;
    public string? TestCode { get; set; }
    public string Category { get; set; } = null!; // Enum string representation
    public string? Result { get; set; }
    public string? Unit { get; set; }
    public string? ReferenceRange { get; set; }
    public string? Status { get; set; } // Enum string representation
    public string? SampleType { get; set; } // Enum string representation
    public DateTime? SampleCollectionDate { get; set; }
    public string? SampleCollectionLocation { get; set; }
    public string? LabTechnician { get; set; }
    public string? TestNotes { get; set; }
    public DateTime TestDate { get; set; }
    public bool? IsRecommendedByAI { get; set; }
    public int? AIRecommendationConfidence { get; set; }
    public string? AIClinicalNotes { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
}

// Batch Create Request
public class BatchCreateLaboratoriumRequest
{
    public Guid AppointmentId { get; set; }
    public List<CreateLaboratoriumTestItem> Tests { get; set; } = new();
}

public class CreateLaboratoriumTestItem
{
    public Guid LaboratoriumTestMasterId { get; set; }
    public string? Result { get; set; }
    public string? Status { get; set; }
    public DateTime? TestDate { get; set; }
    public DateTime? SampleCollectionDate { get; set; }
    public string? SampleCollectionLocation { get; set; }
    public string? LabTechnician { get; set; }
    public string? TestNotes { get; set; }
}

// Legacy schema for backward compatibility
public class PatientLaboratoriumSchema
{
    public Guid Id { get; set; }
    public Guid AppointmentId { get; set; }
    public Guid LaboratoriumTestMasterId { get; set; }
    public string TestName { get; set; } = null!;
    public string? TestCode { get; set; }
    public string? Result { get; set; }
    public string? Unit { get; set; }
    public string? ReferenceRange { get; set; }
    public string? Status { get; set; }
    public DateTime TestDate { get; set; }
    public DateTime CreatedAt { get; set; }
}

// Search Request
public class SearchLaboratoriumRequest
{
    public string? Query { get; set; }
    public string? Category { get; set; }
    public Guid? AppointmentId { get; set; }
    public int Page { get; set; } = 1;
    public int PageSize { get; set; } = 10;
}

// Test Recommendation Request
public class GenerateLaboratoriumRecommendationRequest
{
    public Guid AppointmentId { get; set; }
    public string? Symptoms { get; set; }
    public string? ClinicalFindings { get; set; }
    public int? MaxRecommendations { get; set; } = 10;
}

// Test Recommendation Item
public class LaboratoriumRecommendationItem
{
    [JsonPropertyName("testId")]
    public string? TestId { get; set; }

    [JsonPropertyName("testName")]
    public string TestName { get; set; } = null!;

    [JsonPropertyName("category")]
    public string? Category { get; set; }

    [JsonPropertyName("clinicalNotes")]
    public string? ClinicalNotes { get; set; }

    [JsonPropertyName("confidenceScore")]
    public int ConfidenceScore { get; set; }

    [JsonPropertyName("sampleType")]
    public string? SampleType { get; set; }
}

// Test Recommendation Response
public class LaboratoriumRecommendationResponse
{
    [JsonPropertyName("recommendedTests")]
    public List<LaboratoriumRecommendationItem> RecommendedTests { get; set; } = new();
}

// Batch Response
public class BatchLaboratoriumResponse
{
    public List<LaboratoriumDetailResponse> CreatedTests { get; set; } = new();
    public int TotalCreated { get; set; }
    public int TotalFailed { get; set; }
    public List<string>? FailureMessages { get; set; }
    public DateTime CreatedAt { get; set; }
}

// Patient Laboratory Tests Summary
public class PatientLaboratoriumSummary
{
    public Guid AppointmentId { get; set; }
    public List<LaboratoriumResponse> Tests { get; set; } = new();
    public int TotalTests { get; set; }
    public int NormalResults { get; set; }
    public int AbnormalResults { get; set; }
    public int CriticalResults { get; set; }
    public int PendingResults { get; set; }
    public DateTime SummaryDate { get; set; }
}

// Search Master Tests Request
public class SearchLaboratoriumTestMasterRequest
{
    public string? Query { get; set; }
    public string? Category { get; set; }
    public bool? IsActive { get; set; } = true;
    public int Page { get; set; } = 1;
    public int PageSize { get; set; } = 10;
}

// Master Tests by Category Response
public class LaboratoriumTestsByCategory
{
    public string Category { get; set; } = null!;
    public List<LaboratoriumTestMasterResponse> Tests { get; set; } = new();
    public int Count { get; set; }
}

