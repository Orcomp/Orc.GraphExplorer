#region Copyright (c) 2014 Orcomp development team.
// -------------------------------------------------------------------------------------------------------------------
// <copyright file="IGraphAreaEditorService.cs" company="Orcomp development team">
//   Copyright (c) 2014 Orcomp development team. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------
#endregion
namespace Orc.GraphExplorer.Services
{
    using System.Threading.Tasks;
    using System.Windows;
    using Models;

    public interface IGraphAreaEditorService
    {
        void SaveChanges(GraphArea area);
        void AddVertex(GraphArea area, DataVertex dataVertex, Point point);
        void AddEdge(GraphArea area, DataVertex startVertex, DataVertex endVertex);
        void RemoveEdge(GraphArea area, DataEdge edge);
        void RemoveVertex(GraphArea area, DataVertex vertex);
        Task<bool> TryExitEditMode  (GraphArea area);
    }
}