using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace NETCoreSignalR.Domain.Interfaces
{
    public interface IRepository<T> where T : class
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
        T Get(params object[] id);
        DbContext GetDbContext();
        void SaveChanges();
        T Update(T obj);
        Task<T> AddAsync(T obj, CancellationToken cancellationToken);
        Task DisposeAsync();
        Task<T> GetAsync(CancellationToken cancellationToken, params object[] id);
        Task SaveChangesAsync(CancellationToken cancellationToken);
    }
}
