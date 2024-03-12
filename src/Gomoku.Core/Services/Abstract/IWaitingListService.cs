using Gomoku.DAL.Entities;

namespace Gomoku.Core.Services.Abstract;

public interface IWaitingListService
{
    Task Add();
    Task<IEnumerable<PlayerWaiting>> GetAll();
    Task<int> Count();
    Task Remove(string playerName);
}