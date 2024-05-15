using System;
using System.Collections.Generic;
using System.Text;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace BurglarGame
{
    public class ToolView : MonoBehaviour
    {
        private static readonly Dictionary<ToolType, string> _toolNamesMap = new Dictionary<ToolType, string>()
        {
            [ToolType.Drill] = "дрель",
            [ToolType.Hammer] = "молоток",
            [ToolType.SkeletonKey] = "отмычка",
        };

        //public Action<ToolInfo> ApplyButtonClicked;

        [field: SerializeField]
        public ToolType ToolType { get; private set; }

        [SerializeField]
        private TMP_Text ToolInfluenceToLockText;
        [SerializeField]
        private Button ApplyButton;

        private ToolInfo _toolInfo;
        private BurglarGameEventsHandler _eventsHandler;

        private void OnEnable()
        {
            ApplyButton.onClick.AddListener(OnApplyButtonClick);
        }

        private void OnDisable()
        {
            ApplyButton.onClick.RemoveListener(OnApplyButtonClick);
        }

        public void Init(BurglarGameEventsHandler eventsHandler)
        {
            _eventsHandler = eventsHandler;
        }

        public void SetToolInfo(ToolInfo toolInfo)
        {
            _toolInfo = toolInfo;
            RefreshApplyButtonText();
        }

        //public bool CanBeUsed(int[] pinValues, int minPinValue, int maxPinValue)
        //{
        //    bool doArraysMuchEachOther = pinValues != null && _pinChangeValues != null
        //        && pinValues.Length == _pinChangeValues.Length;

        //    if (!doArraysMuchEachOther)
        //    {
        //        Debug.LogError("PinData and ToolData don't match");
        //        return false;
        //    }

        //    for (int i = 0; i < pinValues.Length; i++)
        //    {
        //        int applyingResult = pinValues[i] + _pinChangeValues[i];
        //        if (applyingResult < minPinValue || applyingResult > maxPinValue)
        //        {
        //            return false;
        //        }
        //    }

        //    return true;
        //}

        public void SetApplyButtonInteractable(bool interactable)
        {
            ApplyButton.interactable = interactable;
        }

        //public int GetPinChangeValue(int pinIndex)
        //{
        //    if (_pinChangeValues == null || pinIndex < 0 || pinIndex >= _pinChangeValues.Length)
        //    {
        //        Debug.LogError("Trying to get unexisting pin value");
        //        return 0;
        //    }
        //    return _pinChangeValues[pinIndex];
        //}

        private void RefreshApplyButtonText()
        {
            if (_toolInfo == null || _toolInfo.PinChangeValues == null)
            {
                Debug.LogError($"{nameof(ToolView)}.{nameof(RefreshApplyButtonText)}. Wrong ToolInfo.");
                return;
            }

            StringBuilder stringBuilder = new StringBuilder(_toolInfo.PinChangeValues.Length);

            for (int i = 0; i < _toolInfo.PinChangeValues.Length; i++)
            {
                int changeValue = _toolInfo.PinChangeValues[i];
                string appendingString;

                if (changeValue == 0)
                {
                    appendingString = $" 0";
                }
                else if (changeValue > 0)
                {
                    appendingString = $"+{changeValue}";
                }
                else
                {
                    appendingString = $"{changeValue}";
                }

                if (i == 0)
                {
                    stringBuilder.Append($"{appendingString}");
                }
                else
                {
                    stringBuilder.Append($"|{appendingString}");
                }
            }

            ToolInfluenceToLockText.text = $"{stringBuilder}\n{_toolNamesMap[_toolInfo.ToolType]}";
        }

        private void OnApplyButtonClick()
        {
            _eventsHandler?.RaiseToolUseRequested(_toolInfo);
            //ApplyButtonClicked?.Invoke(_toolInfo);
        }
    }
}