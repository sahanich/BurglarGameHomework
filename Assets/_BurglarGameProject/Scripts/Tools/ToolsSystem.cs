using Assets._BurglarGameProject.Scripts.Settings;

namespace Assets._BurglarGameProject.Scripts.Tools
{
    public class ToolsSystem
    {
        private IToolsModel _toolsModel;
        private ToolsController _toolsController;

        public IToolsModel ToolsModel => _toolsModel;
        public ToolsController ToolsController => _toolsController;

        public ToolsSystem(IBurglarGameSettings gameSettings, IToolsView toolsView)
        {
            _toolsModel = new ToolsModel(gameSettings.Tools);
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