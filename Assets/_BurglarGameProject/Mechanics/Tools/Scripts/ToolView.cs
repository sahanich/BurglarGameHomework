using System;
using System.Collections.Generic;
using System.Text;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace _BurglarGameProject.Mechanics.Tools.Scripts
{
    public class ToolView : MonoBehaviour
    {
        public event Action<ToolInfo> ApplyToolButtonClicked;

        private static readonly Dictionary<ToolType, string> _toolNamesMap = new()
        {
            [ToolType.Drill] = "дрель",
            [ToolType.Hammer] = "молоток",
            [ToolType.SkeletonKey] = "отмычка",
        };

        [field: SerializeField] public ToolType ToolType { get; private set; }

        [SerializeField] private AudioSource ToolAudioSource;
        [SerializeField] private TMP_Text ToolInfluenceToLockText;
        [SerializeField] private Button ApplyButton;


        public ToolInfo ToolInfo { get; private set; }

        private void OnEnable()
        {
            ApplyButton.onClick.AddListener(OnApplyButtonClick);
        }

        private void OnDisable()
        {
            ApplyButton.onClick.RemoveListener(OnApplyButtonClick);
        }

        public void Init(ToolInfo toolInfo)
        {
            ToolInfo = toolInfo;
            RefreshApplyButtonText();
        }

        public void SetToolInfo(ToolInfo toolInfo)
        {
            ToolInfo = toolInfo;
            RefreshApplyButtonText();
        }

        public void SetApplyButtonInteractable(bool interactable)
        {
            ApplyButton.interactable = interactable;
        }

        private void RefreshApplyButtonText()
        {
            if (ToolInfo == null || ToolInfo.PinChangeValues == null)
            {
                Debug.LogError($"{nameof(ToolView)}.{nameof(RefreshApplyButtonText)}. Wrong ToolInfo.");
                return;
            }

            StringBuilder stringBuilder = new StringBuilder(ToolInfo.PinChangeValues.Length);

            for (int i = 0; i < ToolInfo.PinChangeValues.Length; i++)
            {
                int changeValue = ToolInfo.PinChangeValues[i];
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

                stringBuilder.Append(i == 0 ? $"{appendingString}" : $"|{appendingString}");
            }

            ToolInfluenceToLockText.text = $"{stringBuilder}\n{_toolNamesMap[ToolInfo.ToolType]}";
        }

        private void OnApplyButtonClick()
        {
            ApplyToolButtonClicked?.Invoke(ToolInfo);

            if (ToolAudioSource.isPlaying)
            {
                ToolAudioSource.Stop();
            }

            ToolAudioSource.Play();
        }
    }
}