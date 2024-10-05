namespace Gomoku.Core.Dtos.Games;
public class GameListDto
{
    public GameListDto()
    {
    }

    public GameListDto(Guid code, string blackName, string whiteName)
    {
        Code = code;
        BlackName = blackName;
        WhiteName = whiteName;
    }

    public Guid Code { get; set; }
    public string BlackName { get; set; }
    public string WhiteName { get; set; }
}