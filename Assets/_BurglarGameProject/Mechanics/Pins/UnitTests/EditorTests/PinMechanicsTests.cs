using System.Collections.Generic;
using System.Linq;
using _BurglarGameProject.Mechanics.Pins.Scripts;
using NUnit.Framework;

namespace _BurglarGameProject.Mechanics.Pins.UnitTests.EditorTests
{
    public class TestPinsView : IPinsView
    {
        public int[] PinValues;
        
        public void SetPinValues(int[] pinValues)
        {
            PinValues = pinValues.ToArray();
        }
    }

    public class PinMechanicsTests
    {
        private IPinsModel _pinsModel;
        private TestPinsView _pinsView;
        private PinsController _pinsController;

        [SetUp]
        public void SetUp()
        {
            _pinsModel = new OneTargetValuePinsModel(3, 1, 10, 5);

            List<int[]> tools = new List<int[]>
            {
                new int[] {1, -1, 0},
                new int[] {-1, 2, -1},
                new int[] {-1, 1, 1},
            };

            _pinsView = new TestPinsView();
            _pinsController = new ToolBasedPinsController(_pinsModel, _pinsView,
                tools, 3);
            _pinsController.RegisterListeners();
        }

        [TearDown]
        public void TearDown()
        {
            _pinsController.UnregisterListeners();
            _pinsModel = null;
            _pinsController = null;
            _pinsView = null;
        }

        [Test]
        public void StartStateCorrespondsToTargetStateTest()
        {
            _pinsController.SetStartState();

            bool isStartStateEqualTargetState = true;
            for (int i = 0; i < _pinsModel.State.Length; i++)
            {
                if (_pinsModel.State[i] != _pinsModel.TargetState[i])
                {
                    isStartStateEqualTargetState = false;
                    break;
                }
            }

            Assert.IsFalse(isStartStateEqualTargetState);
        }

        [Test]
        public void PinsChangeTest()
        {
            int[] pinStartValues = new int[] { 1, 2, 3 };
            int[] pinChangeValues = new int[] { 1, 2, -1 };
            int[] pinEndValues = new int[pinStartValues.Length];
            for (int i = 0; i < pinStartValues.Length; i++)
            {
                pinEndValues[i] = pinStartValues[i] + pinChangeValues[i];
            }

            _pinsModel.SetState(pinStartValues);
            _pinsController.AddPinValues(pinChangeValues);

            for (int i = 0; i < _pinsModel.State.Length; i++)
            {
                Assert.AreEqual(pinEndValues[i], _pinsModel.State[i]);
            }
        }

        [Test]
        public void PinsConstraintsTest()
        {
            _pinsController.SetStartState();
            int[] pinChangeValues = new int[] { -10, 20, -30 };
            _pinsController.AddPinValues(pinChangeValues);

            bool areValuesBetweenMinMax = true;
            for (int i = 0; i < _pinsModel.State.Length; i++)
            {
                if (_pinsModel.State[i] > _pinsModel.MaxPossibleValue
                    || _pinsModel.State[i] < _pinsModel.MinPossibleValue)
                {
                    areValuesBetweenMinMax = false;
                    break;
                }
            }
            Assert.IsTrue(areValuesBetweenMinMax);
        }

        [Test]
        public void ViewRefreshTest()
        {
            _pinsController.SetStartState();
            for (int i = 0; i < _pinsView.PinValues.Length; i++)
            {
                Assert.AreEqual(_pinsView.PinValues[i], _pinsModel.State[i]);
            }
        }
    }
}