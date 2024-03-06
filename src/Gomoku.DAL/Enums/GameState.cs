namespace Gomoku.DAL.Enums;
public enum GameState
{
    Unknown,
    Created,
    PlayersConnected,
    Started,
    Paused,
    Resumed,
    Finished,
    FinishedByPlayerTimeout,
}
