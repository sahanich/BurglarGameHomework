using System;
using System.Collections;
using _BurglarGameProject.Mechanics.Time.Scripts;
using NUnit.Framework;
using UnityEngine.TestTools;

namespace _BurglarGameProject.Mechanics.Time.UnitTests.EditorTests
{
    public class TestGameTimeView : IGameTimeView
    {
        public float Time;
        public void SetTime(float time)
        {
            Time = time;
        }
    }
    
    public class TimeMechanicsTests
    {
        private const float TestTimerFrequency = 0.1f;
        
        private IGameTimeModel _gameTimeModel;
        private TestGameTimeView _gameTimeView;
        private GameTimeController _gameTimeController;
        private IGameTimer _timer;
        
        [SetUp]
        public void SetUp()
        {
             _timer = new TaskGameTimer(TestTimerFrequency);
            _gameTimeModel = new GameTimeModel();
            _gameTimeView = new TestGameTimeView();
            _gameTimeController = new GameTimeController(_gameTimeModel, _gameTimeView, _timer);
            _gameTimeController.RegisterListeners();
        }

        [TearDown]
        public void TearDown()
        {
            _gameTimeController.UnregisterListeners();
            _timer.Stop();
        }
        
        [UnityTest]
        public IEnumerator GameTimeRefreshTest()
        {
            Assert.AreEqual(_gameTimeModel.CurrentTime, 0);
            Assert.AreEqual(_gameTimeView.Time, 0);

            _timer.Start();
            
            DateTime startTime = DateTime.Now;
            DateTime endTime = startTime + TimeSpan.FromSeconds(TestTimerFrequency);
            while (DateTime.Now < endTime)
            {
                yield return null;
            }
            yield return null;

            float endTimeInSeconds = (float)(endTime - startTime).TotalSeconds;
            Assert.AreEqual(_gameTimeModel.CurrentTime, endTimeInSeconds, 0.01f);
            Assert.AreEqual(_gameTimeView.Time, endTimeInSeconds, 0.01f);
        }
    }
}
