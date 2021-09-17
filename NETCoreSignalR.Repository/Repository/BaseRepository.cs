using Microsoft.EntityFrameworkCore;
using NETCoreSignalR.Repository.Configurations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace NETCoreSignalR.Repository.Repository
{
    public interface IRepository<T> where T :class
    {
        T Add(T obj);
        void Delete(T obj);
        void Dispose();
        IQueryable<T> Get();
        ParallelQuery<T> GetParallel();
        ParallelQuery<T> GetParallel(Expression<Func<T, bool>> filter = null);
        ParallelQuery<T> GetParallel(Expression<Func<T, bool>> filter = null, Expression<Func<T, object>> order = null, int? count = 0, int? skip = 0, bool reverse = false);
        IQueryable<T> Get(Expression<Func<T, bool>> filter = null);
        IQueryable<T> Get(Expression<Func<T, bool>> filter = null, Expression<Func<T, object>> order = null, int? count = 0, int? skip = 0, bool reverse = false);
        T Get(params object [] id);
        DbContext GetDbContext();
        void SaveChanges();
        T Update(T obj);
        Task<T> AddAsync(T obj);
        Task DisposeAsync();
        Task<T> GetAsync(CancellationToken cancellationToken, params object[] id);
        Task SaveChangesAsync();
    }

    public abstract class BaseRepository<T> : IRepository<T> where T : class
    {
        public MyDbContext _dbContext;
        public BaseRepository(MyDbContext context)
        {
            _dbContext = context;
        }
        public virtual T Add(T obj) => _dbContext.Add(obj).Entity;

        public virtual async Task<T> AddAsync(T obj)
        {
           var result =  await _dbContext.AddAsync(obj);

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
        public virtual T Get(params object[] param) => param?.Length < 0 ? throw new ArgumentException("ID can't be null or empty.") : _dbContext.Set<T>().Find(param);
        public virtual async Task<T> GetAsync(CancellationToken cancellationToken, params object[] param) => param?.Length < 0 ? throw new ArgumentException("ID  can't be null or empty.") : await _dbContext.Set<T>().FindAsync(param, cancellationToken);

        public DbContext GetDbContext() => _dbContext;

        public void SaveChanges() => _dbContext.SaveChanges();
        public async Task SaveChangesAsync() => await _dbContext.SaveChangesAsync();
        public void Dispose() => _dbContext.Dispose();
        public async Task DisposeAsync() => await _dbContext.DisposeAsync();

    }
}
