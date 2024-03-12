using Gomoku.DAL.Entities;
using Gomoku.DAL.Enums;
using Gomoku.DAL.Repository.Abstract;

namespace Gomoku.DAL.Repository;
public interface IGameRepository : IBaseRepository<Game, int>
{
    void SetState(Guid code, GameState state);
}
