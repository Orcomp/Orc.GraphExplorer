#region Copyright (c) 2014 Orcomp development team.
// -------------------------------------------------------------------------------------------------------------------
// <copyright file="IGraphLogicProvider.cs" company="Orcomp development team">
//   Copyright (c) 2014 Orcomp development team. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------
#endregion
namespace Orc.GraphExplorer
{
    using Orc.GraphExplorer.Models.Data;

    public interface IGraphLogicProvider
    {
        GraphLogic Logic { get; }
    }
}