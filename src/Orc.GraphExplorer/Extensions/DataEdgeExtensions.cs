#region Copyright (c) 2014 Orcomp development team.
// -------------------------------------------------------------------------------------------------------------------
// <copyright file="DataEdgeExtensions.cs" company="Orcomp development team">
//   Copyright (c) 2014 Orcomp development team. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------
#endregion
namespace Orc.GraphExplorer
{
    using Orc.GraphExplorer.Models;

    public static class DataEdgeExtensions
    {
        public static bool IsFiltered(this DataEdge dataEdge)
        {
            var source = dataEdge.Source;
            var target = dataEdge.Target;

            return source != null && target != null && (source.IsFiltered && target.IsFiltered);
        }
    }
}