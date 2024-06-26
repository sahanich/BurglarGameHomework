using System;

namespace _BurglarGameProject.Mechanics.Tools.Scripts
{
    public interface IToolsView
    {
        public event Action<ToolInfo> ToolUseRequested;
        public void SetTools(ToolInfo[] toolInfos);
        public void SetToolInteractable(ToolType toolType, bool interactable);
    }
}