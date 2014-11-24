namespace Orc.GraphExplorer.Services
{
    using Catel.Memento;
    using Catel.Services;
    using Models;

    public class GraphExplorerFactory : IGraphExplorerFactory
    {
        private readonly IMementoService _mementoService;
        private readonly IMessageService _messageService;
        private readonly IConfigLocationService _configLocationService;

        public GraphExplorerFactory(IMementoService mementoService, IMessageService messageService, IConfigLocationService configLocationService)
        {
            _mementoService = mementoService;
            _messageService = messageService;
            _configLocationService = configLocationService;
        }

        public Explorer CreateExplorer()
        {
            var explorer = new Explorer();
            explorer.EditorToolset = new GraphToolset("Editor", true, _mementoService, _messageService);
            explorer.NavigatorToolset = new GraphToolset("Navigator", false, _mementoService, _messageService);

            explorer.Settings = new Settings(_configLocationService);

            return explorer;
        }
    }
}