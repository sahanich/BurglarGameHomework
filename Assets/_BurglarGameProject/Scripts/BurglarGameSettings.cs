﻿using UnityEngine;

namespace BurglarGame
{
    public interface IBurglarGameSettings
    {
        public int PinCount { get; }
        public int MinPinValue { get; }
        public int MaxPinValue { get; }
        public int TargetPinValue { get; }
        public ToolInfo[] Tools { get; }
        public int CodingIterationCount { get; }
        public int SecondsToLose { get; }
        public float PinValueChangeAnimationDuration { get; }
        public float GameOverPanelAnimationDuration { get; }
    }


    [CreateAssetMenu(fileName = "GameSettings", menuName = "ScriptableObjects/BurglarGameSettings")]
    public class BurglarGameSettings : ScriptableObject, IBurglarGameSettings
    {
        [field: Header("Pins Settings")]
        [field: SerializeField]
        public int PinCount { get; private set; } = 3;
        [field: SerializeField]
        public int MinPinValue { get; private set; } = 0;
        [field: SerializeField]
        public int MaxPinValue { get; private set; } = 10;
        [field: SerializeField]
        public int TargetPinValue { get; private set; } = 5;

        [field: Header("Tools Settings")]
        [field: SerializeField]
        public ToolInfo[] Tools { get; private set; }

        [field: Header("Difficulty Settings")]
        [field: SerializeField]
        public int CodingIterationCount { get; private set; } = 5;
        [field: SerializeField]
        public int SecondsToLose { get; private set; } = 60;

        [field: Header("Display Settings")]
        [field: SerializeField]
        public float PinValueChangeAnimationDuration { get; private set; } = 0.4f;
        [field: SerializeField]
        public float GameOverPanelAnimationDuration { get; private set; } = 0.5f;
    }
}