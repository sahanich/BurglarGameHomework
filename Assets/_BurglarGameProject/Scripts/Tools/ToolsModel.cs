using System;
using System.Collections.Generic;
using System.Linq;

namespace Assets._BurglarGameProject.Scripts.Tools
{
    public class ToolsModel : IToolsModel
    {
        public event Action StateChanged;

        public ToolInfo[] ToolInfos { get; private set; }

        private Dictionary<ToolType, bool> _toolsInteractableStateMap;

        public ToolsModel(ToolInfo[] toolInfos)
        {
            _toolsInteractableStateMap = new Dictionary<ToolType, bool>();
            SetToolInfos(toolInfos);
        }

        public void SetToolInfos(ToolInfo[] toolInfos)
        {
            ToolInfos = toolInfos.ToArray();
            foreach (ToolInfo toolInfo in ToolInfos)
            {
                SetToolInteractable(toolInfo.ToolType, true);
            }
            StateChanged?.Invoke();
        }

        public void SetToolInteractable(ToolType toolType, bool interactable)
        {
            _toolsInteractableStateMap[toolType] = interactable;
            StateChanged?.Invoke();
        }

        public bool IsToolInteractable(ToolType toolType)
        {
            if (_toolsInteractableStateMap.TryGetValue(toolType, out bool interactable))
            {
                return interactable;
            }
            return false;
        }
    }
}