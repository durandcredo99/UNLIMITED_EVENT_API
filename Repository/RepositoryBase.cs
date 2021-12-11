using Contracts;
using Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Repository
{
    public abstract class RepositoryBase<T> : IRepositoryBase<T> where T : class
    {
        protected RepositoryContext RepositoryContext { get; set; }

        public RepositoryBase(RepositoryContext repositoryContext)
        {
            this.RepositoryContext = repositoryContext;
        }

        public IQueryable<T> FindAll()
        {
            return this.RepositoryContext.Set<T>().AsNoTracking();
        }

        public IQueryable<T> FindByCondition(Expression<Func<T, bool>> expression)
        {
            return this.RepositoryContext.Set<T>().Where(expression).AsNoTracking();
            //return this.RepositoryContext.Set<T>().Where(expression).AsTracking();
        }

        public async Task CreateAsync(T entity)
        {
            await this.RepositoryContext.Set<T>().AddAsync(entity);
        }

        public async Task CreateAsync(IEnumerable<T> entities)
        {
            await this.RepositoryContext.Set<T>().AddRangeAsync(entities);
        }

        public async Task UpdateAsync(T entity)
        {
            await Task.Run(() => this.RepositoryContext.Set<T>().Update(entity));
        }

        public async Task UpdateAsync(IEnumerable<T> entities)
        {
            await Task.Run(() => this.RepositoryContext.Set<T>().UpdateRange(entities));
        }

        public async Task DeleteAsync(T entity)
        {
            await Task.Run(() => this.RepositoryContext.Set<T>().Remove(entity));
        }
    }
}
