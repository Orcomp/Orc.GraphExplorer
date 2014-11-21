#region Copyright (c) 2014 Orcomp development team.
// -------------------------------------------------------------------------------------------------------------------
// <copyright file="NavigatorGraphDataGetter.cs" company="Orcomp development team">
//   Copyright (c) 2014 Orcomp development team. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------
#endregion
namespace Orc.GraphExplorer.Services
{
    using System.Collections.Generic;
    using System.Linq;
    using Behaviors;

    using Catel;

    using GraphX.GraphSharp;
    using Models;
    using Models.Data;

    public class NavigatorGraphDataGetter : IGraphDataGetter, IGraphNavigator
    {
        private readonly Graph _graph;

        public NavigatorGraphDataGetter()
        {
            
        }

        public NavigatorGraphDataGetter(Graph graph)
        {
            Argument.IsNotNull(() => graph);

            _graph = graph;
        }

        public IEnumerable<DataVertex> GetVerteces()
        {
            return _vertices;
        }

        public IEnumerable<DataEdge> GetEdges()
        {
            return _edges;
        }

        private IEnumerable<DataEdge> _edges = Enumerable.Empty<DataEdge>();
        private IEnumerable<DataVertex> _vertices = Enumerable.Empty<DataVertex>();

        public void NavigateTo(DataVertex dataVertex)
        {
            Argument.IsNotNull(() => dataVertex);

            IEnumerable<DataEdge> inEdges;
            IEnumerable<DataEdge> outEdges;

            if (!_graph.TryGetInEdges(dataVertex, out inEdges) || !_graph.TryGetOutEdges(dataVertex, out outEdges))
            {
                return;
            }

            _edges = inEdges.Concat(outEdges);

            _vertices = _graph.GetNeighbours(dataVertex).Concat(Enumerable.Repeat(dataVertex, 1));
        }

    }
}