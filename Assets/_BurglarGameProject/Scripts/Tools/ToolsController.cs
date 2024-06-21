using System;

namespace Assets._BurglarGameProject.Scripts.Tools
{
    public abstract class ToolsController
    {
        public event Action<ToolInfo> ToolUseRequested;

        private IToolsModel _toolsModel;
        private IToolsView _toolsView;

        public ToolsController(IToolsModel toolsModel, IToolsView toolsView)
        {
            _toolsModel = toolsModel;
            _toolsView = toolsView;
            _toolsView.SetTools(_toolsModel.ToolInfos);
        }

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

        protected virtual bool IsToolCanBeUsed(ToolInfo toolInfo, int[] pinValues, int minPinValue, int maxPinValue)
        {
            return true;
        }
    }
}