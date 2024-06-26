using Assets._BurglarGameProject.Scripts.Settings;
using System.Collections.Generic;
using System.Linq;

namespace Assets._BurglarGameProject.Scripts.Pins
{
    public class PinsSystem
    {
        private IPinsModel _pinsModel;
        private PinsController _pinsController;
        private IBurglarGameSettings _gameSettings;

        public IPinsModel PinsModel => _pinsModel;
        public PinsController PinsController => _pinsController;

        public PinsSystem(IBurglarGameSettings gameSettings, IPinsView pinsView)
        {
            _gameSettings = gameSettings;

            _pinsModel = new OneTargetValuePinsModel(_gameSettings.PinCount, _gameSettings.MinPinValue,
                _gameSettings.MaxPinValue, _gameSettings.TargetPinValue);

            List<int[]> tools = new List<int[]>();
            foreach (var tool in _gameSettings.Tools)
            {
                tools.Add(tool.PinChangeValues.ToArray());
            }
            _pinsController = new ToolBasedPinsController(_pinsModel, pinsView,
                tools, _gameSettings.CodingIterationCount);
        }

        public void Reset()
        {
            _pinsController.SetStartState();
        }

        public void RegisterListeners()
        {
            UnregisterListeners();

            _pinsController?.RegisterListeners();
        }

        public void UnregisterListeners()
        {
            _pinsController?.UnregisterListeners();
        }
    }
}