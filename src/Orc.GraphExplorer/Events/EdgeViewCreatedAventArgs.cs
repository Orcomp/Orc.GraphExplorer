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
    using Views.Base;

    public class EdgeViewCreatedAventArgs : EventArgs
    {
        #region Constructors
        public EdgeViewCreatedAventArgs(EdgeViewBase edgeViewBase)
        {
            EdgeViewBase = edgeViewBase;
        }
        #endregion

        #region Properties
        public EdgeViewBase EdgeViewBase { get; private set; }
        #endregion
    }
}