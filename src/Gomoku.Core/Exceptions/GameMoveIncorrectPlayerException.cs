namespace Gomoku.Core.Exceptions;
public class GameMoveIncorrectPlayerException : AppException
{
    public override string Code => "game_move_incorrect_player";

    public GameMoveIncorrectPlayerException() : base("Incorrect player.") { }
}
