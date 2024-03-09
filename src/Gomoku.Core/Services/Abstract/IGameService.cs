namespace Gomoku.Core.Services.Abstract;

public interface IGameService
{
    Task<Guid> CreateGame();
}