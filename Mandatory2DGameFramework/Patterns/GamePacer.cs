using System.Threading;
using System.Threading.Tasks;
using Mandatory2DGameFramework.Interfaces;

namespace Mandatory2DGameFramework.Patterns
{
    /// <summary>
    /// Default pacer. Assign a custom implementation to Current to override globally.
    /// </summary>
    public sealed class GamePacer : IPacer
    {
        private static IPacer _current = new GamePacer();
        public static IPacer Current
        {
            get => _current;
            set => _current = value ?? _current;
        }

        public int DelayMilliseconds { get; set; } = 0;

        public void Pause()
        {
            if (DelayMilliseconds > 0)
                System.Threading.Thread.Sleep(DelayMilliseconds);
        }

        public Task PauseAsync(CancellationToken ct = default)
            => DelayMilliseconds > 0 ? Task.Delay(DelayMilliseconds, ct) : Task.CompletedTask;
    }
}