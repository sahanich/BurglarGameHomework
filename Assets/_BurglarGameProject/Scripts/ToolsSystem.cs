namespace BurglarGame
{
    public class TimeSystem
    {
        private IGameTimeModel _gameTimeModel;
        private GameTimeController _gameTimeController;
        private IBurglarGameSettings _gameSettings;
        private IGameTimer _gameTimer;

        public IGameTimeModel GameTimeModel => _gameTimeModel;

        public TimeSystem(IBurglarGameSettings gameSettings, IGameTimeView gameTimeView)
        {
            _gameSettings = gameSettings;
            _gameTimer = new TaskGameTimer(1);
            _gameTimeModel = new GameTimeModel();
            _gameTimeController = new GameTimeController(_gameTimeModel, gameTimeView, _gameTimer);
        }

        public void Reset()
        {
            _gameTimer.Stop();
            _gameTimeModel.SetTime(0);
        }

        public void StartTimer()
        {
            _gameTimer.Start();
        }

        public void StopTimer()
        {
            _gameTimer.Stop();
        }

        public bool IsTimeOver()
        {
            return _gameTimeModel.CurrentTime >= _gameSettings.SecondsToLose;
        }

        public void RegisterListeners()
        {
            UnregisterListeners();
            _gameTimeController?.RegisterListeners();
        }

        public void UnregisterListeners()
        {
            _gameTimeController?.UnregisterListeners();
        }
    }

    public class ToolsSystem
    {
        private IToolsModel _toolsModel;
        private ToolsController _toolsController;

        public IToolsModel ToolsModel => _toolsModel;
        public ToolsController ToolsController => _toolsController;

        public ToolsSystem(IBurglarGameSettings gameSettings, IToolsView toolsView)
        {
            _toolsModel = new ToolsModel(gameSettings.Tools);
            _toolsController = new ConstrainedToolsController(_toolsModel, toolsView);
        }

        public void RegisterListeners()
        {
            UnregisterListeners();
            _toolsController?.RegisterListeners();
        }

        public void UnregisterListeners()
        {
            _toolsController?.UnregisterListeners();
        }
    }
}