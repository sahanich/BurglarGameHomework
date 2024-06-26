using System;

namespace _BurglarGameProject.Mechanics.Tools.Scripts
{
    public interface IToolsModel
    {
        public event Action StateChanged;
        public ToolInfo[] ToolInfos { get; }
        public void SetToolInfos(ToolInfo[] toolInfos);
    }
}