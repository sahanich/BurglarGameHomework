using System;

namespace BurglarGame
{
    public class BurglarGameEventsHandler
    {
        public event Action<ToolInfo> ToolUseRequested;
        public event Action<ToolInfo> ToolUseConfirmed;
        public event Action RestartGameRequested;
        public event Action GameTimerUpdated;

        public void RaiseToolUseRequested(ToolInfo toolInfo)
        {
            ToolUseRequested?.Invoke(toolInfo);
        }

        public void RaiseToolUseConfirmed(ToolInfo toolInfo)
        {
            ToolUseConfirmed?.Invoke(toolInfo);
        }

        public void RaiseRestartGameRequested()
        {
            RestartGameRequested?.Invoke();
        }

        public void RaiseTimerUpdated()
        {
            GameTimerUpdated?.Invoke();
        }
    }
}