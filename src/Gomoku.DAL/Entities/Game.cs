using Gomoku.DAL.Abstract;
using Gomoku.DAL.Enums;

namespace Gomoku.DAL.Entities;
public sealed class Game : IEntity<int>
{
    public int Id { get; set; }
    public Guid Code { get; set; }
    public string? Moves { get; set; }
    public GameState State { get; set; }
    public string BlackName { get; set; }
    public string WhiteName { get; set; }
    public string? Winner { get; set; }
    public bool IsWhiteConnected { get; set; }
    public bool IsBlackConnected { get; set; }
    public GameVariant Variant { get; set; }
    public DateTime? StartTime { get; set; }
    public decimal Time { get; set; }
    public decimal BlackTime { get; set; }
    public DateTime? BlackLastMoveTime { get; set; }
    public decimal WhiteTime { get; set; }
    public DateTime? WhiteLastMoveTime { get; set; }

    // Concurrency token
    public uint Version { get; set; }
}
