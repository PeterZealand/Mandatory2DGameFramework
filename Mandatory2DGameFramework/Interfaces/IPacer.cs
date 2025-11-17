using System.Threading;
using System.Threading.Tasks;

namespace Mandatory2DGameFramework.Interfaces
{
    /// <summary>Abstraction for pacing game actions (e.g., delays between logs/steps).</summary>
    public interface IPacer
    {
        /// <summary>Milliseconds to pause after an action. 0 = no delay.</summary>
        int DelayMilliseconds { get; set; }

        /// <summary>Blocking pause suitable for console demos.</summary>
        void Pause();

        /// <summary>Async-friendly pause.</summary>
        Task PauseAsync(CancellationToken ct = default);
    }
}