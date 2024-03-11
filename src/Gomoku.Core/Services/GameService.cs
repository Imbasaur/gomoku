using AutoMapper;
using Gomoku.Core.Dtos.Games;
using Gomoku.Core.Services.Abstract;
using Gomoku.DAL.Entities;
using Gomoku.DAL.Enums;
using Gomoku.DAL.Repository;

namespace Gomoku.Core.Services;
public class GameService(IGameRepository repository, IMapper mapper) : IGameService
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

    public Task<GameDto> GetGame(Guid gameCode)
    {
        var game = repository.Get(x => x.Code == gameCode);

        return Task.FromResult(mapper.Map<GameDto>(game));
    }

    public Task<IEnumerable<GameDto>> GetGames()
    {
        var games = repository.GetMany();

        return Task.FromResult(mapper.Map<IEnumerable<GameDto>>(games));
    }
}
