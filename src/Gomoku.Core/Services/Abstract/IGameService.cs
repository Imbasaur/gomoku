using Gomoku.Core.Dtos.Games;

namespace Gomoku.Core.Services.Abstract;

public interface IGameService
{
    Task<Guid> Create();
    Task<IEnumerable<GameDto>> GetAll();
    Task<GameDto> Get(Guid gameCode);
}