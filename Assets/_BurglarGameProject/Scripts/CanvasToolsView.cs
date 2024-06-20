using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace BurglarGame
{
    public interface IToolsModel
    {
        public event Action StateChanged;
        public ToolInfo[] ToolInfos { get; }
        public void SetToolInfos(ToolInfo[] toolInfos);
        public void SetToolInteractable(ToolType toolType, bool interactable);
        public bool IsToolInteractable(ToolType toolType);
    }

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

    public abstract class ToolsController
    {
        public event Action<ToolInfo> ToolUseRequested;

        private IToolsModel _toolsModel;
        private IToolsView _toolsView;

        public ToolsController(IToolsModel toolsModel, IToolsView toolsView)
        {
            _toolsModel = toolsModel;
            _toolsView = toolsView;
            _toolsView.Init(_toolsModel.ToolInfos);
        }
        
        //public void Init()
        //{
        //    _toolsView.Init(_toolsModel.ToolInfos);
        //    SetToolsInteractable(false);
        //}

        public void RegisterListeners()
        {
            if (_toolsModel != null)
            {
                _toolsModel.StateChanged += OnModelStateChanged;
            }
            if (_toolsView != null)
            {
                _toolsView.ToolUseRequested += OnViewToolUseRequested;
            }
        }

        public void UnregisterListeners()
        {
            if (_toolsModel != null)
            {
                _toolsModel.StateChanged -= OnModelStateChanged;
            }
            if (_toolsView != null)
            {
                _toolsView.ToolUseRequested -= OnViewToolUseRequested;
            }
        }

        public void SetToolsInteractableByPinValues(int[] pinValues, int minPinValue, int maxPinValue)
        {
            foreach (var tool in _toolsModel.ToolInfos)
            {
                _toolsView.SetToolInteractable(tool.ToolType,
                    IsToolCanBeUsed(tool, pinValues, minPinValue, maxPinValue));
            }
        }

        public void SetToolsInteractable(bool interactable)
        {
            foreach (var tool in _toolsModel.ToolInfos)
            {
                _toolsView.SetToolInteractable(tool.ToolType, interactable);
            }
        }

        private void OnModelStateChanged()
        {
            _toolsView.Init(_toolsModel.ToolInfos);
            foreach (var tool in _toolsModel.ToolInfos)
            {
                _toolsView.SetToolInteractable(tool.ToolType,
                    _toolsModel.IsToolInteractable(tool.ToolType));
            }
        }

        private void OnViewToolUseRequested(ToolInfo toolInfo)
        {
            ToolUseRequested?.Invoke(toolInfo);
        }

        //public void Init()
        //{

        //}

        protected virtual bool IsToolCanBeUsed(ToolInfo toolInfo, int[] pinValues, int minPinValue, int maxPinValue)
        {
            return true;
        }
    }

    public sealed class ConstrainedToolsController : ToolsController
    {
        public ConstrainedToolsController(IToolsModel toolsModel, IToolsView toolsView) : base(toolsModel, toolsView)
        {
        }

        protected override bool IsToolCanBeUsed(ToolInfo toolInfo, int[] pinValues, int minPinValue, int maxPinValue)
        {
            bool toolCanChangePins = true;
            for (int i = 0; i < pinValues.Length; i++)
            {
                int pinValue = pinValues[i];
                int toolChangeValue = toolInfo.PinChangeValues[i];
                int resultPinValue = pinValue + toolChangeValue;
                if (resultPinValue < minPinValue || resultPinValue > maxPinValue)
                {
                    toolCanChangePins = false;
                    break;
                }
            }

            return toolCanChangePins;
        }
    }

    public interface IToolsView
    {
        public event Action<ToolInfo> ToolUseRequested;
        public void Init(ToolInfo[] toolInfos);
        public void SetToolInteractable(ToolType toolType, bool interactable);

        //public void SetToolsInteractableByPinValues(int[] pinValues, int minPinValue, int maxPinValue)
        //{
        //    foreach (var tool in ToolViews)
        //    {
        //        tool.SetApplyButtonInteractable(IsToolCanBeUsed(tool.ToolInfo, pinValues,
        //            minPinValue, maxPinValue));
        //    }
        //}
    }

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

        public void Init(ToolInfo[] toolInfos)
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
            RegisterEventListeners();
        }

        public void SetToolInteractable(ToolType toolType, bool interactable)
        {
            ToolView toolView = ToolViews.FirstOrDefault(t => t.ToolInfo.ToolType == toolType);
            if (toolView != null)
            {
                toolView.SetApplyButtonInteractable(interactable);
            }
        }

        public void SetToolsInteractable(bool interactable)
        {
            foreach (var tool in ToolViews)
            {
                tool.SetApplyButtonInteractable(interactable);
            }
        }

        //public void SetToolsInteractableByPinValues(int[] pinValues, int minPinValue, int maxPinValue)
        //{
        //    foreach (var tool in ToolViews)
        //    {
        //        tool.SetApplyButtonInteractable(IsToolCanBeUsed(tool.ToolInfo, pinValues,
        //            minPinValue, maxPinValue));
        //    }
        //}

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

        //private bool IsToolCanBeUsed(ToolInfo toolInfo, int[] pinValues, int minPinValue, int maxPinValue)
        //{
        //    bool toolCanChangePins = true;
        //    for (int i = 0; i < pinValues.Length; i++)
        //    {
        //        int pinValue = pinValues[i];
        //        int toolChangeValue = toolInfo.PinChangeValues[i];
        //        int resultPinValue = pinValue + toolChangeValue;
        //        if (resultPinValue < minPinValue || resultPinValue > maxPinValue)
        //        {
        //            toolCanChangePins = false;
        //            break;
        //        }
        //    }

        //    return toolCanChangePins;
        //}

        private void OnToolButtonClicked(ToolInfo toolInfo)
        {
            ToolUseRequested?.Invoke(toolInfo);
        }
    }
}