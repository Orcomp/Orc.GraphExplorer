namespace Orc.GraphExplorer.Services
{
    using System.Threading.Tasks;

    using Catel.Memento;
    using Catel.Services;

    using Orc.GraphExplorer.Messages;
    using Orc.GraphExplorer.Models;
    using Orc.GraphExplorer.Models.Data;

    public class GraphAreaLoadingService  : IGraphAreaLoadingService
    {
        private readonly IMementoService _mementoService;

        private readonly IMessageService _messageService;


        public GraphAreaLoadingService(IMementoService mementoService, IMessageService messageService)
        {
            _mementoService = mementoService;
            _messageService = messageService;
        }

        public void ReloadGraphArea(GraphArea graphArea, int offsetY)
        {
            if (graphArea.GraphDataGetter == null)
            {
                return;
            }
            var logic = graphArea.Logic;

            logic.PrepareGraphReloading();

            var graph = new Graph(graphArea.GraphDataGetter);

            graph.ReloadGraph();
            logic.ExternalLayoutAlgorithm = new TopologicalLayoutAlgorithm<DataVertex, DataEdge, Graph>(graph, 1.5, offsetY: offsetY);

            logic.ResumeGraphReloading(graph);
        }

        public async Task<bool> TryRefresh(GraphArea area)
        {
            if (area.IsInEditing && _mementoService.CanUndo)
            {
                var messageResult = await _messageService.Show("Refresh view in edit mode will discard changes you made, will you want to continue?", "Confirmation", MessageButton.YesNo);
                if (messageResult != MessageResult.Yes)
                {
                    return false;
                }

                _mementoService.Clear();
                area.IsInEditing = false;
                area.IsDragEnabled = false;
                ReloadGraphArea(area, 600);
                StatusMessage.SendWith("Graph Refreshed");
            }
            else
            {
                ReloadGraphArea(area, 600);
            }

            return true;
        }
    }
}