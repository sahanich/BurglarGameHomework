using System;

namespace BurglarGame
{
    public class BurglarGameEventsHandler
    {
        public event Action<ToolInfo> ToolUseRequested;
        public event Action<ToolInfo> ToolUseConfirmed;
        public event Action RestartGameRequested;

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
    }
}