using Gomoku.DAL.Entities;
using Gomoku.DAL.Repository.Abstract;

namespace Gomoku.DAL.Repository;
public interface IWaitingListRepository : IBaseRepository<PlayerWaiting, int>
{
    Task<int> CountAsync();
    Task<List<string>> GetTop2Async();
}
