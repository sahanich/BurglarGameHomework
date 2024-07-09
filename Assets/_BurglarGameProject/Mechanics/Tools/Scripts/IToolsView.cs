using Plugins.CommonFunc.Events;

namespace _BurglarGameProject.Mechanics.Tools.Scripts
{
    public interface IToolsView : IEventObservable<ToolUseEventSignal>
    {
        public void SetTools(ToolInfo[] toolInfos);
        public void SetToolInteractable(ToolType toolType, bool interactable);
    }
}