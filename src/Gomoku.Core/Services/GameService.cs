using AutoMapper;
using Gomoku.Core.Dtos.Games;
using Gomoku.Core.Services.Abstract;
using Gomoku.DAL.Entities;
using Gomoku.DAL.Enums;
using Gomoku.DAL.Repository;

namespace Gomoku.Core.Services;
public class GameService(IGameRepository repository, IMapper mapper, IWaitingListRepository waitingListRepository) : IGameService
{
    public Task<GameCreatedDto> Create()
    {
        var players = waitingListRepository.GetTop2();

        if (players == null || players.Count() < 2)
            return null;

        var gameCode = Guid.NewGuid();
        var game = new Game
        {
            BlackName = players[0], // todo: have to add some randomisation here
            WhiteName = players[1],
            Code = gameCode,
            State = GameState.Created
        };

        repository.Add(game);
        waitingListRepository.Delete(x => x.PlayerName == players[0] || x.PlayerName == players[1]);

        return Task.FromResult(mapper.Map<GameCreatedDto>(game));
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
