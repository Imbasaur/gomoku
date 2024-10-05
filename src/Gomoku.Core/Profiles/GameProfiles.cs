using AutoMapper;
using Gomoku.Core.Dtos.Games;
using Gomoku.Core.Dtos.SignalR;
using Gomoku.Core.Dtos.WaitingList;
using Gomoku.DAL.Entities;

namespace Gomoku.Core.Profiles;
public class GameProfiles : Profile
{
    public GameProfiles()
    {
        CreateMap<Game, GameDto>().ReverseMap();
        CreateMap<Game, GameListDto>().ReverseMap();
        CreateMap<Game, GameCreatedDto>();
        CreateMap<Game, InitGame>()
            .ForMember(dest => dest.Clock, opt =>
                opt.MapFrom(x => new ClockDto
                {
                    Black = (!x.BlackLastMoveTime.HasValue && x.WhiteLastMoveTime.HasValue) || x.BlackLastMoveTime < x.WhiteLastMoveTime
                        ? decimal.Round(x.Time - (decimal)(DateTime.UtcNow - x.WhiteLastMoveTime).Value.TotalSeconds, 2)
                        : decimal.Round(x.BlackTime, 2),
                    White = (!x.WhiteLastMoveTime.HasValue && x.BlackLastMoveTime.HasValue) || x.WhiteLastMoveTime < x.BlackLastMoveTime
                        ? decimal.Round(x.Time - (decimal)(DateTime.UtcNow - x.BlackLastMoveTime).Value.TotalSeconds, 2)
                        : decimal.Round(x.WhiteTime, 2),
                }));


        CreateMap<PlayerWaiting, PlayerWaitingDto>().ReverseMap();
    }
}
