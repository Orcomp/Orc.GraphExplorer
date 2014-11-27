#region Copyright (c) 2014 Orcomp development team.
// -------------------------------------------------------------------------------------------------------------------
// <copyright file="TopologicalLayoutAlgorithm.cs" company="Orcomp development team">
//   Copyright (c) 2014 Orcomp development team. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------
#endregion

namespace Orc.GraphExplorer.Services
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Windows;
    using Catel;
    using GraphX.GraphSharp.Algorithms.Layout;
    using GraphX.GraphSharp.Algorithms.Layout.Simple.Hierarchical;
    using QuickGraph;

    public class TopologicalLayoutAlgorithm<TVertex, TEdge, TGraph> : IExternalLayout<TVertex>
        where TVertex : class
        where TEdge : IEdge<TVertex>
        where TGraph : IVertexAndEdgeListGraph<TVertex, TEdge>
    {
        #region Fields
        private readonly TGraph _graph;

        private readonly double _offsetX;
        private double _rate;
        private double _offsetY;
        private IDictionary<TVertex, Point> vertexPositions;
        #endregion

        #region Constructors
        public TopologicalLayoutAlgorithm(TGraph graph, double rate, double offsetX = 0, double offsetY = 600)
        {
            Argument.IsNotNull(() => graph);

            _graph = graph;
            _rate = rate;
            _offsetX = offsetX;
            _offsetY = offsetY;
        }
        #endregion

        #region IExternalLayout<TVertex> Members
        public void Compute()
        {
            var eslaParameters = new EfficientSugiyamaLayoutParameters()
            {
                MinimizeEdgeLength = true,
                LayerDistance = 80
            };

            var esla = new EfficientSugiyamaLayoutAlgorithm<TVertex, TEdge, TGraph>(_graph, eslaParameters, null, VertexSizes);

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
                vertexPositions.Add(item.Key, new Point(item.Value.Y*1.5 + _offsetX, item.Value.X + _offsetY));
            }
        }

        public bool NeedVertexSizes
        {
            get { return true; }
        }

        public IDictionary<TVertex, Point> VertexPositions
        {
            get { return vertexPositions; }
        }

        public IDictionary<TVertex, Size> VertexSizes { get; set; }
        #endregion
    }
}