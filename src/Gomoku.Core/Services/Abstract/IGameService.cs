using Gomoku.Core.Dtos.Games;

namespace Gomoku.Core.Services.Abstract;

public interface IGameService
{
    Task<GameCreatedDto> Create();
    Task<IEnumerable<GameDto>> GetAll();
    Task<GameDto> Get(Guid gameCode);
    Task Join(Guid code, string playerName);
    Task Move(Guid code, int x, int y);
}