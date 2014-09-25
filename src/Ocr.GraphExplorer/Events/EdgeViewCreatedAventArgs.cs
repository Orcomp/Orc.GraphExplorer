#region Copyright (c) 2014 Orcomp development team.
// -------------------------------------------------------------------------------------------------------------------
// <copyright file="EdgeViewCreatedAventArgs.cs" company="Orcomp development team">
//   Copyright (c) 2014 Orcomp development team. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------
#endregion

namespace Orc.GraphExplorer.Events
{
    using System;
    using GraphX;

    using Orc.GraphExplorer.Views;

    public class EdgeViewCreatedAventArgs : EventArgs
    {
        #region Constructors
        public EdgeViewCreatedAventArgs(EdgeView edgeView)
        {
            EdgeView = edgeView;
        }
        #endregion

        #region Properties
        public EdgeView EdgeView { get; private set; }
        #endregion
    }
}