namespace Gomoku.Core.Dtos.SignalR;
sealed class GameFinished
{
    public string Code { get; set; }
    public string Winner { get; set; }
    public List<string> WinningStones { get; set; }
}
