using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using MiniETrade.Application.Common.Abstractions.Persistence.Dynamic;
using MiniETrade.Application.Repositories;
using MiniETrade.Domain.Entities.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Net.Mime;
using System.Text;
using System.Threading.Tasks;

namespace MiniETrade.Persistence.Persistence;

public class EfAsyncRepository<TEntity, TContext> : IAsyncRepository<TEntity> where TEntity : BaseEntity where TContext : DbContext
{
    protected readonly TContext Context;
    public EfAsyncRepository(TContext context)
    {
        Context = context;
    }
    public async Task<TEntity> AddAsync(TEntity entity)
    {
        await Context.AddAsync(entity);
        await Context.SaveChangesAsync();
        return entity; //Burada DB'ye eklenen entityi döndürmüş oluyoruz.
    }

    public async Task<ICollection<TEntity>> AddRangeAsync(ICollection<TEntity> entities)
    {
        await Context.AddRangeAsync(entities);
        await Context.SaveChangesAsync();
        return entities;
    }

    public Task<bool> AnyAsync(Expression<Func<TEntity, bool>>? predicate = null, bool isActive = true, bool enableTracking = true, CancellationToken cancellation = default)
    {
        IQueryable<TEntity> query = Query();
        if (!enableTracking) query = query.AsNoTracking();
        if (!isActive) query = query.IgnoreQueryFilters();
        if (predicate is not null) query = query.Where(predicate);

        return query.AnyAsync(cancellation);
    }

    public async Task<TEntity> DeleteAsync(TEntity entity, bool deletePermanently = false)
    {
        await SetEntityAsDeletedAsync(entity, deletePermanently);
        await Context.SaveChangesAsync();
        return entity;
    }

    

    public Task<ICollection<TEntity>> DeleteRangeAsync(ICollection<TEntity> entities, bool deletePermanently = false)
    {
        throw new NotImplementedException();
    }

    public Task<TEntity?> GetAsync(Expression<Func<TEntity, bool>> predicate, Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>>? include = null, bool isActive = false, bool enableTracking = true, CancellationToken cancellation = default)
    {
        throw new NotImplementedException();
    }

    public Task<Paginate<TEntity?>> GetListAsync(Expression<Func<TEntity, bool>>? predicate = null, Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>? orderBy = null, Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>>? include = null, int index = 0, int size = 10, bool isActive = false, bool enableTracking = true, CancellationToken cancellation = default)
    {
        throw new NotImplementedException();
    }

    public Task<Paginate<TEntity?>> GetListByDynamicAsync(DynamicQuery dynamic, Expression<Func<TEntity, bool>>? predicate = null, Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>? orderBy = null, Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>>? include = null, int index = 0, int size = 10, bool isActive = false, bool enableTracking = true, CancellationToken cancellation = default)
    {
        throw new NotImplementedException();
    }

    public IQueryable<TEntity> Query()
    {
        throw new NotImplementedException();
    }

    public Task<TEntity> UpdateAsync(TEntity entity)
    {
        throw new NotImplementedException();
    }

    public Task<ICollection<TEntity>> UpdateRangeAsync(ICollection<TEntity> entities)
    {
        throw new NotImplementedException();
    }

    protected async Task SetEntityAsDeletedAsync(TEntity entity, bool deletePermanently)
    {
        if (deletePermanently)
           Context.Remove(entity);

        if (!deletePermanently)
        {
            CheckIfEntityHasOneToOneRelation(entity);
            await SetEntityAsSoftDeletedAsync(entity);
        }
    }

    protected void CheckIfEntityHasOneToOneRelation(TEntity entity)
    {
        bool hasEntityHaveOneToOneRelation = Context
            .Entry(entity)
            .Metadata.GetForeignKeys()
            .All(
            x =>
                x.DependentToPrincipal?.IsCollection == true
                || x.PrincipalToDependent?.IsCollection == true
                || x.DependentToPrincipal?.ForeignKey.DeclaringEntityType.ClrType == entity.GetType()
            ) == false;
        if (hasEntityHaveOneToOneRelation)
            throw new InvalidOperationException(
                "Entity has one-to-one relationship.Soft delete causes problems if you try to entry again by some foreigner");
    }
}
