namespace Gomoku.Core.Requests;
public class AddMoveRequest
{
    public Guid Code { get; set; }
    public string Move { get; set; }
}
