using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using MiniETrade.Application.Repositories;
using MiniETrade.Domain.Entities.Common;
using MiniETrade.Persistence.Contexts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace MiniETrade.Persistence.Persistence
{
    public class ReadRepository<T> : IReadRepository<T> where T : BaseEntity
    {
        private readonly BaseDbContext _context;

        public ReadRepository(BaseDbContext context)
        {
            _context = context;
        }

        public DbSet<T> Table => _context.Set<T>();

        public IQueryable<T> GetAll(bool tracking = true) {
            var query = Table.AsQueryable();
            if (!tracking) query = query.AsNoTracking();    //böylece arka planda çalışan tracking sisteminin çalışmamasını sağlamış olduk.
            return query;
        } 

        public async Task<T> GetByIdAsync(string Id, bool tracking = true)
        {
            //=> await Table.FirstOrDefaultAsync(x => x.Id == Guid.Parse(Id));
            //=> await Table.FindAsync(Guid.Parse(Id));
            var query  = Table.AsQueryable();
            if (!tracking) query = query.AsNoTracking();
            return await query.FirstOrDefaultAsync(data => data.Id == Guid.Parse(Id));
        }            

        public async Task<T> GetSingleAsync(Expression<Func<T, bool>> method, bool tracking = true)
        {
            var query = Table.AsQueryable();
            if (!tracking) query = Table.AsNoTracking();
            return await query.FirstOrDefaultAsync(method);
        } 
            
        public IQueryable<T> GetWhere(Expression<Func<T, bool>> method, bool tracking = true)
        {
            var query = Table.Where(method);
            if (!tracking) query = query.AsNoTracking();
            return query;
        }

        public async Task<T> GetAsync()
        {
            throw new NotImplementedException();
        }

        public Task<T?> GetAsync(Expression<Func<T, bool>> predicate, Func<IQueryable<T>, IIncludableQueryable<T, object>>? include = null, bool withDeleted = false, bool enableTracking = true, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public Task<T?> GetAsync(Expression<Func<T, bool>> predicate)
        {
            throw new NotImplementedException();
        }
    }
}

/* 
 EF arka planda tracking yaparak dataların nasıl değiştiğini takip eder ve biz SaveChanges() dediğimizde bu takibe göre SQL oluşturur. Fakat read repository'de herhangi bir
manipülasyon işlemi olmadığı için tracking'e gerek yoktur ve biz de onu kapattık.
 */