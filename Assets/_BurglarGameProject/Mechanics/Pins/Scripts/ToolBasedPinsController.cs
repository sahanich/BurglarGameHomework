using System;
using System.Collections.Generic;
using System.Linq;

namespace _BurglarGameProject.Mechanics.Pins.Scripts
{
    public sealed class ToolBasedPinsController : PinsController
    {
        private List<int[]> _tools;
        private Random _random;
        private int _codingIterationsCount;

        public ToolBasedPinsController(IPinsModel model, IPinsView view,
            List<int[]> tools, int codingIterationsCount) : base(model, view)
        {
            _tools = tools.ToList();
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

        private void ShuffleList<T>(List<T> array)
        {
            for (int i = 0; i < array.Count - 1; i++)
            {
                int randomIndex = _random.Next(i, array.Count);
                T tempElement = array[i];
                array[i] = array[randomIndex];
                array[randomIndex] = tempElement;
            }
        }

        private bool IsRevertedToolApplyingResultInPinValuesRange(int[] toolPinChangingValues, int[] pinValues)
        {
            bool resultPinValueIsInRange = true;

            for (int i = 0; i < pinValues.Length; i++)
            {
                int pinValue = pinValues[i];
                int toolChangeValue = toolPinChangingValues[i];
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

        private bool TryApplyToolReverted(int[] toolPinChangingValues, int[] pinValues)
        {
            if (!IsRevertedToolApplyingResultInPinValuesRange(toolPinChangingValues, pinValues))
            {
                return false;
            }

            for (int i = 0; i < pinValues.Length; i++)
            {
                int pinValue = pinValues[i];
                int toolChangeValue = toolPinChangingValues[i];
                pinValues[i] = pinValue - toolChangeValue;
            }
            return true;

        }

        private int[] CreatePinStates(List<int[]> tools)
        {
            int[] pinValues = _model.TargetState.ToArray();
            List<int[]> randomOrderedToolInfos = tools.ToList();
            ShuffleList(randomOrderedToolInfos);

            int currentToolIndex = 0;
            for (int i = 0; i < _codingIterationsCount; i++)
            {
                int tryCountToApplyTool = randomOrderedToolInfos.Count;

                while (tryCountToApplyTool > 0)
                {
                    int[] toolPinChangingValues = randomOrderedToolInfos[currentToolIndex];

                    bool missTool = _random.Next(0, 2) == 1;

                    if (!missTool && TryApplyToolReverted(toolPinChangingValues, pinValues))
                    {
                        tryCountToApplyTool = 0;
                    }
                    else
                    {
                        --tryCountToApplyTool;
                    }

                    ++currentToolIndex;
                    if (currentToolIndex >= randomOrderedToolInfos.Count)
                    {
                        currentToolIndex = 0;
                    }
                }
            }

            return pinValues;
        }
    }
}