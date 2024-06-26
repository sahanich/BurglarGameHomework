using System;

namespace _BurglarGameProject.Mechanics.Time.Scripts
{
    public interface IGameTimer
    {
        public event Action<float> Ticked;
        public void Start();
        public void Stop();
    }
}