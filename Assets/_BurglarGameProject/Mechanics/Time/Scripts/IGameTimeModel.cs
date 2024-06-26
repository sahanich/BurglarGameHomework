using System;

namespace _BurglarGameProject.Mechanics.Time.Scripts
{
    public interface IGameTimeModel
    {
        public event Action TimeChanged;
        public float CurrentTime { get; }
        public void SetTime(float time);
    }
}