using System;
using System.Linq;
using _BurglarGameProject.Mechanics.Tools.Scripts;
using NUnit.Framework;

namespace _BurglarGameProject.Mechanics.Tools.UnitTests.EditorTests
{
    public class TestToolsView : IToolsView
    {
        public event Action<ToolInfo> ToolUseRequested;

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
            ToolUseRequested?.Invoke(tool);
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
            ToolInfo[] toolInfos = new ToolInfo[]
            {
                new ToolInfo(ToolType.SkeletonKey, new int[] { 1, -1, 0 }),
                new ToolInfo(ToolType.Drill, new int[] { -1, 2, -1 }),
                new ToolInfo(ToolType.Hammer, new int[] { -1, 1, 1 })
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
            ToolInfo toolInfo = new ToolInfo(ToolType.SkeletonKey, new int[] { 1, -1, 0 });

            int[] pinValues = new int[] { 5, 5, 5 };
            Assert.IsTrue(_toolsController.IsToolCanBeUsed(toolInfo, pinValues, 1, 10));

            pinValues = new int[] { 4, 1, 5 };

            Assert.IsFalse(_toolsController.IsToolCanBeUsed(toolInfo, pinValues, 1, 10));
        }

        [Test]
        public void ToolInteractableChangeTest()
        {
            ToolInfo[] toolInfos = new ToolInfo[]
            {
                new ToolInfo(ToolType.SkeletonKey, new int[] { 1, -1, 0 }),
                new ToolInfo(ToolType.Drill, new int[] { -1, 2, -1 }),
                new ToolInfo(ToolType.Hammer, new int[] { -1, 1, 1 })
            };
            int[] pinValues = new int[] { 5, 5, 5 };

            _toolsModel.SetToolInfos(toolInfos);
            _toolsController.SetToolsInteractableByPinValues(pinValues, 1, 10);
            foreach (var tool in _toolsModel.ToolInfos)
            {
                Assert.IsTrue(_toolsView.IsToolInteractable(tool.ToolType));
            }

            pinValues = new int[] { 10, 10, 10 };
            _toolsController.SetToolsInteractableByPinValues(pinValues, 1, 10);
            foreach (var tool in _toolsModel.ToolInfos)
            {
                Assert.IsFalse(_toolsView.IsToolInteractable(tool.ToolType));
            }

            pinValues = new int[] { 1, 1, 1 };
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

            ToolInfo[] toolInfos = new ToolInfo[]
            {
                new ToolInfo(ToolType.SkeletonKey, new int[] { 3, -5, 4 }),
            };

            _toolsModel.SetToolInfos(toolInfos);
            for (int i = 0; i < _toolsModel.ToolInfos.Length; i++)
            {
                Assert.IsTrue(AreToolsEqual(_toolsModel.ToolInfos[i], _toolsView.ToolInfos[i]));
            }
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