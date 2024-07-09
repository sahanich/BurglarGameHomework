using System.Linq;
using Plugins.CommonFunc.Events;
using UnityEngine;

namespace _BurglarGameProject.Mechanics.Tools.Scripts
{
    public class CanvasToolsView : MonoBehaviour, IToolsView
    {
        [SerializeField]
        private ToolView[] ToolViews;
        
        private readonly EventHandlingProcessor<ToolUseEventSignal> _eventHandlingProcessor = new();

        private void OnEnable()
        {
            RegisterEventListeners();
        }

        private void OnDisable()
        {
            UnregisterEventListeners();
        }

        private void Update()
        {
            foreach (var reference in _eventHandlingProcessor._handlers)
            {
                if (!reference.TryGetTarget(out var handler))
                {
                    Debug.Log("reference.TryGetTarget fail");
                }
                // else if (handler == null)
                // {
                //     Debug.Log("IToolUseRequestHandler null ref");
                // }
            }
        }

        public void SetTools(ToolInfo[] toolInfos)
        {
            foreach (var toolInfo in toolInfos)
            {
                ToolView toolView = ToolViews.FirstOrDefault(tool => tool.ToolType == toolInfo.ToolType);
                if (toolView == null)
                {
                    Debug.LogWarning($"ToolView for {toolInfo.ToolType} doesn't exist");
                    continue;
                }
                toolView.Init(toolInfo);
            }

            UnregisterEventListeners();

            if (gameObject.activeInHierarchy)
            {
                RegisterEventListeners();
            }
        }

        public void SetToolInteractable(ToolType toolType, bool interactable)
        {
            ToolView toolView = ToolViews.FirstOrDefault(t => t.ToolInfo.ToolType == toolType);
            if (toolView != null)
            {
                toolView.SetApplyButtonInteractable(interactable);
            }
        }

        public void RegisterEventHandler(IEventHandler<ToolUseEventSignal> handler)
        {
            _eventHandlingProcessor.RegisterHandler(handler);
        }

        public void UnregisterEventHandler(IEventHandler<ToolUseEventSignal> handler)
        {
            _eventHandlingProcessor.UnregisterHandler(handler);
        }

        private void RegisterEventListeners()
        {
            UnregisterEventListeners();
        
            foreach (var tool in ToolViews)
            {
                tool.ApplyToolButtonClicked += OnToolButtonClicked;
            }
        }
        
        private void UnregisterEventListeners()
        {
            foreach (var tool in ToolViews)
            {
                tool.ApplyToolButtonClicked -= OnToolButtonClicked;
            }
        }

        private void OnToolButtonClicked(ToolInfo toolInfo)
        {
            _eventHandlingProcessor.FireEvent(new ToolUseEventSignal(){ToolInfo = toolInfo});
        }        
    }
}