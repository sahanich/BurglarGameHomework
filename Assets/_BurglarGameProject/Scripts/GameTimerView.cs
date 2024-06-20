using System;
using System.Collections;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;

namespace BurglarGame
{
    public interface IGameTimeModel
    {
        public event Action TimeChanged;
        public float CurrentTime { get; }
        public void SetTime(float time);
    }

    public interface IGameTimeView
    {
        public void SetTime(float time);
    }

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

    public interface IGameTimer
    {
        public event Action<float> Ticked;
        public void Start();
        public void Stop();
    }

    public sealed class GameTimeController
    {
        private IGameTimeModel _model;
        private IGameTimeView _view;
        private IGameTimer _gameTimer;

        public GameTimeController(IGameTimeModel model, IGameTimeView view, IGameTimer gameTimer)
        {
            _model = model;
            _view = view;
            _gameTimer = gameTimer;
        }

        public void RegisterListeners()
        {
            _model.TimeChanged += OnModelTimeChanged;
            _gameTimer.Ticked += OnGameTimerTicked;
        }

        public void UnregisterListeners()
        {
            _model.TimeChanged -= OnModelTimeChanged;
            _gameTimer.Ticked -= OnGameTimerTicked;
        }

        private void OnGameTimerTicked(float deltaTime)
        {
            _model.SetTime(_model.CurrentTime + deltaTime);
        }

        private void OnModelTimeChanged()
        {
            _view.SetTime(_model.CurrentTime);
        }
    }

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

    public class GameTimerView : MonoBehaviour
    {
        [SerializeField]
        private float TimerUpdateFrequency = 1;

        public float CurrentGameTimeInSec { get; private set; }

        private BurglarGameEventsHandler _eventsHandler;

        public void Init(BurglarGameEventsHandler eventsHandler)
        {
            _eventsHandler = eventsHandler;
        }

        public void StartGameTimer()
        {
            StopAllCoroutines();

            CurrentGameTimeInSec = 0;
            _eventsHandler.RaiseTimerUpdated();

            StartCoroutine(TimerRoutine());
        }

        public void StopGameTimer()
        {
            StopAllCoroutines();
        }

        private IEnumerator TimerRoutine()
        {
            WaitForSeconds timerUpdateYieldInstruction = new(TimerUpdateFrequency);

            while (true)
            {
                yield return timerUpdateYieldInstruction;
                CurrentGameTimeInSec += TimerUpdateFrequency;
                _eventsHandler.RaiseTimerUpdated();
            }
        }
    }
}