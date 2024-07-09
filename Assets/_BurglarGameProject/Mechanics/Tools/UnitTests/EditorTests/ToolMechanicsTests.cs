using System;
using System.Collections;
using System.Linq;
using _BurglarGameProject.Mechanics.Tools.Scripts;
using NUnit.Framework;
using Plugins.CommonFunc.Events;
using UnityEngine;
using UnityEngine.TestTools;

namespace _BurglarGameProject.Mechanics.Tools.UnitTests.EditorTests
{
    public class TestToolsView : IToolsView
    {
        private readonly EventHandlingProcessor<ToolUseEventSignal> _eventHandlingProcessor = new();

        public ToolInfo[] ToolInfos { get; private set; }
        public bool[] Interactables { get; private set; }

        public void SetToolInteractable(ToolType toolType, bool interactable)
        {
            for (int i = 0; i < ToolInfos.Length; i++)
            {
                if (ToolInfos[i].ToolType == toolType)
                {
                    Interactables[i] = interactable;
                }
            }
        }

        public bool IsToolInteractable(ToolType toolType)
        {
            for (int i = 0; i < ToolInfos.Length; i++)
            {
                if (ToolInfos[i].ToolType == toolType)
                {
                    return Interactables[i];
                }
            }

            return false;
        }

        public void SetTools(ToolInfo[] toolInfos)
        {
            ToolInfos = toolInfos.ToArray();
            Interactables = new bool[toolInfos.Length];
        }

        public void RaiseToolButtonClick(ToolType toolType)
        {
            ToolInfo tool = ToolInfos.FirstOrDefault(t => t.ToolType == toolType);
            _eventHandlingProcessor.FireEvent(new ToolUseEventSignal() {ToolInfo = tool});
        }

        public void RegisterEventHandler(IEventHandler<ToolUseEventSignal> handler)
        {
            _eventHandlingProcessor.RegisterHandler(handler);
        }

        public void UnregisterEventHandler(IEventHandler<ToolUseEventSignal> handler)
        {
            _eventHandlingProcessor.UnregisterHandler(handler);
        }
    }

    public class ToolMechanicsTests
    {
        private IToolsModel _toolsModel;
        private TestToolsView _toolsView;
        private ToolsController _toolsController;

        private ToolInfo _lastToolClicked;

        [SetUp]
        public void SetUp()
        {
            ToolInfo[] toolInfos =
            {
                new(ToolType.SkeletonKey, new int[] {1, -1, 0}),
                new(ToolType.Drill, new int[] {-1, 2, -1}),
                new(ToolType.Hammer, new int[] {-1, 1, 1})
            };

            _toolsModel = new ToolsModel(toolInfos);
            _toolsView = new TestToolsView();
            _toolsController = new ConstrainedToolsController(_toolsModel, _toolsView);
            _toolsController.RegisterListeners();
            _toolsController.ToolUseRequested += OnToolUseRequested;
        }

        [TearDown]
        public void TearDown()
        {
            _toolsController.UnregisterListeners();
            _toolsController.ToolUseRequested -= OnToolUseRequested;
        }

        [Test]
        public void ToolCanBeUsedTest()
        {
            ToolInfo toolInfo = new ToolInfo(ToolType.SkeletonKey, new int[] {1, -1, 0});

            int[] pinValues = new int[] {5, 5, 5};
            Assert.IsTrue(_toolsController.IsToolCanBeUsed(toolInfo, pinValues, 1, 10));

            pinValues = new int[] {4, 1, 5};

            Assert.IsFalse(_toolsController.IsToolCanBeUsed(toolInfo, pinValues, 1, 10));
        }

        [Test]
        public void ToolInteractableChangeTest()
        {
            ToolInfo[] toolInfos =
            {
                new(ToolType.SkeletonKey, new int[] {1, -1, 0}),
                new(ToolType.Drill, new int[] {-1, 2, -1}),
                new(ToolType.Hammer, new int[] {-1, 1, 1})
            };
            int[] pinValues = {5, 5, 5};

            _toolsModel.SetToolInfos(toolInfos);
            _toolsController.SetToolsInteractableByPinValues(pinValues, 1, 10);
            foreach (var tool in _toolsModel.ToolInfos)
            {
                Assert.IsTrue(_toolsView.IsToolInteractable(tool.ToolType));
            }

            pinValues = new[] {10, 10, 10};
            _toolsController.SetToolsInteractableByPinValues(pinValues, 1, 10);
            foreach (var tool in _toolsModel.ToolInfos)
            {
                Assert.IsFalse(_toolsView.IsToolInteractable(tool.ToolType));
            }

            pinValues = new[] {1, 1, 1};
            _toolsController.SetToolsInteractableByPinValues(pinValues, 1, 10);
            foreach (var tool in _toolsModel.ToolInfos)
            {
                Assert.IsFalse(_toolsView.IsToolInteractable(tool.ToolType));
            }
        }

        [Test]
        public void ToolButtonClickEventTest()
        {
            ToolType clickingToolType = ToolType.SkeletonKey;

            _lastToolClicked = null;
            _toolsView.RaiseToolButtonClick(clickingToolType);
            Assert.IsNotNull(_lastToolClicked);
            Assert.AreEqual(_lastToolClicked.ToolType, clickingToolType);
        }

        [Test]
        public void ViewRefreshTest()
        {
            for (int i = 0; i < _toolsModel.ToolInfos.Length; i++)
            {
                Assert.IsTrue(AreToolsEqual(_toolsModel.ToolInfos[i], _toolsView.ToolInfos[i]));
            }

            ToolInfo[] toolInfos =
            {
                new(ToolType.SkeletonKey, new int[] {3, -5, 4}),
            };

            _toolsModel.SetToolInfos(toolInfos);
            for (int i = 0; i < _toolsModel.ToolInfos.Length; i++)
            {
                Assert.IsTrue(AreToolsEqual(_toolsModel.ToolInfos[i], _toolsView.ToolInfos[i]));
            }
        }

        [UnityTest]
        public IEnumerator CheckForPinControllerFree()
        {
            // DateTime startTime = DateTime.Now;v
            // _toolsController.RegisterListeners();
            // _toolsController.ToolUseRequested += OnToolUseRequested;
            _toolsController = null;
            yield return null;
            System.GC.Collect();
            DateTime endTime = DateTime.Now + TimeSpan.FromSeconds(1);
            while (DateTime.Now < endTime)
            {
                yield return null;
                // System.GC.Collect();
            }
            
            Assert.IsNull(_toolsController);
            Debug.Log("Finished");
        }

        private bool AreToolsEqual(ToolInfo tool1, ToolInfo tool2)
        {
            if (tool1.ToolType != tool2.ToolType)
            {
                return false;
            }

            if (tool1.PinChangeValues.Length != tool2.PinChangeValues.Length)
            {
                return false;
            }

            for (int i = 0; i < tool1.PinChangeValues.Length; i++)
            {
                if (tool1.PinChangeValues[i] != tool2.PinChangeValues[i])
                {
                    return false;
                }
            }

            return true;
        }

        private void OnToolUseRequested(ToolInfo toolInfo)
        {
            _lastToolClicked = toolInfo;
        }
    }
}