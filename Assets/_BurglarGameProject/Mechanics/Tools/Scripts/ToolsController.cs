using System;
using Plugins.CommonFunc.Events;
using UnityEngine;

namespace _BurglarGameProject.Mechanics.Tools.Scripts
{
    
    public abstract class ToolsController : IEventHandler<ToolUseEventSignal>
    {
        public event Action<ToolInfo> ToolUseRequested;

        private IToolsModel _toolsModel;
        private IToolsView _toolsView;
        
        protected ToolsController(IToolsModel toolsModel, IToolsView toolsView)
        {
            _toolsModel = toolsModel;
            _toolsView = toolsView;
            _toolsView.SetTools(_toolsModel.ToolInfos);
        }

        public void HandleEvent(ToolUseEventSignal signal)
        {
            ToolUseRequested?.Invoke(signal.ToolInfo);
        }

        ~ToolsController()
        {
            Debug.Log("destroyed");
        }

        public void RegisterListeners()
        {
            if (_toolsModel != null)
            {
                _toolsModel.StateChanged += OnModelStateChanged;
            }
            if (_toolsView != null)
            {
                _toolsView.RegisterEventHandler(this);
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
                _toolsView.UnregisterEventHandler(this);
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

        public virtual bool IsToolCanBeUsed(ToolInfo toolInfo, int[] pinValues, int minPinValue, int maxPinValue)
        {
            return true;
        }

        private void OnModelStateChanged()
        {
            _toolsView.SetTools(_toolsModel.ToolInfos);
        }
    }
}