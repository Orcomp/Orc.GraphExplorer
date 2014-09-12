namespace Orc.GraphExplorer.Common
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Windows;

    using GraphX.GraphSharp.Algorithms.Layout;
    using GraphX.GraphSharp.Algorithms.Layout.Simple.Hierarchical;

    public class TopologicalLayoutAlgorithm<TVertex, TEdge, TGraph> : IExternalLayout<TVertex>
        where TVertex : class
        where TEdge : global::QuickGraph.IEdge<TVertex>
        where TGraph : global::QuickGraph.IVertexAndEdgeListGraph<TVertex, TEdge>
    {
        TGraph _graph;
        double _rate;
        double _offsetX;
        double _offsetY;
        public TopologicalLayoutAlgorithm(TGraph graph,double rate,double offsetX = 0,double offsetY = 600)
        {
            _graph = graph;
            _rate = rate;
            _offsetX = offsetX;
            _offsetY = offsetY;
        }

        public void Compute()
        {
            var eslaParameters = new EfficientSugiyamaLayoutParameters()
            {
                MinimizeEdgeLength = true,
                LayerDistance = 80
            };

            var esla = new EfficientSugiyamaLayoutAlgorithm<TVertex, TEdge, TGraph>(_graph, eslaParameters,null, VertexSizes);

            esla.Compute();

            vertexPositions = new Dictionary<TVertex, Point>();

            double offsetY = esla.VertexPositions.Values.Min(p => p.X);
            if (offsetY < 0)
            {
                _offsetY = -offsetY;
            }
            //vertexPositions = esla.VertexPositions;
            foreach (var item in esla.VertexPositions)
            {
                vertexPositions.Add(item.Key, new Point(item.Value.Y * 1.5 +this._offsetX, item.Value.X + this._offsetY));
            }
        }

        public bool NeedVertexSizes
        {
            get { return true; }
        }

        IDictionary<TVertex, Point> vertexPositions;
        public IDictionary<TVertex, Point> VertexPositions
        {
            get { return vertexPositions; }
        }

        IDictionary<TVertex, Size> vertexSizes;
        public IDictionary<TVertex, Size> VertexSizes
        {
            get
            {
                return vertexSizes;
            }
            set
            {
                vertexSizes = value;
            }
        }
    }
}
