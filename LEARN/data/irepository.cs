using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;

namespace LEARN.data
{
    public interface irepository<TEntity> where TEntity : class, IEntity
    {
        Task<TEntity> InsertAsync(TEntity entity, CancellationToken cancellationToken = default);    
        Task InsertRangeAsync(IEnumerable<TEntity> entity, CancellationToken cancellationToken = default);
        Task DeleteAsync(TEntity entity, CancellationToken cancellationToken = default);
        Task<TEntity> GetByIdAsync<TKey>(TKey id, CancellationToken cancellationToken = default) where TKey : notnull;
        Task<TEntity> GetByNameAsync(string name, CancellationToken cancellationToken = default);
        Task UpdateAsync(TEntity entity, CancellationToken cancellationToken = default);
        DbSet<TEntity> Table { get; }
        Task<TEntity> SingleOrDefaultAsync(
                Expression<Func<TEntity, bool>> filter = null,
                CancellationToken cancellationToken = default);



    }
}
