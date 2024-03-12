using AutoMapper;
using Gomoku.Core.Dtos.WaitingList;
using Gomoku.DAL.Entities;

namespace Gomoku.Core.Profiles;
public class WaitingListProfiles : Profile
{
    public WaitingListProfiles()
    {
        CreateMap<PlayerWaiting, PlayerWaitingDto>().ReverseMap();
    }
}
