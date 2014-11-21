namespace Orc.GraphExplorer.Models.Data
{
    using System;

    using Catel;

    using QuickGraph;
    using Services;

    public class Graph : BidirectionalGraph<DataVertex, DataEdge>
    {
        private readonly IGraphDataGetter _graphDataGetter;

        public Graph()
        {
            
        }

        public Graph(IGraphDataGetter graphDataGetter)
        {
            Argument.IsNotNull(() => graphDataGetter);

            _graphDataGetter = graphDataGetter;
        }

        public void ReloadGraph()
        {
            RemoveEdgeIf(e => true);
            RemoveVertexIf(v => true);

            AddVertexRange(_graphDataGetter.GetVerteces());
            AddEdgeRange(_graphDataGetter.GetEdges());
        }
    }
}
