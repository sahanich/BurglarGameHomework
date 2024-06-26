using System;
using UnityEngine;

namespace _BurglarGameProject.Mechanics.Tools.Scripts
{
    [Serializable]
    public sealed class ToolInfo
    {
        [field: SerializeField]
        public ToolType ToolType { get; private set; }
        [field: SerializeField]
        public int[] PinChangeValues { get; private set; }

        public ToolInfo(ToolType toolType, int[] pinChangeValues)
        {
            ToolType = toolType;
            PinChangeValues = pinChangeValues;
        }
    }
}