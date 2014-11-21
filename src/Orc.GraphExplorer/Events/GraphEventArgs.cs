#region Copyright (c) 2014 Orcomp development team.
// -------------------------------------------------------------------------------------------------------------------
// <copyright file="GraphEventArgs.cs" company="Orcomp development team">
//   Copyright (c) 2014 Orcomp development team. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------
#endregion
namespace Orc.GraphExplorer
{
    using System;

    using Catel;

    using Models.Data;

    public class GraphEventArgs : EventArgs
    {
        public Graph Graph { get; set; }

        public GraphEventArgs(Graph graph)
        {
            Argument.IsNotNull(() => graph);
 
            Graph = graph;
        }
    }
}