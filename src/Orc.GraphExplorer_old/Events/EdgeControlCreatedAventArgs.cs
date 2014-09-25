#region Copyright (c) 2014 Orcomp development team.
// -------------------------------------------------------------------------------------------------------------------
// <copyright file="EdgeControlCreatedAventArgs.cs" company="Orcomp development team">
//   Copyright (c) 2014 Orcomp development team. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------
#endregion

namespace Orc.GraphExplorer.Events
{
    using System;
    using GraphX;

    public class EdgeControlCreatedAventArgs : EventArgs
    {
        #region Constructors
        public EdgeControlCreatedAventArgs(EdgeControl edgeControl)
        {
            EdgeControl = edgeControl;
        }
        #endregion

        #region Properties
        public EdgeControl EdgeControl { get; private set; }
        #endregion
    }
}