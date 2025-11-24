using MedizID.API.DTOs;

namespace MedizID.API.Services;

/// <summary>
/// Service interface for drug operations
/// </summary>
public interface IDrugService
{
    /// <summary>
    /// Get drug by ID
    /// </summary>
    Task<DrugResponse?> GetDrugAsync(Guid drugId, CancellationToken cancellationToken = default);

    /// <summary>
    /// Get all drugs
    /// </summary>
    Task<IEnumerable<DrugResponse>> GetAllDrugsAsync(CancellationToken cancellationToken = default);

    /// <summary>
    /// Get drugs by category
    /// </summary>
    Task<IEnumerable<DrugResponse>> GetDrugsByCategoryAsync(Guid categoryId, CancellationToken cancellationToken = default);

    /// <summary>
    /// Search drugs
    /// </summary>
    Task<DrugSearchResponse> SearchDrugsAsync(DrugSearchRequest request, CancellationToken cancellationToken = default);

    /// <summary>
    /// Create new drug
    /// </summary>
    Task<DrugResponse> CreateDrugAsync(CreateDrugRequest request, CancellationToken cancellationToken = default);

    /// <summary>
    /// Update drug
    /// </summary>
    Task<DrugResponse> UpdateDrugAsync(Guid drugId, UpdateDrugRequest request, CancellationToken cancellationToken = default);

    /// <summary>
    /// Get drug categories
    /// </summary>
    Task<IEnumerable<DrugCategoryResponse>> GetDrugCategoriesAsync(CancellationToken cancellationToken = default);

    /// <summary>
    /// Create drug category
    /// </summary>
    Task<DrugCategoryResponse> CreateDrugCategoryAsync(CreateDrugCategoryRequest request, CancellationToken cancellationToken = default);
}
