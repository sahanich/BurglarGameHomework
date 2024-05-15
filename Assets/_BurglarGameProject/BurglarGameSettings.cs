using System.ComponentModel;
using UnityEngine;

namespace BurglarGame
{
    //public class GameManager : MonoBehaviour
    //{

    //}

    //public class TimeController
    //{
    //    internal void Init(BurglarGameSettings gameSettings, TimerView timerView)
    //    {
    //        //throw new NotImplementedException();
    //    }
    //}

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

        //[field: SerializeField]
        //public ToolType[] Tools { get; private set; } = new[] { ToolType.Drill, ToolType.Hammer, ToolType.SkeletonKey };
        //[field: SerializeField]
        //public int MinToolPinChangeValue { get; private set; } = -2;
        //[field: SerializeField]
        //public int MaxToolPinChangeValue { get; private set; } = 2;
    }
}