using System.Collections.Concurrent;

namespace Gomoku.Core.Services;
public class GameTimeoutManager
{
    private readonly ConcurrentDictionary<Guid, CancellationTokenSource> _games = new();

    public void TrackGameTimeout(Guid gameId, decimal timeLeft, Func<Guid, Task> onTimeoutAction)
    {
        if (_games.TryGetValue(gameId, out var existingCts))
        {
            existingCts.Cancel();
            existingCts.Dispose();
        }

        var cts = new CancellationTokenSource();
        _games[gameId] = cts;

        _ = GameCheckTask(gameId, timeLeft, onTimeoutAction, cts.Token);
    }

    public void StopTracking(Guid gameId)
    {
        if (_games.TryGetValue(gameId, out var cts))
        {
            cts.Cancel();
            cts.Dispose();
        }
    }

    private async Task GameCheckTask(Guid gameId, decimal timeLeft, Func<Guid, Task> onTimeoutAction, CancellationToken cancellationToken)
    {
        try
        {
            await Task.Delay(TimeSpan.FromSeconds((double)timeLeft), cancellationToken);

            if (!cancellationToken.IsCancellationRequested)
                await onTimeoutAction(gameId);
        }
        catch (TaskCanceledException) { }
        catch (ArgumentNullException ex)
        {
            throw new ArgumentException("Invalid game code", gameId.ToString(), ex);
        }
        finally
        {
            _games.TryRemove(gameId, out _);
        }
    }
}
