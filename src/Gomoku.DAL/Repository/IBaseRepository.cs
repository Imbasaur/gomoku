using Gomoku.DAL.Abstract;
using System.Linq.Expressions;

namespace Gomoku.DAL.Repository.Abstract;
public interface IBaseRepository<TEntity, T>
    where TEntity : class, IEntity<T>, new()
    where T : IComparable, IEquatable<T>
{
    TEntity? Get(T id);
    TEntity? Get(Expression<Func<TEntity, bool>> predicate);
    IEnumerable<TEntity> GetMany(Expression<Func<TEntity, bool>>? predicate = null);
    void Add(TEntity entity);
    void Update(TEntity entity);
    void Delete(TEntity entity);
}
