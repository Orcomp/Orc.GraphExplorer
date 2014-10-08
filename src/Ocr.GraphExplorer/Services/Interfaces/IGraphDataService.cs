#region Copyright (c) 2014 Orcomp development team.
// -------------------------------------------------------------------------------------------------------------------
// <copyright file="IGraphDataService.cs" company="Orcomp development team">
//   Copyright (c) 2014 Orcomp development team. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------
#endregion
namespace Orc.GraphExplorer.Services.Interfaces
{
    using System;
    using System.Collections.Generic;

    using Orc.GraphExplorer.Models;
    using Orc.GraphExplorer.Models.Data;

    public interface IGraphDataService
    {
        IEnumerable<DataVertex> GetVerteces();

        IEnumerable<DataEdge> GetEdges();

        void SaveChanges(Graph graph);
    }
}