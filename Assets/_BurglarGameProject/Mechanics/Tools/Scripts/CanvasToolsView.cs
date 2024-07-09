using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace _BurglarGameProject.Mechanics.Tools.Scripts
{
    public class CanvasToolsView : MonoBehaviour, IToolsView
    {
        // public event Action<ToolInfo> ToolUseRequested;

        [SerializeField]
        private ToolView[] ToolViews;

        private readonly List<WeakReference<IToolUseRequestHandler>> _toolUseRequestHandlers = new();
        
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
            foreach (var reference in _toolUseRequestHandlers)
            {
                if (!reference.TryGetTarget(out IToolUseRequestHandler handler))
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

        public void RegisterToolUseHandler(IToolUseRequestHandler handler)
        {
            if (_toolUseRequestHandlers.Any(handlerReference => 
                    handlerReference.TryGetTarget(out IToolUseRequestHandler target) && target == handler))
            {
                return;
            }

            _toolUseRequestHandlers.Add(new WeakReference<IToolUseRequestHandler>(handler));
        }

        public void UnregisterToolUseHandler(IToolUseRequestHandler handler)
        {
            _toolUseRequestHandlers.RemoveAll(handlerReference =>
                handlerReference.TryGetTarget(out IToolUseRequestHandler target) && target  == handler);
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
            // ToolUseRequested?.Invoke(toolInfo);
            foreach (var handlerReference in _toolUseRequestHandlers)
            {
                if (handlerReference.TryGetTarget(out IToolUseRequestHandler handler))
                {
                    handler.OnToolUseRequested(toolInfo);
                }
            }
        }

    }
}