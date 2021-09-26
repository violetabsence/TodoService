using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace TodoApiDTO.Repository
{
    public class Repository<TEntity> : IRepository<TEntity> where TEntity : class
    {
        protected RepositoryContext RepositoryContext { get; set; }

        public Repository(RepositoryContext repositoryContext)
        {
            RepositoryContext = repositoryContext;
        }

        public async Task<IEnumerable<TEntity>> GetAllAsync()
        {
            return await RepositoryContext.Set<TEntity>().ToListAsync();
        }

        public async Task AddAsync(TEntity entity)
        {
            await RepositoryContext.Set<TEntity>().AddAsync(entity);
            await CommitAsync();
        }

        public async Task Remove(TEntity entity)
        {
            RepositoryContext.Set<TEntity>().Remove(entity);
            await CommitAsync();
        }

        public async ValueTask<TEntity> GetByIdAsync(long id)
        {
            return await RepositoryContext.Set<TEntity>().FindAsync(id);
        }

        public async Task<int> CommitAsync()
        {
            return await RepositoryContext.SaveChangesAsync();
        }
    }
}
