using Gomoku.DAL.Abstract;
using Gomoku.DAL.Repository.Abstract;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace Gomoku.DAL.Repository;
public abstract class BaseRepository<TEntity, T, TContext> : IBaseRepository<TEntity, T>
    where TEntity : class, IEntity<T>, new()
    where T : IComparable, IEquatable<T>
    where TContext : DbContext
{
    private TContext _context;
    protected readonly DbSet<TEntity> DbSet;
    protected TContext Context { get { return _context; } }

    protected BaseRepository(TContext context)
    {
        ArgumentNullException.ThrowIfNull(context);

        DbSet = context.Set<TEntity>();
        _context = context;
    }

    public TEntity? Get(T id)
    {
        if (id.Equals(default))
            throw new ArgumentNullException(nameof(id));

        return DbSet.SingleOrDefault(x => x.Id.Equals(id));
    }

    public TEntity? Get(Expression<Func<TEntity, bool>> predicate)
    {
        ArgumentNullException.ThrowIfNull(predicate);

        return DbSet.SingleOrDefault(predicate);
    }

    public IEnumerable<TEntity> GetMany(Expression<Func<TEntity, bool>>? predicate = null)
    {
        return predicate != null
            ? DbSet.Where(predicate).AsEnumerable()
            : DbSet.AsEnumerable();
    }

    public void Add(TEntity entity)
    {
        ArgumentNullException.ThrowIfNull(entity);
        DbSet.Add(entity);
        Context.SaveChanges();
    }

    public void Update(TEntity entity)
    {
        ArgumentNullException.ThrowIfNull(entity);
        DbSet.Update(entity);
        Context.SaveChanges();
    }

    public void Delete(TEntity entity)
    {
        ArgumentNullException.ThrowIfNull(entity);
        DbSet.Remove(entity);
        Context.SaveChanges();
    }

    public void Delete(Expression<Func<TEntity, bool>> predicate)
    {
        ArgumentNullException.ThrowIfNull(predicate);
        var entity = DbSet.SingleOrDefault(predicate);
        ArgumentNullException.ThrowIfNull(entity);
        DbSet.Remove(entity);
        Context.SaveChanges();
    }

    public void DeleteMany(Expression<Func<TEntity, bool>> predicate)
    {
        ArgumentNullException.ThrowIfNull(predicate);
        var entity = DbSet.Where(predicate);
        ArgumentNullException.ThrowIfNull(entity);
        DbSet.RemoveRange(entity);
        Context.SaveChanges();
    }
}
