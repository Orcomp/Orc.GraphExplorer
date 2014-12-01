#region Copyright (c) 2014 Orcomp development team.
// -------------------------------------------------------------------------------------------------------------------
// <copyright file="IEdgeDrawingService.cs" company="Orcomp development team">
//   Copyright (c) 2014 Orcomp development team. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------
#endregion
namespace Orc.GraphExplorer.Services
{
    using System.Windows;
    using System.Windows.Media;
    using Models;
    using Models.Data;

    public interface IEdgeDrawingService
    {
        bool IsInDrawing();
        void StartEdgeDrawing(Graph graph, DataVertex startVertex, Point startPoint, Point lastPoint);
        bool IsStartVertex(DataVertex dataVertex);
        void FinishEdgeDrawing(DataVertex endVertex);
        DataVertex GetStartVertex();
        void MoveBrush(Point point);
        PathGeometry GetEdgeGeometry();
    }
}