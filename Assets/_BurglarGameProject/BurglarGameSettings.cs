using System.ComponentModel;
using UnityEngine;

namespace BurglarGame
{
    [CreateAssetMenu(fileName = "GameSettings", menuName = "ScriptableObjects/BurglarGameSettings")]
    public class BurglarGameSettings : ScriptableObject
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
    }
}