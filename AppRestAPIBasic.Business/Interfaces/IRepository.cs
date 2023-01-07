using AppRestAPIBasic.Business.Models;
using System.Linq.Expressions;

namespace AppRestAPIBasic.Business.Interfaces
{
    public interface IRepository<TEntity> : IDisposable where TEntity : Entity
    {
        Task AddAsync(TEntity entity);
        Task<TEntity> GetAsync(Guid id);
        Task<List<TEntity>> GetAsync();
        Task<List<TEntity>> GetAsync(Expression<Func<TEntity, bool>> predicate);
        Task UpdateAsync(TEntity entity);
        Task DeleteAsync(Guid id);
        Task<int> SaveAsync();
    }
}
