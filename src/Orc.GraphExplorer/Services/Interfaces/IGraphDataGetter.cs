#region Copyright (c) 2014 Orcomp development team.
// -------------------------------------------------------------------------------------------------------------------
// <copyright file="IGraphDataGetter.cs" company="Orcomp development team">
//   Copyright (c) 2014 Orcomp development team. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------
#endregion
namespace Orc.GraphExplorer.Services
{
    using System;
    using System.Collections.Generic;

    using Orc.GraphExplorer.Models;
    using Orc.GraphExplorer.Models.Data;

    public interface IGraphDataGetter
    {
        IEnumerable<DataVertex> GetVerteces();

        IEnumerable<DataEdge> GetEdges();        
    }
}