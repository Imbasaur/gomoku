using Gomoku.DAL.Entities;

namespace Gomoku.DAL.Repository;
public class WaitingListRepository(GomokuDbContext dbContext) : BaseRepository<PlayerWaiting, int, GomokuDbContext>(dbContext), IWaitingListRepository
{
    public int Count()
    {
        return DbSet.Count();
    }
    public List<string> GetTop2()
    {
        return DbSet.OrderBy(x => x.Id)
            .Take(2)
            .Select(x => x.PlayerName)
            .ToList();
    }
}
