using System.Collections.Generic;
using System.Threading.Tasks;

namespace TodoApiDTO.Repository
{
    public interface IRepository<TEntity> where TEntity : class
    {
        Task<IEnumerable<TEntity>> GetAllAsync();
        ValueTask<TEntity> GetByIdAsync(long id);
        Task AddAsync(TEntity entity);
        Task Remove(TEntity entity);
        Task<int> CommitAsync();
    }
}
