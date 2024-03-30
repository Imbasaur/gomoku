using Gomoku.DAL.Entities;
using Microsoft.EntityFrameworkCore;

namespace Gomoku.DAL.Repository;
public class WaitingListRepository(GomokuDbContext dbContext) : BaseRepository<PlayerWaiting, int, GomokuDbContext>(dbContext), IWaitingListRepository
{
    public Task<int> CountAsync()
    {
        return DbSet.CountAsync();
    }
    public Task<List<string>> GetTop2Async()
    {
        return DbSet.OrderBy(x => x.Id)
            .Take(2)
            .Select(x => x.PlayerName)
            .ToListAsync();
    }
}
