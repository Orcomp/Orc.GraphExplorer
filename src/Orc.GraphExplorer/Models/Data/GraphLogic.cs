#region Copyright (c) 2014 Orcomp development team.
// -------------------------------------------------------------------------------------------------------------------
// <copyright file="GraphLogic.cs" company="Orcomp development team">
//   Copyright (c) 2014 Orcomp development team. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------
#endregion

namespace Orc.GraphExplorer.Models.Data
{
    using System;
    using Catel;
    using Events;
    using GraphX.Logic;

    public class GraphLogic : GXLogicCore<DataVertex, DataEdge, Graph>
    {
        public GraphLogic()
        {
        }

        public void PrepareGraphReloading()
        {
            BeforeReloadingGraph.SafeInvoke(this);
        }

        public void ResumeGraphReloading(Graph graph)
        {
            GraphReloaded.SafeInvoke(this, new GraphEventArgs(graph));
        }

        public event EventHandler BeforeReloadingGraph;
        public event EventHandler<GraphEventArgs> GraphReloaded;
    }
}