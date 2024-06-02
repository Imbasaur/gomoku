using Gomoku.DAL.Enums;

namespace Gomoku.Core.Dtos.Games;
public class GameCreatedDto
{
    public Guid Code { get; set; }
    public string BlackName { get; set; }
    public string WhiteName { get; set; }
    public decimal Time { get; set; }
    public GameVariant Variant { get; set; }
}
