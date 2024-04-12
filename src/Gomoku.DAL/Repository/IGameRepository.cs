using Gomoku.DAL.Entities;
using Gomoku.DAL.Enums;

namespace Gomoku.DAL.Repository;
public interface IGameRepository : IBaseRepository<Game, int>
{
    Task SetStateAsync(Guid code, GameState state);
    Task ConnectPlayerAsync(Guid code, string playerName);
    Task<bool> AreBothPlayersConnectedAsync(Guid code);
}
