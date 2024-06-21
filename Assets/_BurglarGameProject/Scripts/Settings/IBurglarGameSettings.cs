using Assets._BurglarGameProject.Scripts.Tools;

namespace Assets._BurglarGameProject.Scripts.Settings
{
    public interface IBurglarGameSettings
    {
        public int PinCount { get; }
        public int MinPinValue { get; }
        public int MaxPinValue { get; }
        public int TargetPinValue { get; }
        public ToolInfo[] Tools { get; }
        public int CodingIterationCount { get; }
        public int SecondsToLose { get; }
        public float PinValueChangeAnimationDuration { get; }
        public float GameOverPanelAnimationDuration { get; }
    }
}