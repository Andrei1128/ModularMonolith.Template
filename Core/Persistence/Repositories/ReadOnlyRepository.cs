using Core.Abstractions;
using Core.Contracts.Persistence;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace Core.Persistence.Repositories;

public abstract class ReadOnlyRepository<T> : IReadOnlyRepository<T> where T : Entity
{
    protected readonly DbContext _dbContext;

    protected ReadOnlyRepository(DbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<T?> GetByIdAsync(int id, params Expression<Func<T, object>>[] includes)
    {
        var query = _dbContext.Set<T>().AsNoTracking();

        if (includes != null)
        {
            foreach (var include in includes)
            {
                query = query.Include(include).DefaultIfEmpty()!;
            }
        }

        return await query.SingleOrDefaultAsync(entity => entity.Id == id);
    }

    public async Task<T?> GetFirstOrDefaultAsync(Expression<Func<T, bool>>? filters = null, params Expression<Func<T, object>>[] includes)
    {
        var query = _dbContext.Set<T>().AsNoTracking();

        if (includes != null)
        {
            foreach (var include in includes)
            {
                query = query.Include(include).DefaultIfEmpty()!;
            }
        }

        if (filters != null)
        {
            query = query.Where(filters);
        }

        return await query.FirstOrDefaultAsync();
    }

    public async Task<List<T>> QueryAsync(Expression<Func<T, bool>>? filters = null, params Expression<Func<T, object>>[] includes)
    {
        var query = _dbContext.Set<T>().AsNoTracking();

        if (includes != null)
        {
            foreach (var include in includes)
            {
                query = query.Include(include).DefaultIfEmpty()!;
            }
        }

        if (filters != null)
        {
            query = query.Where(filters);
        }

        return await query.ToListAsync();
    }

    //public async Task<PaginationResult<T>> QueryPaginatedAsync(GridifyQuery gridifyQuery, Expression<Func<T, bool>>? additionalFilters = null, params Expression<Func<T, object>>[] includes)
    //{
    //    var query = _dbContext.Set<T>().AsNoTracking();

    //    if (includes != null)
    //    {
    //        foreach (var include in includes)
    //        {
    //            query = query.Include(include).DefaultIfEmpty()!;
    //        }
    //    }

    //    if (additionalFilters != null)
    //    {
    //        query = query.Where(additionalFilters);
    //    }

    //    query = query.ApplyFiltering(gridifyQuery)
    //                  .ApplyOrdering(gridifyQuery)
    //                  .ApplyPaging(gridifyQuery.Page + 1, gridifyQuery.PageSize);

    //    var totalEntries = await query.CountAsync();
    //    var data = await query.ToListAsync();

    //    return new PaginationResult<T>
    //    {
    //        Data = data,
    //        TotalEntries = totalEntries
    //    };
    //}
}