namespace Assets._BurglarGameProject.Scripts.Time
{
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
}