namespace Orc.GraphExplorer.Services
{
    using System.Collections.Generic;
    using System.Linq;
    using Catel;
    using Catel.IoC;

    using GraphX.GraphSharp;

    using Orc.GraphExplorer.Behaviors;
    using Orc.GraphExplorer.Models;

    public class NavigationService : INavigationService
    {
        public void NavigateTo(Explorer explorer, DataVertex dataVertex)
        {
            Argument.IsNotNull(() => explorer);
            Argument.IsNotNull(() => dataVertex);

            explorer.IsNavTabVisible = true;
            explorer.IsNavTabSelected = true;

            var loadingService = this.GetServiceLocator().ResolveType<IGraphAreaLoadingService>();

            var navigatorArea = explorer.NavigatorToolset.Area;
            
            IEnumerable<DataEdge> inEdges;
            IEnumerable<DataEdge> outEdges;

            var graphDataGetter = navigatorArea.GraphDataGetter as IOverridableGraphDataGetter;
            if (graphDataGetter == null)
            {
                return;
            }
            var graph = explorer.EditorToolset.Area.Logic.Graph;

            if (!graph.TryGetInEdges(dataVertex, out inEdges) || !graph.TryGetOutEdges(dataVertex, out outEdges))
            {
                return;
            }

            var edges = inEdges.Concat(outEdges);

            var vertices = graph.GetNeighbours(dataVertex).Concat(Enumerable.Repeat(dataVertex, 1));
            
            graphDataGetter.RedefineEdgesGetter(() => edges);
            graphDataGetter.RedefineVertecesGetter(() => vertices);

            loadingService.ReloadGraphArea(navigatorArea, 0);

            loadingService.TryRefresh(navigatorArea);
        }
    }
}