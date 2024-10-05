using Gomoku.Core.Dtos.Games;
using Gomoku.DAL.Enums;

namespace Gomoku.Core.Dtos.SignalR;

public class InitGame
{
    public string Moves { get; set; }
    public string BlackName { get; set; }
    public string WhiteName { get; set; }
    public decimal Time { get; set; }
    public GameVariant Variant { get; set; }
    public ClockDto Clock { get; set; }
}
