namespace Assets._BurglarGameProject.Scripts.Tools
{
    public sealed class ConstrainedToolsController : ToolsController
    {
        public ConstrainedToolsController(IToolsModel toolsModel, IToolsView toolsView) : base(toolsModel, toolsView)
        {
        }

        public override bool IsToolCanBeUsed(ToolInfo toolInfo, int[] pinValues, int minPinValue, int maxPinValue)
        {
            bool toolCanChangePins = true;
            for (int i = 0; i < pinValues.Length; i++)
            {
                int pinValue = pinValues[i];
                int toolChangeValue = toolInfo.PinChangeValues[i];
                int resultPinValue = pinValue + toolChangeValue;
                if (resultPinValue < minPinValue || resultPinValue > maxPinValue)
                {
                    toolCanChangePins = false;
                    break;
                }
            }

            return toolCanChangePins;
        }
    }
}