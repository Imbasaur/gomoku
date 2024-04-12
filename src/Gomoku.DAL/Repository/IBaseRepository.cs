using Gomoku.DAL.Abstract;
using System.Linq.Expressions;

namespace Gomoku.DAL.Repository;
public interface IBaseRepository<TEntity, T>
    where TEntity : class, IEntity<T>, new()
    where T : IComparable, IEquatable<T>
{
    Task<TEntity?> GetAsync(T id);
    Task<TEntity?> GetAsync(Expression<Func<TEntity, bool>> predicate);
    Task<List<TEntity>> GetManyAsync(Expression<Func<TEntity, bool>>? predicate = null);
    Task AddAsync(TEntity entity);
    Task UpdateAsync(TEntity entity);
    Task DeleteAsync(TEntity entity);
    Task DeleteAsync(Expression<Func<TEntity, bool>> predicate);
    Task DeleteManyAsync(Expression<Func<TEntity, bool>> predicate);
}
