using System;
using System.Linq;
using UnityEngine;

namespace Assets._BurglarGameProject.Scripts.Tools
{
    public class CanvasToolsView : MonoBehaviour, IToolsView
    {
        public event Action<ToolInfo> ToolUseRequested;

        [SerializeField]
        private ToolView[] ToolViews;

        private void OnEnable()
        {
            RegisterEventListeners();
        }

        private void OnDisable()
        {
            UnregisterEventListeners();
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
            ToolUseRequested?.Invoke(toolInfo);
        }
    }
}