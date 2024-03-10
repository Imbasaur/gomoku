using Gomoku.Core.Services.Abstract;
using Gomoku.DAL.Entities;
using Gomoku.DAL.Enums;
using Gomoku.DAL.Repository;

namespace Gomoku.Core.Services;
public class GameService(IGameRepository repository) : IGameService
{
    public Task<Guid> CreateGame()
    {
        var gameCode = Guid.NewGuid();

        repository.Add(new Game
        {
            BlackName = "black",
            WhiteName = "white",
            Code = gameCode,
            State = GameState.Created
        });

        return Task.FromResult(gameCode);
    }
}
