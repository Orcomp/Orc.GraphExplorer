namespace Orc.GraphExplorer.Models.Data
{
    using System;

    using Orc.GraphExplorer.Services.Interfaces;

    using QuickGraph;

    public class Graph : BidirectionalGraph<DataVertex, DataEdge>
    {
        private readonly IGraphDataService _graphDataService;

        public Graph()
        {
            
        }

        public Graph(IGraphDataService graphDataService)
        {
            _graphDataService = graphDataService;
        }

        public void ReloadGraph()
        {
            RemoveEdgeIf(e => true);
            RemoveVertexIf(v => true);

            AddVertexRange(_graphDataService.GetVerteces());
            AddEdgeRange(_graphDataService.GetEdges());
        }
    }
}
