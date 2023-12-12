// <copyright file="IDataRepository.cs" company="Cheffo Org">
// Copyright (c) Cheffo Org. All rights reserved.
// </copyright>

using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Query;

namespace LifeQuality.DataContext.Repository
{
    public interface IDataRepository<TEntity>
    {
        Task<List<TEntity>> AllAsync(bool trackChanges = false, params Expression<Func<TEntity, object>>[] includes);

        Task<TEntity> GetByAsync(Expression<Func<TEntity, bool>> filter, bool ignoreQueryFilters = false, params Expression<Func<TEntity, object>>[] includes);

        Task<List<TEntity>> GetByManyAsync(Expression<Func<TEntity, bool>> filter, params Expression<Func<TEntity, object>>[] includes);

        public Task<List<TEntity>> GetAllAsync(
            Expression<Func<TEntity, bool>> predicate = null,
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
            Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>> include = null,
            bool disableTracking = true,
            bool ignoreQueryFilters = false);

        public Task<List<TResult>> GetSelectAsync<TResult>(
            Expression<Func<TEntity, TResult>> selector,
            Expression<Func<TEntity, bool>> predicate = null,
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
            Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>> include = null,
            bool disableTracking = true,
            bool ignoreQueryFilters = false);

        public Task<TEntity> GetFirstOrDefaultAsync(
            Expression<Func<TEntity, bool>> predicate = null,
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
            Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>> include = null,
            bool disableTracking = true,
            bool ignoreQueryFilters = false);

        void AddNew(TEntity entity);

        void Update(TEntity entity, bool withTimeUpdate = true);

        Task Remove(int entityId);
        Task SaveAsync();
        public EntityEntry Entry(TEntity entity);
    }
}
