using Gomoku.Core.Dtos.Games;

namespace Gomoku.Core.Dtos.SignalR;
sealed class MoveAdded
{
    public string Move { get; set; }
    public ClockDto Clock { get; set; }
}
