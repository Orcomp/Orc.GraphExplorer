// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DataVertexExtensions.cs" company="Orcomp development team">
//   Copyright (c) 2008 - 2014 Orcomp development team. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------
namespace Orc.GraphExplorer.Extensions
{
    using Models;

    public static class DataVertexExtensions
    {
        public static bool IsFakeVertex(this DataVertex vertex)
        {
            return vertex.ID == DataVertex.FakeVertexId;
        }
    }
}