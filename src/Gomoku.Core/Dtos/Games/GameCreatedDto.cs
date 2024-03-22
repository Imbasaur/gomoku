using Gomoku.DAL.Enums;

namespace Gomoku.Core.Dtos.Games;
public class GameCreatedDto
{
    public Guid Code { get; set; }
    public GameState State { get; set; }
    public string BlackName { get; set; }
    public string WhiteName { get; set; }
}
