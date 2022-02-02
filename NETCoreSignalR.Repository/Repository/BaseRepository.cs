using Dawn;
using Microsoft.EntityFrameworkCore;
using NETCoreSignalR.Domain.Interfaces;
using NETCoreSignalR.Repository.Configurations;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace NETCoreSignalR.Repository.Repository
{    
    public abstract class BaseRepository<T> : IRepository<T> where T : class
    {
        public MyDbContext _dbContext;
        public BaseRepository(MyDbContext context)
        {
            _dbContext = context;
        }
        public virtual T Add(T obj) => _dbContext.Add(obj).Entity;

        public virtual async Task<T> AddAsync(T obj, CancellationToken cancellationToken)
        {
           var result =  await _dbContext.AddAsync(obj, cancellationToken);

            return result.Entity;
        }
        public virtual T Update(T obj)
        {
            _dbContext.Entry(obj).State = EntityState.Modified;

            return _dbContext.Update(obj).Entity;
        }
        public virtual void Delete(T obj)
        {
            _dbContext.Entry(obj).State = EntityState.Deleted;
            _dbContext.Remove(obj);
        }
        public virtual IQueryable<T> Get() => _dbContext.Set<T>().AsQueryable();
        public virtual ParallelQuery<T> GetParallel() => _dbContext.Set<T>().AsParallel();

        public virtual IQueryable<T> Get(Expression<Func<T, bool>> filter = null)
        {
            var dados = _dbContext.Set<T>().AsQueryable();
            return filter != null ? dados.Where(filter) : dados;
        }
        public virtual ParallelQuery<T> GetParallel(Expression<Func<T, bool>> filter = null)
        {
            var dados = _dbContext.Set<T>();
            return filter != null ? dados.Where(filter).AsParallel() : dados.AsParallel();
        }
        public virtual IQueryable<T> Get(Expression<Func<T, bool>> filter = null, Expression<Func<T, object>> order = null, int? count = 0, int? skip = 0, bool reverse = false)
        {
            var dados = _dbContext.Set<T>().AsQueryable();

            if (filter != null)
                dados = dados.Where(filter);
            if (order != null)
                dados = reverse ? dados.OrderByDescending(order) : dados.OrderBy(order);
            if (count > 0 && skip > 0)
                dados = dados.Take((int)count).Skip((int)skip);

            return dados;
        }
        public virtual ParallelQuery<T> GetParallel(Expression<Func<T, bool>> filter = null, Expression<Func<T, object>> order = null, int? count = 0, int? skip = 0, bool reverse = false)
        {
            var dados = _dbContext.Set<T>().AsQueryable();

            if (filter != null)
                dados = dados.Where(filter);
            if (order != null)
                dados = reverse ? dados.OrderByDescending(order) : dados.OrderBy(order);
            if (count > 0 && skip > 0)
                dados = dados.Take((int)count).Skip((int)skip);

            return dados.AsParallel();
        }
        public virtual T Get(params object[] param) 
        {
            Guard.Argument(param, nameof(param)).NotEmpty().DoesNotContainNull();

            return _dbContext.Set<T>().Find(param);
        }
        public virtual async Task<T> GetAsync(CancellationToken cancellationToken, params object[] param)
        {
            Guard.Argument(param, nameof(param)).NotEmpty().DoesNotContainNull();

            return await _dbContext.Set<T>().FindAsync(param, cancellationToken);
        }
        public DbContext GetDbContext() => _dbContext;

        [ExcludeFromCodeCoverage]
        public void SaveChanges() => _dbContext.SaveChanges();
        [ExcludeFromCodeCoverage]
        public async Task SaveChangesAsync(CancellationToken cancellationToken) => await _dbContext.SaveChangesAsync(cancellationToken);
        [ExcludeFromCodeCoverage]
        public void Dispose() => _dbContext.Dispose();
        [ExcludeFromCodeCoverage]
        public async Task DisposeAsync() => await _dbContext.DisposeAsync();

    }
}
