using System;

namespace Assets._BurglarGameProject.Scripts.Pins
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