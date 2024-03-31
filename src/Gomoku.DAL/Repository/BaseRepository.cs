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
    private readonly TContext _context;
    protected readonly DbSet<TEntity> DbSet;
    protected TContext Context { get { return _context; } }

    protected BaseRepository(TContext context)
    {
        ArgumentNullException.ThrowIfNull(context);

        DbSet = context.Set<TEntity>();
        _context = context;
    }

    public Task<TEntity?> GetAsync(T id)
    {
        if (id.Equals(default))
            throw new ArgumentNullException(nameof(id));

        return DbSet.SingleOrDefaultAsync(x => x.Id.Equals(id));
    }

    public Task<TEntity?> GetAsync(Expression<Func<TEntity, bool>> predicate)
    {
        ArgumentNullException.ThrowIfNull(predicate);

        return DbSet.SingleOrDefaultAsync(predicate);
    }

    public Task<List<TEntity>> GetManyAsync(Expression<Func<TEntity, bool>>? predicate = null)
    {
        return predicate != null
            ? DbSet.Where(predicate).ToListAsync()
            : DbSet.ToListAsync();
    }

    public async Task AddAsync(TEntity entity)
    {
        ArgumentNullException.ThrowIfNull(entity);
        await DbSet.AddAsync(entity);
        await Context.SaveChangesAsync();
    }

    public async Task UpdateAsync(TEntity entity)
    {
        ArgumentNullException.ThrowIfNull(entity);
        DbSet.Update(entity);
        await Context.SaveChangesAsync();
    }

    public async Task DeleteAsync(TEntity entity)
    {
        ArgumentNullException.ThrowIfNull(entity);
        DbSet.Remove(entity);
        await Context.SaveChangesAsync();
    }

    public async Task DeleteAsync(Expression<Func<TEntity, bool>> predicate)
    {
        ArgumentNullException.ThrowIfNull(predicate);
        var entity = await DbSet.SingleOrDefaultAsync(predicate);
        ArgumentNullException.ThrowIfNull(entity);
        DbSet.Remove(entity);
        await Context.SaveChangesAsync();
    }

    public async Task DeleteManyAsync(Expression<Func<TEntity, bool>> predicate)
    {
        ArgumentNullException.ThrowIfNull(predicate);
        var entity = DbSet.Where(predicate);
        ArgumentNullException.ThrowIfNull(entity);
        DbSet.RemoveRange(entity);
        await Context.SaveChangesAsync();
    }
}
