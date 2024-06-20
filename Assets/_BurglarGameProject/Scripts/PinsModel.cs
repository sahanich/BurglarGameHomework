using System;
using System.Linq;
using UniRx;

namespace BurglarGame
{
    public abstract class PinsModel : IPinsModel
    {
        public event Action StateChanged;

        public int[] State { get; private set; }
        public int[] TargetState { get; protected set; }
        public int MinPossibleValue { get; }
        public int MaxPossibleValue { get; }

        public PinsModel(int pinCount, int minPossibleValue, int maxPossibleValue)
        {
            State = new int[pinCount];
            TargetState = new int[pinCount];
            MinPossibleValue = minPossibleValue;
            MaxPossibleValue = maxPossibleValue;
        }

        public bool IsWinState()
        {
            for (int i = 0; i < State.Length; i++)
            {
                if (State[i] != TargetState[i])
                {
                    return false;
                }
            }
            return true;
        }

        public void SetState(int[] pinValues)
        {
            State = pinValues.ToArray();
            for (int i = 0; i < State.Length; i++)
            {
                State[i] = Math.Clamp(State[i], MinPossibleValue, MaxPossibleValue);
            }
            StateChanged?.Invoke();
        }
    }
}