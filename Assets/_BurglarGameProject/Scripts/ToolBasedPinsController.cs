using System;
using System.Linq;

namespace BurglarGame
{
    public sealed class ToolBasedPinsController : PinsController
    {
        private ToolInfo[] _tools;
        private Random _random;
        private int _codingIterationsCount;

        public ToolBasedPinsController(IPinsModel model, IPinsView view,
            ToolInfo[] tools, int codingIterationsCount) : base(model, view)
        {
            _tools = tools.ToArray();
            _codingIterationsCount = codingIterationsCount;
            _random = new Random();
        }

        public override void SetStartState()
        {
            int[] pinStates = CreatePinStates(_tools);
            _model.SetState(pinStates);
        }

        public override void AddPinValues(int[] addingPinValues)
        {
            int[] newPinValues = _model.State.ToArray();

            for (int i = 0; i < addingPinValues.Length; i++)
            {
                newPinValues[i] = newPinValues[i] + addingPinValues[i];
            }

            _model.SetState(newPinValues);
        }

        private void ShuffleArray(object[] array)
        {
            for (int i = 0; i < array.Length - 1; i++)
            {
                int randomIndex = _random.Next(i, array.Length);
                object tempElement = array[i];
                array[i] = array[randomIndex];
                array[randomIndex] = tempElement;
            }
        }

        private bool IsRevertedToolApplyingResultInPinValuesRange(ToolInfo toolInfo, int[] pinValues)
        {
            bool resultPinValueIsInRange = true;

            for (int i = 0; i < pinValues.Length; i++)
            {
                int pinValue = pinValues[i];
                int toolChangeValue = toolInfo.PinChangeValues[i];
                int resultPinValue = pinValue - toolChangeValue;
                if (resultPinValue < _model.MinPossibleValue
                    || resultPinValue > _model.MaxPossibleValue)
                {
                    resultPinValueIsInRange = false;
                    break;
                }
            }

            return resultPinValueIsInRange;
        }

        private bool TryApplyToolReverted(ToolInfo toolInfo, int[] pinValues)
        {
            if (!IsRevertedToolApplyingResultInPinValuesRange(toolInfo, pinValues))
            {
                return false;
            }

            for (int i = 0; i < pinValues.Length; i++)
            {
                int pinValue = pinValues[i];
                int toolChangeValue = toolInfo.PinChangeValues[i];
                pinValues[i] = pinValue - toolChangeValue;
            }
            return true;

        }

        private int[] CreatePinStates(ToolInfo[] toolInfos)
        {
            int[] pinValues = _model.TargetState.ToArray();

            ToolInfo[] randomOrderedToolInfos = (ToolInfo[])toolInfos.Clone();
            ShuffleArray(randomOrderedToolInfos);

            int currentToolIndex = 0;
            for (int i = 0; i < _codingIterationsCount; i++)
            {
                int tryCountToApplyTool = randomOrderedToolInfos.Length;

                while (tryCountToApplyTool > 0)
                {
                    ToolInfo toolInfo = randomOrderedToolInfos[currentToolIndex];

                    bool missTool = _random.Next(0, 2) == 1;

                    if (!missTool && TryApplyToolReverted(toolInfo, pinValues))
                    {
                        tryCountToApplyTool = 0;
                    }
                    else
                    {
                        --tryCountToApplyTool;
                    }

                    ++currentToolIndex;
                    if (currentToolIndex >= randomOrderedToolInfos.Length)
                    {
                        currentToolIndex = 0;
                    }
                }
            }

            return pinValues;
        }
    }
}