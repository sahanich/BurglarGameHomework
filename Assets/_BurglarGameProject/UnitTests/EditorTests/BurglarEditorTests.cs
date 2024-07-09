using System;
using System.Linq;
using _BurglarGameProject.Mechanics.Pins.UnitTests.EditorTests;
using _BurglarGameProject.Mechanics.Time.Scripts;
using _BurglarGameProject.Mechanics.Tools.Scripts;
using _BurglarGameProject.Mechanics.Tools.UnitTests.EditorTests;
using _BurglarGameProject.Scripts.Gameplay;
using _BurglarGameProject.Scripts.Settings;
using _BurglarGameProject.Scripts.Views;
using NUnit.Framework;
using UnityEngine;

namespace _BurglarGameProject.UnitTests.EditorTests
{
    public class TestGameTimeView : IGameTimeView
    {
        public void SetTime(float time)
        {
        }
    }

    public class TestGameOverView : IGameOverView
    {
        public event Action RestartButtonClicked;

        public void Hide()
        {
        }

        public void ShowLoseScreen()
        {
        }

        public void ShowWinScreen()
        {
        }

        public void ClickRestartButton()
        {
            RestartButtonClicked?.Invoke();
        }
    }

    public class BurglarEditorTest
    {
        private IBurglarGameSettings _gameSettings;

        private GameplaySystem _gameplaySystem;
        private PinsSystem _pinsSystem;
        private ToolsSystem _toolsSystem;
        private TimeSystem _timeSystem;

        private IGameOverView _gameOverView;
        private IGameTimeView _gameTimeView;
        private TestPinsView _pinsView;
        private TestToolsView _toolsView;

        [SetUp]
        public void Setup()
        {
            var gameSettingsArray = Resources.FindObjectsOfTypeAll<BurglarGameSettings>();
            if (gameSettingsArray.Length == 0)
            {
                gameSettingsArray = Resources.LoadAll<BurglarGameSettings>("");
            }

            Assert.IsTrue(gameSettingsArray.Length > 0);
            
            _gameSettings = gameSettingsArray[0];

            Assert.IsNotNull(_gameSettings);

            _gameOverView = new TestGameOverView();
            _gameTimeView = new TestGameTimeView();
            _pinsView = new TestPinsView();
            _toolsView = null;//;new TestToolsView();

            _pinsSystem = new PinsSystem(_gameSettings, _pinsView);
            _toolsSystem = new ToolsSystem(_gameSettings, _toolsView);
            _timeSystem = new TimeSystem(_gameSettings, _gameTimeView);

            _gameplaySystem = new GameplaySystem(_pinsSystem, _toolsSystem, _timeSystem,
                _gameOverView);

            _gameplaySystem.RegisterListeners();
        }

        [TearDown]
        public void TearDown()
        {
            _gameplaySystem.UnregisterListeners();
        }

        [Test]
        public void IsTargetPinsStateCorrelatesSettings()
        {
            int[] targetPinValues = _pinsSystem.PinsModel.TargetState;

            bool isTargetStateCorrelatesSettings =
                targetPinValues.All(targetPinValue => targetPinValue == _gameSettings.TargetPinValue);

            Assert.IsTrue(isTargetStateCorrelatesSettings);
        }

        [Test]
        public void IsStartPinsStateNotEqualTargetState()
        {
            _pinsSystem.Reset();
            int[] pinValues = _pinsSystem.PinsModel.State;

            bool isStartStateEqualTargetState =
                pinValues.All(pinValue => pinValue == _gameSettings.TargetPinValue);

            Assert.IsFalse(isStartStateEqualTargetState);
        }

        [Test]
        public void IsToolsUseFromViewCorrect()
        {
            _pinsSystem.PinsModel.SetState(new int[] {5, 5, 5});
            ToolInfo toolToUse = _toolsSystem.ToolsModel.ToolInfos[0];

            int[] endPinValues = _pinsSystem.PinsModel.State.ToArray();
            for (int i = 0; i < endPinValues.Length; i++)
            {
                endPinValues[i] += toolToUse.PinChangeValues[i];
            }

            _toolsView.RaiseToolButtonClick(toolToUse.ToolType);

            for (int i = 0; i < endPinValues.Length; i++)
            {
                Assert.AreEqual(_pinsSystem.PinsModel.State[i], endPinValues[i]);
            }
        }
    }
}