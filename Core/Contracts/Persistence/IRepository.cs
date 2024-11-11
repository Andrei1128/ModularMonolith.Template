using Core.Abstractions;

namespace Core.Contracts.Persistence;

public interface IRepository<T> : IReadOnlyRepository<T> where T : Entity
{
    Task<T> AddAsync(T entity);
    Task<IEnumerable<T>> AddRangeAsync(IEnumerable<T> entities);
    void UpdateAsync(T entity);
    void DeleteAsync(T entity);
}
