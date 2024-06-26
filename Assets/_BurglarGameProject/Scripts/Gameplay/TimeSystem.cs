using Assets._BurglarGameProject.Scripts.Settings;

namespace Assets._BurglarGameProject.Scripts.Time
{
    public class TimeSystem
    {
        private IGameTimeModel _gameTimeModel;
        private GameTimeController _gameTimeController;
        private IGameTimer _gameTimer;
        private IBurglarGameSettings _burglarGameSettings;

        public IGameTimeModel GameTimeModel => _gameTimeModel;

        public TimeSystem(IBurglarGameSettings burglarGameSettings, IGameTimeView gameTimeView)
        {
            _burglarGameSettings = burglarGameSettings;
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
            return _gameTimeModel.CurrentTime >= _burglarGameSettings.SecondsToLose;
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
}