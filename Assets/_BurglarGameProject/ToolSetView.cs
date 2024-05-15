using System.Linq;
using UnityEngine;

namespace BurglarGame
{

    public class ToolSetView : MonoBehaviour
    {
        [SerializeField]
        private ToolView[] ToolViews;

        private int _minPinValue;
        private int _maxPinValue;

        private GameState _gameState;
        private BurglarGameEventsHandler _eventsHandler;

        private void OnEnable()
        {
            RegisterEventListeners();
        }

        private void OnDisable()
        {
            UnregisterEventListeners();
        }

        public void Init(BurglarGameSettings gameSettings, BurglarGameEventsHandler eventsHandler)
        {
            _minPinValue = gameSettings.MinPinValue;
            _maxPinValue = gameSettings.MaxPinValue;

            foreach (var tool in ToolViews)
            {
                tool.Init(eventsHandler);
            }

            UnregisterEventListeners();
            _eventsHandler = eventsHandler;
            RegisterEventListeners();
        }

        public void Deinit()
        {
            UnregisterEventListeners();
        }

        public void SetState(GameState gameState)
        {
            _gameState = gameState;

            if (_gameState == null || _gameState.ToolInfos == null
                || _gameState.ToolInfos.Length != ToolViews.Length)
            {
                Debug.LogError($"{nameof(ToolSetView)}.{nameof(SetState)}. gameState and pinViews don't match");
                return;
            }

            foreach (var toolInfo in _gameState.ToolInfos)
            {
                ToolView toolView = ToolViews.FirstOrDefault(tool => tool.ToolType == toolInfo.ToolType);
                if (toolView == null)
                {
                    Debug.LogWarning($"{nameof(ToolSetView)}.{nameof(SetState)}. " +
                        $"ToolView for {toolInfo.ToolType} doesn't exist");
                    continue;
                }
                toolView.SetToolInfo(toolInfo);
                toolView.SetApplyButtonInteractable(GameState.IsToolCanBeUsed(toolInfo, _gameState.PinValues, 
                    _minPinValue, _maxPinValue));
            }
        }

        //private bool IsToolUsingResultInPinValuesRange(GameState gameState, ToolInfo toolInfo)
        //{
        //    bool resultPinValueIsInRange = true;

        //    for (int i = 0; i < gameState.PinValues.Length; i++)
        //    {
        //        int pinValue = gameState.PinValues[i];
        //        int toolChangeValue = toolInfo.PinChangeValues[i];
        //        int resultPinValue = pinValue + toolChangeValue;
        //        if (resultPinValue < _minPinValue
        //            || resultPinValue > _maxPinValue)
        //        {
        //            resultPinValueIsInRange = false;
        //            break;
        //        }
        //    }

        //    return resultPinValueIsInRange;
        //}

        //private bool IsToolCanBeUsed(GameState gameState, ToolInfo toolInfo)
        //{
        //    bool toolCanChangePins = false;
        //    for (int i = 0; i < gameState.PinValues.Length; i++)
        //    {
        //        int pinValue = gameState.PinValues[i];
        //        int toolChangeValue = toolInfo.PinChangeValues[i];
        //        int resultPinValue = Mathf.Clamp(pinValue + toolChangeValue, _minPinValue, _maxPinValue);
        //        if (resultPinValue != pinValue)
        //        {
        //            toolCanChangePins = true;
        //            break;
        //        }
        //    }

        //    return toolCanChangePins;

        //    //bool resultPinValueIsInRange = true;

        //    //for (int i = 0; i < gameState.PinValues.Length; i++)
        //    //{
        //    //    int pinValue = gameState.PinValues[i];
        //    //    int toolChangeValue = toolInfo.PinChangeValues[i];
        //    //    int resultPinValue = pinValue + toolChangeValue;
        //    //    if (resultPinValue < _minPinValue
        //    //        || resultPinValue > _maxPinValue)
        //    //    {
        //    //        resultPinValueIsInRange = false;
        //    //        break;
        //    //    }
        //    //}

        //    //return resultPinValueIsInRange;
        //}

        private void RegisterEventListeners()
        {
            UnregisterEventListeners();

            if (_eventsHandler == null)
            {
                return;
            }
            _eventsHandler.ToolUseRequested += OnToolUseRequested;
        }

        private void UnregisterEventListeners()
        {
            if (_eventsHandler == null)
            {
                return;
            }
            _eventsHandler.ToolUseRequested -= OnToolUseRequested;
        }

        private void OnToolUseRequested(ToolInfo toolInfo)
        {
            if (GameState.IsToolCanBeUsed(toolInfo, _gameState.PinValues, _minPinValue, _maxPinValue))
            {
                _eventsHandler.RaiseToolUseConfirmed(toolInfo);
            }
        }
    }
}