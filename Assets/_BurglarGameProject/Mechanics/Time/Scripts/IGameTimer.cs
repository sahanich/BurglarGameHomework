using System;

namespace Assets._BurglarGameProject.Scripts.Time
{
    public interface IGameTimer
    {
        public event Action<float> Ticked;
        public void Start();
        public void Stop();
    }
}