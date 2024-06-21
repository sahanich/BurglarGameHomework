using System;

namespace Assets._BurglarGameProject.Scripts.Time
{
    public interface IGameTimeModel
    {
        public event Action TimeChanged;
        public float CurrentTime { get; }
        public void SetTime(float time);
    }
}