using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;

namespace LifeQuality.DataContext
{
    public interface IDataContext
    {
        DatabaseFacade Database { get; }

        DbSet<TEntity> Set<TEntity>()
            where TEntity : class;

        EntityEntry Entry(object entity);

        Task<int> DefaultEFSaveChangesAsync(CancellationToken cancellationToken = default);

        int SaveChanges();

        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);

        ChangeTracker ChangeTracker { get; }
    }
}