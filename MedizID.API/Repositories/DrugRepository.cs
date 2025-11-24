using Microsoft.EntityFrameworkCore;
using MedizID.API.Data;
using MedizID.API.Models;

namespace MedizID.API.Repositories;

/// <summary>
/// Drug repository with specialized query methods
/// </summary>
public class DrugRepository : BaseRepository<Drug>, IDrugRepository
{
    public DrugRepository(MedizIDDbContext context) : base(context)
    {
    }

    public async Task<Drug?> GetByGenericNameAsync(string genericName, CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .FirstOrDefaultAsync(d => d.GenericName.ToLower() == genericName.ToLower(), cancellationToken);
    }

    public async Task<IEnumerable<Drug>> GetByCategoryAsync(Guid categoryId, CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .Where(d => d.DrugCategoryId == categoryId)
            .OrderBy(d => d.GenericName)
            .ToListAsync(cancellationToken);
    }

    public async Task<IEnumerable<Drug>> GetByManufacturerAsync(string manufacturer, CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .Where(d => d.Manufacturer != null && d.Manufacturer.Contains(manufacturer))
            .OrderBy(d => d.GenericName)
            .ToListAsync(cancellationToken);
    }

    public async Task<IEnumerable<Drug>> SearchAsync(string searchTerm, CancellationToken cancellationToken = default)
    {
        var lowerSearch = searchTerm.ToLower();
        return await _dbSet
            .Where(d => d.GenericName.ToLower().Contains(lowerSearch) || 
                       (d.BrandName != null && d.BrandName.ToLower().Contains(lowerSearch)))
            .OrderBy(d => d.GenericName)
            .ToListAsync(cancellationToken);
    }

    public async Task<Drug?> GetWithInteractionsAsync(Guid drugId, CancellationToken cancellationToken = default)
    {
        // Drug doesn't have direct interactions relationship, return drug without includes
        return await _dbSet
            .FirstOrDefaultAsync(d => d.Id == drugId, cancellationToken);
    }

    public async Task<IEnumerable<Drug>> GetAvailableAsync(CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .OrderBy(d => d.GenericName)
            .ToListAsync(cancellationToken);
    }
}
