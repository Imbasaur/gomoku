namespace Gomoku.Core.Exceptions;
public class GameMoveExistsException : AppException
{
    public override string Code => "game_move_exists";

    public GameMoveExistsException() : base("Move already exists.") { }
}
