using System;
using System.Threading;
using System.Threading.Tasks;

namespace Assets._BurglarGameProject.Scripts.Time
{
    public class TaskGameTimer : IGameTimer
    {
        public event Action<float> Ticked;

        private float _timerUpdateFrequency;
        private CancellationTokenSource _currentCancelTokenSource;

        public TaskGameTimer(float timerUpdateFrequency)
        {
            _timerUpdateFrequency = timerUpdateFrequency;
        }

        public void Start()
        {
            Stop();
            _currentCancelTokenSource = new CancellationTokenSource();
            TimerTicker(_currentCancelTokenSource.Token);
        }

        public void Stop()
        {
            _currentCancelTokenSource?.Cancel();
        }

        private async void TimerTicker(CancellationToken cancellationToken)
        {
            while (!cancellationToken.IsCancellationRequested)
            {
                await Task.Run(() =>
                {
                    Thread.Sleep(TimeSpan.FromSeconds(_timerUpdateFrequency));
                },
                cancellationToken);

                if (!cancellationToken.IsCancellationRequested)
                {
                    Ticked?.Invoke(_timerUpdateFrequency);
                }
            }
        }
    }
}