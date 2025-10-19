using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mandatory2DGameFramework.Patterns
{
    public class GameLogger
    {
        private static GameLogger instance = new GameLogger();
        public static GameLogger Instance
        {
            get { return instance; }
        }
        private TraceSource traceSource;
        private TraceListener? debugListener = null;
        private GameLogger()
        {
            traceSource = new TraceSource("GameLogger", SourceLevels.All);
            traceSource.Switch = new SourceSwitch("DebugListener", SourceLevels.All.ToString());
        }

        public void AddListener(TraceListener listener)
        {
            traceSource.Listeners.Add(listener);
        }
        public void RemoveListener(TraceListener listener)
        {
            traceSource.Listeners.Remove(listener);
        }
        public void SetDefaultLogLevel(SourceLevels level)
        {
            traceSource.Switch.Level = level;
        }
        public void AddDebugLogging()
        {
            if (debugListener == null)
            {
                debugListener = new ConsoleTraceListener();
                traceSource.Listeners.Add(debugListener);
            }
        }
        public void RemoveDebugLogging()
        {
            if (debugListener != null)
            {
                traceSource.Listeners.Remove(debugListener);
                debugListener = null;
            }
        }
        public void Stop()
        {
            traceSource.Close();
        }
        public void LogInfo(string message)
        {
            traceSource.TraceEvent(TraceEventType.Information, 2, message);
        }
        public void LogWarning(string message)
        {
            traceSource.TraceEvent(TraceEventType.Warning, 2, message);
        }
        public void LogError(string message)
        {
            traceSource.TraceEvent(TraceEventType.Error, 2, message);
        }

        public void LogCritical(string message)
        {
            traceSource.TraceEvent(TraceEventType.Critical, 2, message);
        }
    }
}