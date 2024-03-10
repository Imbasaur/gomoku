using Gomoku.DAL.Entities;
using Gomoku.DAL.Repository.Abstract;

namespace Gomoku.DAL.Repository;
public interface IGameRepository : IBaseRepository<Game, int>
{
}
