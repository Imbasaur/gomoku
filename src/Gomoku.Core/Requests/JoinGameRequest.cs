namespace Gomoku.Core.Requests;
public class JoinGameRequest
{
    public Guid Code { get; set; }
    public string PlayerName { get; set; }
    public bool AsObserver { get; set; }
}
