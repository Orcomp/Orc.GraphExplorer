#region Copyright (c) 2014 Orcomp development team.
// -------------------------------------------------------------------------------------------------------------------
// <copyright file="GraphAreaLoadingService.cs" company="Orcomp development team">
//   Copyright (c) 2014 Orcomp development team. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------
#endregion

namespace Orc.GraphExplorer.Services
{
    using System.Threading.Tasks;
    using Catel;
    using Catel.Memento;
    using Catel.Services;
    using Messages;
    using Models;
    using Models.Data;

    public class GraphAreaLoadingService : IGraphAreaLoadingService
    {
        #region Fields
        private readonly IMementoService _mementoService;

        private readonly IMessageService _messageService;
        #endregion

        #region Constructors
        public GraphAreaLoadingService(IMementoService mementoService, IMessageService messageService)
        {
            Argument.IsNotNull(() => mementoService);
            Argument.IsNotNull(() => messageService);

            _mementoService = mementoService;
            _messageService = messageService;
        }
        #endregion

        #region IGraphAreaLoadingService Members
        public void ReloadGraphArea(GraphArea graphArea, int offsetY)
        {
            Argument.IsNotNull(() => graphArea);

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
        #endregion
    }
}