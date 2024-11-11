using Core.Abstractions;
using System.Linq.Expressions;

namespace Core.Contracts.Persistence;

public interface IReadOnlyRepository<T> where T : Entity
{
    Task<T?> GetByIdAsync(int id, params Expression<Func<T, object>>[] includes);
    Task<T?> GetFirstOrDefaultAsync(Expression<Func<T, bool>>? filters = null, params Expression<Func<T, object>>[] includes);
    Task<List<T>> QueryAsync(Expression<Func<T, bool>>? filters = null, params Expression<Func<T, object>>[] includes);
    //Task<PaginationResult<T>> QueryPaginatedAsync(GridifyQuery gridifyQuery, Expression<Func<T, bool>>? additionalFilters = null, params Expression<Func<T, object>>[] includes);
}
