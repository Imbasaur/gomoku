using Gomoku.DAL.Entities;
using Gomoku.DAL.Enums;
using Microsoft.EntityFrameworkCore;

namespace Gomoku.DAL.Repository;
public class GameRepository(GomokuDbContext dbContext) : BaseRepository<Game, int, GomokuDbContext>(dbContext), IGameRepository
{
    public async Task SetState(Guid code, GameState state)
    {
        if (code.Equals(default))
            throw new ArgumentNullException(nameof(code));

        var entity = await DbSet.SingleOrDefaultAsync(x => x.Code == code);
        ArgumentNullException.ThrowIfNull(entity);

        entity.State = state;

        await Context.SaveChangesAsync();
    }

    public async Task ConnectPlayer(Guid code, string playerName)
    {
        if (code.Equals(default))
            throw new ArgumentNullException(nameof(code));

        var entity = await DbSet.SingleOrDefaultAsync(x => x.Code == code);
        ArgumentNullException.ThrowIfNull(entity);

        if (entity.BlackName.Equals(playerName))
            entity.IsBlackConnected = true;
        else if (entity.WhiteName.Equals(playerName))
            entity.IsWhiteConnected = true;

        await Context.SaveChangesAsync();
    }

    public async Task<bool> AreBothPlayersConnected(Guid code)
    {
        if (code.Equals(default))
            throw new ArgumentNullException(nameof(code));

        var entity = await DbSet.SingleOrDefaultAsync(x => x.Code == code);
        ArgumentNullException.ThrowIfNull(entity);

        return entity.IsBlackConnected && entity.IsWhiteConnected;
    }
}
