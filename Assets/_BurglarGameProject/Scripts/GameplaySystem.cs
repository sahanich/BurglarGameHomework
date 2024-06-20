namespace BurglarGame.Systems
{
    public class GameplaySystem
    {
        private PinsSystem _pinsSystem;
        private ToolsSystem _toolsSystem;
        private TimeSystem _timeSystem;
        private IGameOverView _gameOverView;

        public GameplaySystem(IBurglarGameSettings gameSettings, IPinsView pinsView, IToolsView toolsView,
            IGameOverView gameOverView, IGameTimeView gameTimeView)
        {
            _pinsSystem = new PinsSystem(gameSettings, pinsView);
            _toolsSystem = new ToolsSystem(gameSettings, toolsView);
            _timeSystem = new TimeSystem(gameSettings, gameTimeView);
            _gameOverView = gameOverView;
        }

        public void RestartGame()
        {
            _gameOverView.Hide();

            _pinsSystem.Reset();
            _timeSystem.Reset();
            _timeSystem.StartTimer();
        }

        public void StopGame()
        {
            _timeSystem.StopTimer();
        }

        public void RegisterListeners()
        {
            UnregisterListeners();
            
            if (_pinsSystem != null)
            {
                _pinsSystem.RegisterListeners();
                _pinsSystem.PinsModel.StateChanged += OnPinsModelStateChanged;
            }
            if (_toolsSystem != null)
            {
                _toolsSystem.RegisterListeners();
                _toolsSystem.ToolsController.ToolUseRequested += OnToolsSystemShiftPinsRequested;
            }
            if (_timeSystem != null)
            {
                _timeSystem.RegisterListeners();
                _timeSystem.GameTimeModel.TimeChanged += OnGameTimeTimeChanged;
            }
            if (_gameOverView != null)
            {
                _gameOverView.RestartButtonClicked += OnRestartButtonClicked;
            }
        }

        public void UnregisterListeners()
        {
            if (_pinsSystem != null)
            {
                _pinsSystem.UnregisterListeners();
                _pinsSystem.PinsModel.StateChanged -= OnPinsModelStateChanged;
            }
            if (_toolsSystem != null)
            {
                _toolsSystem.UnregisterListeners();
                _toolsSystem.ToolsController.ToolUseRequested -= OnToolsSystemShiftPinsRequested;
            }
            if (_timeSystem != null)
            {
                _timeSystem.UnregisterListeners();
                _timeSystem.GameTimeModel.TimeChanged -= OnGameTimeTimeChanged;
            }
            if (_gameOverView != null)
            {
                _gameOverView.RestartButtonClicked -= OnRestartButtonClicked;
            }
        }

        private void SetWinState()
        {
            StopGame();
            _toolsSystem.ToolsController.SetToolsInteractable(false);
            _gameOverView.ShowWinScreen();
        }

        private void SetLoseState()
        {
            StopGame();
            _toolsSystem.ToolsController.SetToolsInteractable(false);
            _gameOverView.ShowLoseScreen();
        }

        private void OnGameTimeTimeChanged()
        {
            if (_timeSystem.IsTimeOver())
            {
                SetLoseState();
            }
        }

        private void OnRestartButtonClicked()
        {
            RestartGame();
        }

        private void OnPinsModelStateChanged()
        {
            if (_pinsSystem.PinsModel.IsWinState())
            {
                SetWinState();
                return;
            }

            _toolsSystem.ToolsController.SetToolsInteractableByPinValues(
                _pinsSystem.PinsModel.State, 
                _pinsSystem.PinsModel.MinPossibleValue, 
                _pinsSystem.PinsModel.MaxPossibleValue);
        }

        private void OnToolsSystemShiftPinsRequested(ToolInfo tool)
        {
            _pinsSystem.PinsController.AddPinValues(tool.PinChangeValues);
        }
    }
}