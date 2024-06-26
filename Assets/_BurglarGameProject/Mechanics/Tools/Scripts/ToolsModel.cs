using System;
using System.Linq;

namespace _BurglarGameProject.Mechanics.Tools.Scripts
{
    public class ToolsModel : IToolsModel
    {
        public event Action StateChanged;

        public ToolInfo[] ToolInfos { get; private set; }

        public ToolsModel(ToolInfo[] toolInfos)
        {
            SetToolInfos(toolInfos);
        }

        public void SetToolInfos(ToolInfo[] toolInfos)
        {
            ToolInfos = toolInfos.ToArray();
            StateChanged?.Invoke();
        }
    }
}