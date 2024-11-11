using Core.Abstractions;
using Core.Contracts.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Core.Persistence.Repositories;

public abstract class Repository<T> : ReadOnlyRepository<T>, IRepository<T> where T : Entity
{
    protected new readonly WeatherManagementDbContext _dbContext;

    protected Repository(WeatherManagementDbContext dbContext) : base(dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<T> AddAsync(T entity)
    {
        await _dbContext.Set<T>().AddAsync(entity);

        return entity;
    }

    public async Task<IEnumerable<T>> AddRangeAsync(IEnumerable<T> entities)
    {
        await _dbContext.Set<T>().AddRangeAsync(entities);

        return entities;
    }

    public void UpdateAsync(T entity)
    {
        _dbContext.Entry(entity).State = EntityState.Modified;
    }

    public void DeleteAsync(T entity)
    {
        _dbContext.Set<T>().Remove(entity);
    }
}