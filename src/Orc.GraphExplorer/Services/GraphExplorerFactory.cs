namespace Orc.GraphExplorer.Services
{
    using Catel;
    using Catel.Memento;
    using Catel.Services;
    using Models;

    using Orc.GraphExplorer.Messages;

    public class GraphExplorerFactory : IGraphExplorerFactory
    {
        private readonly IDataLocationSettingsService _dataLocationSettingsService;

        public GraphExplorerFactory(IDataLocationSettingsService dataLocationSettingsService)
        {
            Argument.IsNotNull(() => dataLocationSettingsService);

            _dataLocationSettingsService = dataLocationSettingsService;
        }

        public Explorer CreateExplorer()
        {
            var explorer = new Explorer();
            explorer.EditorToolset = new GraphToolset("Editor", true);
            explorer.NavigatorToolset = new GraphToolset("Navigator", false);

            explorer.Settings = new Settings();

            explorer.Settings.DataLocationSettings = _dataLocationSettingsService.Load();
            SettingsChangedMessage.SendWith(true);

            return explorer;
        }
    }
}