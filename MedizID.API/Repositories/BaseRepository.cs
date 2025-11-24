using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using MedizID.API.Data;

namespace MedizID.API.Repositories;

/// <summary>
/// Generic base repository with common CRUD operations
/// </summary>
/// <typeparam name="T">Entity type</typeparam>
public abstract class BaseRepository<T> : IRepository<T> where T : class
{
    protected readonly MedizIDDbContext _context;
    protected readonly DbSet<T> _dbSet;

    protected BaseRepository(MedizIDDbContext context)
    {
        _context = context;
        _dbSet = context.Set<T>();
    }

    public virtual async Task<T?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await _dbSet.FindAsync(new object[] { id }, cancellationToken);
    }

    public virtual async Task<IEnumerable<T>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return await _dbSet.ToListAsync(cancellationToken);
    }

    public virtual async Task<IEnumerable<T>> GetAsync(
        Expression<Func<T, bool>>? predicate = null,
        CancellationToken cancellationToken = default)
    {
        IQueryable<T> query = _dbSet;

        if (predicate != null)
            query = query.Where(predicate);

        return await query.ToListAsync(cancellationToken);
    }

    public virtual async Task<T?> GetFirstOrDefaultAsync(
        Expression<Func<T, bool>>? predicate = null,
        CancellationToken cancellationToken = default)
    {
        IQueryable<T> query = _dbSet;

        if (predicate != null)
            query = query.Where(predicate);

        return await query.FirstOrDefaultAsync(cancellationToken);
    }

    public virtual async Task<bool> ExistsAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var entity = await GetByIdAsync(id, cancellationToken);
        return entity != null;
    }

    public virtual async Task<T> AddAsync(T entity, CancellationToken cancellationToken = default)
    {
        await _dbSet.AddAsync(entity, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
        return entity;
    }

    public virtual async Task AddRangeAsync(IEnumerable<T> entities, CancellationToken cancellationToken = default)
    {
        await _dbSet.AddRangeAsync(entities, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
    }

    public virtual async Task UpdateAsync(T entity, CancellationToken cancellationToken = default)
    {
        _dbSet.Update(entity);
        await _context.SaveChangesAsync(cancellationToken);
    }

    public virtual async Task<bool> DeleteAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var entity = await GetByIdAsync(id, cancellationToken);
        if (entity == null)
            return false;

        return await DeleteAsync(entity, cancellationToken);
    }

    public virtual async Task<bool> DeleteAsync(T entity, CancellationToken cancellationToken = default)
    {
        _dbSet.Remove(entity);
        var result = await _context.SaveChangesAsync(cancellationToken);
        return result > 0;
    }

    public virtual async Task<int> DeleteRangeAsync(IEnumerable<T> entities, CancellationToken cancellationToken = default)
    {
        _dbSet.RemoveRange(entities);
        return await _context.SaveChangesAsync(cancellationToken);
    }

    public virtual async Task<int> CountAsync(
        Expression<Func<T, bool>>? predicate = null,
        CancellationToken cancellationToken = default)
    {
        IQueryable<T> query = _dbSet;

        if (predicate != null)
            query = query.Where(predicate);

        return await query.CountAsync(cancellationToken);
    }
}
