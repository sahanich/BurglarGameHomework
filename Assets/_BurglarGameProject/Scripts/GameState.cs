using UnityEngine;

namespace BurglarGame
{
    public class GameState
    {
        private int[] _pinValues;
        private int _secondsToLoseLeft;
        private ToolInfo[] _toolInfos;

        public int[] PinValues => _pinValues;
        public int SecondsToLoseLeft => _secondsToLoseLeft;
        public ToolInfo[] ToolInfos => _toolInfos;

        public void SetPinValues(int[] pinValues)
        {
            _pinValues = (int[])pinValues.Clone();
        }

        public void SetSecondsToLoseLeft(int secondsToLoseLeft)
        {
            _secondsToLoseLeft = secondsToLoseLeft;
        }

        public void SetToolInfos(ToolInfo[] toolInfos)
        {
            _toolInfos = toolInfos;
        }
    }
}