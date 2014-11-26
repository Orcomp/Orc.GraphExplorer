namespace Orc.GraphExplorer.Services
{
    using Catel.Memento;
    using Catel.Services;
    using Models;

    using Orc.GraphExplorer.Messages;

    public class GraphExplorerFactory : IGraphExplorerFactory
    {
        private readonly IConfigLocationService _configLocationService;

        public GraphExplorerFactory(IConfigLocationService configLocationService)
        {
            _configLocationService = configLocationService;
        }

        public Explorer CreateExplorer()
        {
            var explorer = new Explorer();
            explorer.EditorToolset = new GraphToolset("Editor", true);
            explorer.NavigatorToolset = new GraphToolset("Navigator", false);

            explorer.Settings = new Settings();

            explorer.Settings.ConfigLocation = _configLocationService.Load();
            SettingsChangedMessage.SendWith(true);

            return explorer;
        }
    }
}