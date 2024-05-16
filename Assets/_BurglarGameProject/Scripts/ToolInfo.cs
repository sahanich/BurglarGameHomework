using System;
using UnityEngine;

namespace BurglarGame
{
    [Serializable]
    public class ToolInfo
    {
        [field: SerializeField]
        public ToolType ToolType { get; private set; }
        [field: SerializeField]
        public int[] PinChangeValues { get; private set; }
    }
}