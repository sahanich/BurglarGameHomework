using System;

namespace _BurglarGameProject.Mechanics.Tools.Scripts
{
    public interface IToolsView
    {
        // public event Action<ToolInfo> ToolUseRequested;
        public void SetTools(ToolInfo[] toolInfos);
        public void SetToolInteractable(ToolType toolType, bool interactable);
        public void RegisterToolUseHandler(IToolUseRequestHandler handler);
        public void UnregisterToolUseHandler(IToolUseRequestHandler handler);
    }
    
    public interface IToolUseRequestHandler
    {
        void OnToolUseRequested(ToolInfo toolInfo);
    }
    
}