using MedizID.API.Models;

namespace MedizID.API.Repositories;

/// <summary>
/// Specialized repository for drug operations
/// </summary>
public interface IDrugRepository : IRepository<Drug>
{
    /// <summary>
    /// Get drug by generic name
    /// </summary>
    Task<Drug?> GetByGenericNameAsync(string genericName, CancellationToken cancellationToken = default);

    /// <summary>
    /// Get drugs by category
    /// </summary>
    Task<IEnumerable<Drug>> GetByCategoryAsync(Guid categoryId, CancellationToken cancellationToken = default);

    /// <summary>
    /// Get drugs by manufacturer
    /// </summary>
    Task<IEnumerable<Drug>> GetByManufacturerAsync(string manufacturer, CancellationToken cancellationToken = default);

    /// <summary>
    /// Search drugs by name (generic or brand)
    /// </summary>
    Task<IEnumerable<Drug>> SearchAsync(string searchTerm, CancellationToken cancellationToken = default);

    /// <summary>
    /// Get drug with interactions
    /// </summary>
    Task<Drug?> GetWithInteractionsAsync(Guid drugId, CancellationToken cancellationToken = default);

    /// <summary>
    /// Get available drugs in stock
    /// </summary>
    Task<IEnumerable<Drug>> GetAvailableAsync(CancellationToken cancellationToken = default);
}
