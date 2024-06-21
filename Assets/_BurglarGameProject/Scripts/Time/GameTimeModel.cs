using System;

namespace Assets._BurglarGameProject.Scripts.Time
{
    public sealed class GameTimeModel : IGameTimeModel
    {
        public event Action TimeChanged;
        public float CurrentTime { get; private set; }

        public void SetTime(float time)
        {
            CurrentTime = time;
            TimeChanged?.Invoke();
        }
    }
}