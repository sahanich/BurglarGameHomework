using System;

namespace Assets._BurglarGameProject.Scripts.Tools
{
    public interface IToolsView
    {
        public event Action<ToolInfo> ToolUseRequested;
        public void SetTools(ToolInfo[] toolInfos);
        public void SetToolInteractable(ToolType toolType, bool interactable);
    }
}