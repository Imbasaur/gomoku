using Gomoku.Core.Dtos.Games;

namespace Gomoku.Core.Services.Abstract;

public interface IGameService
{
    Task<Guid> CreateGame();
    Task<IEnumerable<GameDto>> GetGames();
    Task<GameDto> GetGame(Guid gameCode);
}