using UnityEngine;

namespace BurglarGame
{
    //public class PinsController
    //{
    //    public Action StateChanged;

    //    public ToolView[] ToolViews;

    //    public void Init(int pinCount, int minPinValue, int maxPinValue)
    //    {
    //        foreach (var tool in ToolViews)
    //        {
    //            tool.Init(pinCount, minToolPinChangeValue, maxToolPinChangeValue);
    //        }
    //    }

    //    internal void SetState(GameState gameState)
    //    {
    //        throw new NotImplementedException();
    //    }
    //}

    public class GameState
    {
        private int[] _pinValues;
        private int _secondsToGameOver;
        private ToolInfo[] _toolInfos;

        public int[] PinValues => _pinValues;
        public int SecondsToGameOver => _secondsToGameOver;
        public ToolInfo[] ToolInfos => _toolInfos;

        public static bool IsToolCanBeUsed(ToolInfo toolInfo, int[] pinValues, int minPinValue, int maxPinValue)
        {
            bool toolCanChangePins = false;
            for (int i = 0; i < pinValues.Length; i++)
            {
                int pinValue = pinValues[i];
                int toolChangeValue = toolInfo.PinChangeValues[i];
                int resultPinValue = Mathf.Clamp(pinValue + toolChangeValue, minPinValue, maxPinValue);
                if (resultPinValue != pinValue)
                {
                    toolCanChangePins = true;
                    break;
                }
            }

            return toolCanChangePins;
        }

        public void SetPinValues(int[] pinValues)
        {
            _pinValues = (int[])pinValues.Clone();
        }

        //public void ApplyToolValues(ToolInfo toolInfo)
        //{
        //    for (int i = 0; i < toolInfo.PinChangeValues.Length; i++)
        //    {
        //        int newPinValue = UnityEngine.Mathf.Clamp(toolInfo.PinChangeValues[i];
        //        _pinValues[i] = newPinValue;
        //        //_pinValues[i] += UnityEngine.Mathf.Clamp(toolInfo.PinChangeValues[i];
        //    }
        //}

        public void SetSecondsToGameOver(int secondsToGameOver)
        {
            _secondsToGameOver = secondsToGameOver;
        }

        public void SetToolInfos(ToolInfo[] toolInfos)
        {
            _toolInfos = toolInfos;
            //_toolInfos = new ToolInfo[toolInfos.Length];
            //for (int i = 0; i < _toolInfos.Length; i++)
            //{
            //    _toolInfos[i] = toolInfos[i].Clone();
            //}
        }
    }
}