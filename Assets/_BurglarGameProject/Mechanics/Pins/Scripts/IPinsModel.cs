using System;

namespace _BurglarGameProject.Mechanics.Pins.Scripts
{
    public interface IPinsModel
    {
        public event Action StateChanged;
        public int[] State { get; }
        public int[] TargetState { get; }
        public int MinPossibleValue { get; }
        public int MaxPossibleValue { get; }
        public bool IsWinState();
        public void SetState(int[] pinValues);
    }
}