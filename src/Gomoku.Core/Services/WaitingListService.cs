using AutoMapper;
using Gomoku.Core.Dtos.WaitingList;
using Gomoku.Core.Services.Abstract;
using Gomoku.DAL.Entities;
using Gomoku.DAL.Repository;

namespace Gomoku.Core.Services;
public class WaitingListService(IWaitingListRepository repository, IMapper mapper) : IWaitingListService
{
    public Task Add(string playerName)
    {
        repository.Add(new PlayerWaiting(playerName));

        return Task.CompletedTask;
    }

    public Task<int> Count()
    {
        return Task.FromResult(repository.Count());
    }

    public Task<IEnumerable<PlayerWaitingDto>> GetAll()
    {
        var players = repository.GetMany();

        return Task.FromResult(mapper.Map<IEnumerable<PlayerWaitingDto>>(players));
    }

    public Task Remove(string playerName)
    {
        repository.Delete(x => x.PlayerName.ToLower().Equals(playerName.ToLower())); // this will be id in future

        return Task.CompletedTask;
    }
}
