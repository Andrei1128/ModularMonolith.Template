using Core.Abstractions;
using Core.Contracts.Persistence;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace Core.Persistence;

public class WeatherManagementDbContext(DbContextOptions<WeatherManagementDbContext> contextOptions) : DbContext(contextOptions), IUnitOfWork
{
    public DbSet<WeatherForecastEntity> WeatherForecast { get; set; }

    public async Task SaveChangesAsync()
    {
        await SaveChangesAsync();
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        var entitiesClrTypes = modelBuilder.Model.GetEntityTypes()
                                                 .Where(entityType => typeof(ISoftDeletable).IsAssignableFrom(entityType.ClrType))
                                                 .Select(entityType => entityType.ClrType);

        foreach (var clrType in entitiesClrTypes)
        {
            var parameter = Expression.Parameter(clrType);

            var body = Expression.Equal(Expression.Property(parameter, nameof(ISoftDeletable.IsDeleted)), Expression.Constant(true));

            var filter = Expression.Lambda(body, parameter);

            modelBuilder.Entity(clrType).HasQueryFilter(filter);
        }
    }

    public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = new CancellationToken())
    {
        foreach (var entry in ChangeTracker.Entries<Entity>())
        {
            switch (entry.State)
            {
                case EntityState.Added:
                    entry.Entity.CreatedAt = DateTime.Now;
                    entry.Entity.CreatedBy = UserManager.Name;
                    break;

                case EntityState.Modified:
                    entry.Entity.LastModifiedAt = DateTime.Now;
                    entry.Entity.LastModifiedBy = UserManager.Name;
                    break;

                case EntityState.Deleted:
                    entry.Entity.IsDeleted = true;
                    entry.Entity.LastModifiedAt = DateTime.Now;
                    entry.Entity.LastModifiedBy = UserManager.Name;
                    entry.State = EntityState.Modified;
                    break;
            }
        }
        return base.SaveChangesAsync(cancellationToken);
    }
}