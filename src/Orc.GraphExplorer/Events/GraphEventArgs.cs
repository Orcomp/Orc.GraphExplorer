// --------------------------------------------------------------------------------------------------------------------
// <copyright file="GraphEventArgs.cs" company="Orcomp development team">
//   Copyright (c) 2008 - 2014 Orcomp development team. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace Orc.GraphExplorer
{
    using System;
    using Catel;
    using Models.Data;

    public class GraphEventArgs : EventArgs
    {
        #region Constructors
        public GraphEventArgs(Graph graph)
        {
            Argument.IsNotNull(() => graph);

            Graph = graph;
        }
        #endregion

        #region Properties
        public Graph Graph { get; private set; }
        #endregion
    }
}