using Gomoku.Core.Dtos.Games;
using Gomoku.DAL.Entities;
using System.Linq.Expressions;

namespace Gomoku.Core.Services.Abstract;

public interface IGameService
{
    Task<GameCreatedDto> Create();
    Task<IEnumerable<GameListDto>> GetMany(Expression<Func<Game, bool>>? predicate = null);
    Task<GameDto> Get(Guid gameCode);
    Task Join(Guid code, string playerName, string connectionId = null, bool asObserver = false);
    Task AddMove(Guid code, string move, string playerName);
    Task CheckTimeoutWin(Guid gameCode);
    Task FinishGamesByDisconnect(string playerName);
}