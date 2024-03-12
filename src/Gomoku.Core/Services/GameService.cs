using AutoMapper;
using Gomoku.Core.Dtos.Games;
using Gomoku.Core.Services.Abstract;
using Gomoku.DAL.Entities;
using Gomoku.DAL.Enums;
using Gomoku.DAL.Repository;

namespace Gomoku.Core.Services;
public class GameService(IGameRepository repository, IMapper mapper) : IGameService
{
    public Task<Guid> Create()
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

    public Task<GameDto> Get(Guid gameCode)
    {
        var game = repository.Get(x => x.Code == gameCode);

        return Task.FromResult(mapper.Map<GameDto>(game));
    }

    public Task<IEnumerable<GameDto>> GetAll()
    {
        var games = repository.GetMany();

        return Task.FromResult(mapper.Map<IEnumerable<GameDto>>(games));
    }

    public Task SetGameState(Guid code, GameState state)
    {
        repository.SetState(code, state);

        return Task.CompletedTask;
    }
}
