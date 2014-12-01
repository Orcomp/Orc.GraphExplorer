// --------------------------------------------------------------------------------------------------------------------
// <copyright file="EdgeViewCreatedEventArgs.cs" company="Orcomp development team">
//   Copyright (c) 2008 - 2014 Orcomp development team. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace Orc.GraphExplorer
{
    using System;
    using Catel;
    using Views.Base;

    public class EdgeViewCreatedEventArgs : EventArgs
    {
        #region Constructors
        public EdgeViewCreatedEventArgs(EdgeViewBase edgeViewBase)
        {
            Argument.IsNotNull(() => edgeViewBase);

            EdgeViewBase = edgeViewBase;
        }
        #endregion

        #region Properties
        public EdgeViewBase EdgeViewBase { get; private set; }
        #endregion
    }
}