// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DataEdgeExtensions.cs" company="Orcomp development team">
//   Copyright (c) 2008 - 2014 Orcomp development team. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace Orc.GraphExplorer
{
    using Models;

    public static class DataEdgeExtensions
    {
        #region Methods
        public static bool IsFiltered(this DataEdge dataEdge)
        {
            var source = dataEdge.Source;
            var target = dataEdge.Target;

            return source != null && target != null && (source.IsFiltered && target.IsFiltered);
        }
        #endregion
    }
}