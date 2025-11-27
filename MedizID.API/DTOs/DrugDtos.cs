namespace MedizID.API.DTOs;

public class CreateDrugRequest
{
    public string GenericName { get; set; } = null!;
    public string? BrandName { get; set; }
    public string? Dosage { get; set; }
    public Guid DrugCategoryId { get; set; }
    public string? Manufacturer { get; set; }
    public string? IndividualPrice { get; set; }
}

public class UpdateDrugRequest
{
    public string? GenericName { get; set; }
    public string? BrandName { get; set; }
    public string? Dosage { get; set; }
    public Guid? DrugCategoryId { get; set; }
    public string? Manufacturer { get; set; }
    public string? IndividualPrice { get; set; }
    public bool? IsActive { get; set; }
}

public class DrugResponse
{
    public Guid Id { get; set; }
    public string GenericName { get; set; } = null!;
    public string? BrandName { get; set; }
    public string? Dosage { get; set; }
    public Guid DrugCategoryId { get; set; }
    public string? Manufacturer { get; set; }
    public string? IndividualPrice { get; set; }
    public bool IsActive { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
}

public class CreateDrugCategoryRequest
{
    public string Name { get; set; } = null!;
    public string? Description { get; set; }
}

public class UpdateDrugCategoryRequest
{
    public string? Name { get; set; }
    public string? Description { get; set; }
    public bool? IsActive { get; set; }
}

public class DrugCategoryResponse
{
    public Guid Id { get; set; }
    public string Name { get; set; } = null!;
    public string? Description { get; set; }
    public bool IsActive { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
}

public class CheckDrugInteractionRequest
{
    public List<Guid> DrugIds { get; set; } = new();
}

public class DrugInteractionCheckResponse
{
    public List<DrugInteractionDetail> Interactions { get; set; } = new();
    public bool HasInteractions { get; set; }
}

public class DrugInteractionDetail
{
    public Guid Drug1Id { get; set; }
    public Guid Drug2Id { get; set; }
    public string? Drug1Name { get; set; }
    public string? Drug2Name { get; set; }
    public string? InteractionSeverity { get; set; }
    public string? Description { get; set; }
}

public class DrugSearchRequest
{
    public string? Query { get; set; }
    public Guid? CategoryId { get; set; }
}

public class DrugSearchResponse
{
    public List<DrugResponse> Results { get; set; } = new();
}
