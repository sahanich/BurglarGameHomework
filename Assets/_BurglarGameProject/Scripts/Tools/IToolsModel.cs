using System;

namespace Assets._BurglarGameProject.Scripts.Tools
{
    public interface IToolsModel
    {
        public event Action StateChanged;
        public ToolInfo[] ToolInfos { get; }
        public void SetToolInfos(ToolInfo[] toolInfos);
        public void SetToolInteractable(ToolType toolType, bool interactable);
        public bool IsToolInteractable(ToolType toolType);
    }
}