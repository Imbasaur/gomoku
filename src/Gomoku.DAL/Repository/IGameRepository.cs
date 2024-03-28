using Gomoku.DAL.Entities;
using Gomoku.DAL.Enums;
using Gomoku.DAL.Repository.Abstract;

namespace Gomoku.DAL.Repository;
public interface IGameRepository : IBaseRepository<Game, int>
{
    Task SetState(Guid code, GameState state);
    Task ConnectPlayer(Guid code, string playerName);
    Task<bool> AreBothPlayersConnected(Guid code);
}
