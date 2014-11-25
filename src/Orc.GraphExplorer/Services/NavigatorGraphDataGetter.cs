#region Copyright (c) 2014 Orcomp development team.
// -------------------------------------------------------------------------------------------------------------------
// <copyright file="NavigatorGraphDataGetter.cs" company="Orcomp development team">
//   Copyright (c) 2014 Orcomp development team. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------
#endregion
namespace Orc.GraphExplorer.Services
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Behaviors;

    using Catel;

    using GraphX.GraphSharp;
    using Models;
    using Models.Data;

    public class NavigatorGraphDataGetter : IGraphDataGetter, IOverridableGraphDataGetter
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
            return _vertecesGetter();
        }

        public IEnumerable<DataEdge> GetEdges()
        {
            return _edgesGetter();
        }

        private Func<IEnumerable<DataVertex>> _vertecesGetter;
        private Func<IEnumerable<DataEdge>> _edgesGetter;

        public void RedefineVertecesGetter(Func<IEnumerable<DataVertex>> getter)
        {
            _vertecesGetter = getter;
        }
        
        public void RedefineEdgesGetter(Func<IEnumerable<DataEdge>> getter)
        {
            _edgesGetter = getter;
        }
    }
}