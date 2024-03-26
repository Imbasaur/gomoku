namespace Gomoku.Core.Hubs;
public interface IGameHub
{
    Task SendMessageAsync(string name, string message);
}
