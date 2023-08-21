using Microsoft.EntityFrameworkCore.Query;
using MiniETrade.Application.Common.Abstractions.Persistence.Dynamic;
using MiniETrade.Domain.Entities.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace MiniETrade.Application.Repositories
{
    //Burda yazılanlar IReadRepository ve IWriteRepository'ye dağıtılacak.
    public interface IAsyncRepository<TEntity> where TEntity : BaseEntity
    {
        Task<TEntity?> GetAsync(
            Expression<Func<TEntity, bool>> predicate,
            Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>>? include = null,
            bool isActive = false,
            bool enableTracking = true,
            CancellationToken cancellation = default
            );

        Task<Paginate<TEntity?>> GetListAsync(
            Expression<Func<TEntity, bool>>? predicate = null,
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>? orderBy= null,
            Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>>? include = null,
            int index = 0,
            int size = 10,
            bool isActive = false,
            bool enableTracking = true,
            CancellationToken cancellation = default
            );

        Task<Paginate<TEntity?>> GetListByDynamicAsync(
            DynamicQuery dynamic,
            Expression<Func<TEntity, bool>>? predicate = null,
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>? orderBy = null,
            Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>>? include = null,
            int index = 0,
            int size = 10,
            bool isActive = false,
            bool enableTracking = true,
            CancellationToken cancellation = default
            );

        Task<bool> AnyAsync(
            Expression<Func<TEntity, bool>>? predicate = null,
            bool isActive = false,
            bool enableTracking = true,
            CancellationToken cancellation = default
        );

        Task<TEntity> AddAsync(TEntity entity);
        Task<ICollection<TEntity>> AddRangeAsync(ICollection<TEntity> entities);
        Task<TEntity> UpdateAsync(TEntity entity);
        Task<ICollection<TEntity>> UpdateRangeAsync(ICollection<TEntity> entities);
        Task<TEntity> DeleteAsync(TEntity entity, bool deletePermanently = false);
        Task<ICollection<TEntity>> DeleteRangeAsync(ICollection<TEntity> entities,bool deletePermanently = false);

        IQueryable<TEntity> Query();
    }
}
