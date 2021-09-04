﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace NETCoreSignalR.Repository.Repository
{
    public interface IBaseRepository<T>
    {
        T Add(T obj);
        void Delete(T obj);
        void Dispose();
        IQueryable<T> Get();
        IQueryable<T> Get(Expression<Func<T, bool>> filter = null);
        IQueryable<T> Get(Expression<Func<T, bool>> filter = null, Expression<Func<T, object>> order = null, int? count = 0, int? skip = 0, bool reverse = false);
        T Get(int id);
        DbContext GetDbContext();
        void SaveChanges();
        T Update(T obj);
    }

    public abstract class BaseRepository<T> : IBaseRepository<T>
    {
        public NetDbContext _dbContext;
        public BaseRepository(NetDbContext context)
        {
            _dbContext = context;
        }
        public T Add(T obj) => _dbContext.Add(obj).Entity;

        public void Delete(T obj)
        {
            _dbContext.Entry(obj).State = EntityState.Deleted;
            _dbContext.Remove(obj);
        }
        public void Dispose() => _dbContext.Dispose();

        public IQueryable<T> Get() => _dbContext.Set<T>().AsQueryable();

        public IQueryable<T> Get(Expression<Func<T, bool>> filter = null)
        {
            var dados = _dbContext.Set<T>().AsQueryable();
            return filter != null ? dados.Where(filter) : dados;
        }

        public IQueryable<T> Get(Expression<Func<T, bool>> filter = null, Expression<Func<T, object>> order = null, int? count = 0, int? skip = 0, bool reverse = false)
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

        public T Get(int id) => id <= 0 ? throw new ArgumentException("ID must be greater than 0.") : _dbContext.Set<T>().Find(id);

        public DbContext GetDbContext() => _dbContext;

        public void SaveChanges() => _dbContext.SaveChanges();

        public T Update(T obj)
        {
            _dbContext.Entry(obj).State = EntityState.Modified;
            return _dbContext.Update(obj).Entity;
        }
    }
}
