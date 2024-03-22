using AutoMapper;
using Gomoku.Core.Dtos.Games;
using Gomoku.Core.Dtos.WaitingList;
using Gomoku.DAL.Entities;

namespace Gomoku.Core.Profiles;
public class GameProfiles : Profile
{
    public GameProfiles()
    {
        CreateMap<Game, GameDto>().ReverseMap();
        CreateMap<Game, GameCreatedDto>();
        CreateMap<PlayerWaiting, PlayerWaitingDto>().ReverseMap();
    }
}
