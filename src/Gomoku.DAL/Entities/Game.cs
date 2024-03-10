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
}
