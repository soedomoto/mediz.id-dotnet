namespace MedizID.API.DTOs;

public class CreateMedicalEquipmentRequest
{
    public string Name { get; set; } = null!;
    public Guid EquipmentTypeId { get; set; }
    public string? SerialNumber { get; set; }
    public DateTime? PurchaseDate { get; set; }
    public DateTime? LastServiceDate { get; set; }
}

public class UpdateMedicalEquipmentRequest
{
    public string? Name { get; set; }
    public Guid? EquipmentTypeId { get; set; }
    public string? SerialNumber { get; set; }
    public DateTime? PurchaseDate { get; set; }
    public DateTime? LastServiceDate { get; set; }
    public bool? IsActive { get; set; }
}

public class MedicalEquipmentResponse
{
    public Guid Id { get; set; }
    public string Name { get; set; } = null!;
    public Guid EquipmentTypeId { get; set; }
    public string? SerialNumber { get; set; }
    public Guid FacilityId { get; set; }
    public DateTime? PurchaseDate { get; set; }
    public DateTime? LastServiceDate { get; set; }
    public bool IsActive { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
}

public class CreateMedicalEquipmentTypeRequest
{
    public string Name { get; set; } = null!;
    public string? Description { get; set; }
}

public class UpdateMedicalEquipmentTypeRequest
{
    public string? Name { get; set; }
    public string? Description { get; set; }
    public bool? IsActive { get; set; }
}

public class MedicalEquipmentTypeResponse
{
    public Guid Id { get; set; }
    public string Name { get; set; } = null!;
    public string? Description { get; set; }
    public bool IsActive { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
}

public class EquipmentSearchRequest
{
    public string? Query { get; set; }
    public Guid? EquipmentTypeId { get; set; }
}

public class EquipmentSearchResponse
{
    public List<MedicalEquipmentResponse> Results { get; set; } = new();
}
