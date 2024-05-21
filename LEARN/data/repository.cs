using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using LEARN.db;

namespace LEARN.data
{
    public class repository<TEntity> : irepository<TEntity> where TEntity : class, IEntity
    {
        private readonly appdbcontext _dbContext;

        public repository(appdbcontext dbContext)
        {
            _dbContext = dbContext;
        }
        public DbSet<TEntity> Table => _dbContext.Set<TEntity>();



        public async Task<TEntity> GetByIdAsync<TKey>(TKey id, CancellationToken cancellationToken = default)
    where TKey : notnull
        {
            var entity = await Table.FindAsync(new object[] { id });

            return entity;
        }

        public async Task<TEntity> GetByNameAsync(string name, CancellationToken cancellationToken = default)
        {
            var parameter = Expression.Parameter(typeof(TEntity), "x");
            var property = Expression.Property(parameter, "name");
            var condition = Expression.Equal(property, Expression.Constant(name));
            var lambda = Expression.Lambda<Func<TEntity, bool>>(condition, parameter);
         
            var entity = await Table.FirstOrDefaultAsync(lambda, cancellationToken);
            return entity;
        }

        public Task UpdateAsync(TEntity entity, CancellationToken cancellationToken = default)
        {
            Table.Update(entity);
            return Task.CompletedTask;
        }
        public Task DeleteAsync(TEntity entity, CancellationToken cancellationToken = default)
        {
            Table.Remove(entity);

            return Task.CompletedTask;
        }
        public async Task<TEntity> InsertAsync(TEntity entity, CancellationToken cancellationToken = default)
        {
            var entry = await Table.AddAsync(entity, cancellationToken).ConfigureAwait(false);

            return entry.Entity;
        }


       public async Task InsertRangeAsync(IEnumerable<TEntity> entity, CancellationToken cancellationToken = default)
        {
            await Table.AddRangeAsync(entity, cancellationToken).ConfigureAwait(false);
        }

        public Task<TEntity> SingleOrDefaultAsync(Expression<Func<TEntity, bool>> filter = null,
    CancellationToken cancellationToken = default)
        {
            var query = Table.AsQueryable();

            if (filter != null)
            {
                query = query.Where(filter);
            }

            return query.SingleOrDefaultAsync(cancellationToken);
        }


    }
}
