using System;
using System.Collections;
using System.Linq;
using _BurglarGameProject.Mechanics.Pins.UnitTests.EditorTests;
using _BurglarGameProject.Mechanics.Time.Scripts;
using _BurglarGameProject.Mechanics.Tools.Scripts;
using _BurglarGameProject.Mechanics.Tools.UnitTests.EditorTests;
using _BurglarGameProject.Scripts;
using _BurglarGameProject.Scripts.Gameplay;
using _BurglarGameProject.Scripts.Settings;
using _BurglarGameProject.Scripts.Views;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

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
            BurglarGameCompositionRoot mainCompositionRoot = 
                GameObject.FindObjectOfType<BurglarGameCompositionRoot>();
            
            Assert.IsNotNull(mainCompositionRoot);

            _gameSettings = mainCompositionRoot.GameSettings;

            Assert.IsNotNull(_gameSettings);

            _gameOverView = new TestGameOverView();
            _gameTimeView = new TestGameTimeView();
            _pinsView = new TestPinsView();
            _toolsView = new TestToolsView();

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

            bool isTargetStateCorrelatesSettings = true;
            foreach (int targetPinValue in targetPinValues)
            {
                if (targetPinValue != _gameSettings.TargetPinValue)
                {
                    isTargetStateCorrelatesSettings = false;
                    break;
                }
            }

            Assert.IsTrue(isTargetStateCorrelatesSettings);
        }

        [Test]
        public void IsStartPinsStateNotEqualTargetState()
        {
            _pinsSystem.Reset();
            int[] pinValues = _pinsSystem.PinsModel.State;

            bool isStartStateEqualTargetState = true;
            foreach (int pinValue in pinValues)
            {
                if (pinValue != _gameSettings.TargetPinValue)
                {
                    isStartStateEqualTargetState = false;
                    break;
                }
            }

            Assert.IsFalse(isStartStateEqualTargetState);
        }

        [Test]
        public void IsToolsUseFromViewCorrect()
        {
            _pinsSystem.PinsModel.SetState(new int[]{ 5, 5, 5});
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

        // A UnityTest behaves like a coroutine in Play Mode. In Edit Mode you can use
        // `yield return null;` to skip a frame.
        [UnityTest]
        public IEnumerator BurglarEditorTestWithEnumeratorPasses()
        {
            // Use the Assert class to test conditions.
            // Use yield to skip a frame.
            yield return null;
        }
    }
}