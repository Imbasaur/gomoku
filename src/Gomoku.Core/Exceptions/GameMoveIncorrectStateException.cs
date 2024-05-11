namespace Gomoku.Core.Exceptions;
public class GameMoveIncorrectStateException : AppException
{
    public override string Code => "game_move_incorrect_state";

    public GameMoveIncorrectStateException() : base("Player can only move in GameStarted state.") { }
}
