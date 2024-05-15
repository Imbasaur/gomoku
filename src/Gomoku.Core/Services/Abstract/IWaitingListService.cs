using Gomoku.Core.Dtos.WaitingList;

namespace Gomoku.Core.Services.Abstract;

public interface IWaitingListService
{
    Task Add(string playerName, string connectionId);
    Task<IEnumerable<PlayerWaitingDto>> GetAll();
    Task<int> Count();
    Task Remove(string connectionId);
}