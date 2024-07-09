using System;
using System.Linq;
using Plugins.CommonFunc.Events;

namespace _BurglarGameProject.Mechanics.Tools.Scripts
{
    public class ToolsModel : IToolsModel
    {
        private FastSmartWeakEvent<Action> _stateChangedEvent = new();
        
        public event Action StateChanged
        {
            add
            {
                _stateChangedEvent ??= new FastSmartWeakEvent<Action>();
                _stateChangedEvent.Add(value);
            }
            remove => _stateChangedEvent?.Remove(value);
        }

        public ToolInfo[] ToolInfos { get; private set; }

        public ToolsModel(ToolInfo[] toolInfos)
        {
            SetToolInfos(toolInfos);
        }

        public void SetToolInfos(ToolInfo[] toolInfos)
        {
            ToolInfos = toolInfos.ToArray();
            _stateChangedEvent?.GetRaiseDelegate()?.Invoke();
        }
    }
}