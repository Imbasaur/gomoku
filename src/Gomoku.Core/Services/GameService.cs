using Gomoku.Core.Services.Abstract;
using Gomoku.DAL;
using Gomoku.DAL.Entities;
using Gomoku.DAL.Enums;

namespace Gomoku.Core.Services;
public class GameService(GomokuDbContext dbContext) : IGameService
{
    public async Task<Guid> CreateGame()
    {
        var gameCode = Guid.NewGuid();
        await dbContext.AddAsync(new Game
        {
            BlackName = "black",
            WhiteName = "white",
            Code = gameCode,
            State = GameState.Created
        });

        dbContext.SaveChanges();

        return gameCode;
    }
}
