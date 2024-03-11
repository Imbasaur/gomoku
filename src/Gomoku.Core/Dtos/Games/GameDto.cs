using Gomoku.DAL.Enums;

namespace Gomoku.Core.Dtos.Games;
public class GameDto
{
    public Guid Code { get; set; }
    public string? Moves { get; set; }
    public GameState State { get; set; }
    public string BlackName { get; set; }
    public string WhiteName { get; set; }
    public string? Winner { get; set; }
}
