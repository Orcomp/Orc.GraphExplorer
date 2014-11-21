#region Copyright (c) 2014 Orcomp development team.
// -------------------------------------------------------------------------------------------------------------------
// <copyright file="EdgeViewCreatedEventArgs.cs" company="Orcomp development team">
//   Copyright (c) 2014 Orcomp development team. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------
#endregion

namespace Orc.GraphExplorer
{
    using System;
    using GraphX;

    using Orc.GraphExplorer.Views;
    using Views.Base;

    public class EdgeViewCreatedEventArgs : EventArgs
    {
        #region Constructors
        public EdgeViewCreatedEventArgs(EdgeViewBase edgeViewBase)
        {
            EdgeViewBase = edgeViewBase;
        }
        #endregion

        #region Properties
        public EdgeViewBase EdgeViewBase { get; private set; }
        #endregion
    }
}