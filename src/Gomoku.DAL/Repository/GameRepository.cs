using Gomoku.DAL.Entities;
using Gomoku.DAL.Enums;

namespace Gomoku.DAL.Repository;
public class GameRepository(GomokuDbContext dbContext) : BaseRepository<Game, int, GomokuDbContext>(dbContext), IGameRepository
{

    public void SetState(Guid code, GameState state)
    {
        if (code.Equals(default))
            throw new ArgumentNullException(nameof(code));

        var entity = DbSet.SingleOrDefault(x => x.Code == code);

        if (entity == null)
            throw new ArgumentException(nameof(entity));

        entity.State = state;

        Context.SaveChanges();
    }
}
