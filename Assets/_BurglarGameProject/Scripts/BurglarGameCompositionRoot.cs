using Assets._BurglarGameProject.Scripts.Gameplay;
using Assets._BurglarGameProject.Scripts.Pins;
using Assets._BurglarGameProject.Scripts.Settings;
using Assets._BurglarGameProject.Scripts.Time;
using Assets._BurglarGameProject.Scripts.Tools;
using UnityEngine;

namespace BurglarGame
{
    public class BurglarGameCompositionRoot : MonoBehaviour
    {
        [SerializeField]
        private BurglarGameSettings _gameSettings;
        [SerializeField]
        private CanvasAnimatedPinsView _pinsView;
        [SerializeField]
        private CanvasToolsView _toolsView;
        [SerializeField]
        private GameOverView _gameOverView;
        [SerializeField]
        private SecondsToLoseView _secondsToLoseView;

        private GameplaySystem _gameplaySystem;

        private void OnEnable()
        {
            RegisterListeners();
        }

        private void OnDisable()
        {
            UnregisterListeners();
            _gameplaySystem?.StopGame();
        }

        private void Awake()
        {
            _gameOverView.Init(_gameSettings.GameOverPanelAnimationDuration);
            _secondsToLoseView.Init(_gameSettings.SecondsToLose);
            _pinsView.Init(_gameSettings.MinPinValue, _gameSettings.MaxPinValue, 
                _gameSettings.PinValueChangeAnimationDuration);

            _gameplaySystem = new GameplaySystem(_gameSettings, _pinsView, _toolsView, 
                _gameOverView, _secondsToLoseView);

            RegisterListeners();
        }

        private void Start()
        {
            _gameplaySystem.RestartGame();
        }

        private void RegisterListeners()
        {
            UnregisterListeners();
            _gameplaySystem?.RegisterListeners();
        }

        private void UnregisterListeners()
        {
            _gameplaySystem?.UnregisterListeners();
        }
    }
}