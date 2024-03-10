using Gomoku.DAL.Entities;

namespace Gomoku.DAL.Repository;
public class GameRepository(GomokuDbContext dbContext) : BaseRepository<Game, int, GomokuDbContext>(dbContext), IGameRepository
{
}
