#region Copyright (c) 2014 Orcomp development team.
// -------------------------------------------------------------------------------------------------------------------
// <copyright file="IGraphDataFactory.cs" company="Orcomp development team">
//   Copyright (c) 2014 Orcomp development team. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------
#endregion
namespace Orc.GraphExplorer.Services
{
    using Models;

    public interface IDataVertexFactory
    {
        DataVertex CreateFakeVertex();
        DataVertex CreateVertex();
        DataVertex CreateVertex(int id);
        bool IsFakeVertex(DataVertex vertex);
    }
}