using _BurglarGameProject.Mechanics.Tools.Scripts;
using _BurglarGameProject.Scripts.Settings;

namespace _BurglarGameProject.Scripts.Gameplay
{
    public class ToolsSystem
    {
        private IToolsModel _toolsModel;
        private ToolsController _toolsController;

        public IToolsModel ToolsModel => _toolsModel;
        public ToolsController ToolsController => _toolsController;

        public ToolsSystem(IBurglarGameSettings burglarGameSettings, IToolsView toolsView)
        {
            _toolsModel = new ToolsModel(burglarGameSettings.Tools);
            _toolsController = new ConstrainedToolsController(_toolsModel, toolsView);
        }

        public void RegisterListeners()
        {
            UnregisterListeners();
            _toolsController?.RegisterListeners();
        }

        public void UnregisterListeners()
        {
            _toolsController?.UnregisterListeners();
        }
    }
}