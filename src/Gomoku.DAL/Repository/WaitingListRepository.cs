using Gomoku.DAL.Entities;

namespace Gomoku.DAL.Repository;
public class WaitingListRepository(GomokuDbContext dbContext) : BaseRepository<PlayerWaiting, int, GomokuDbContext>(dbContext), IWaitingListRepository
{
}
