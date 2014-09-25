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

    public interface IGraphDataService
    {
        IObservable<DataVertex> GetVerteces();

        IObservable<DataEdge> GetEdges();
    }
}