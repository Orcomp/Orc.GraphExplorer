#region Copyright (c) 2014 Orcomp development team.
// -------------------------------------------------------------------------------------------------------------------
// <copyright file="IEdgeDrawer.cs" company="Orcomp development team">
//   Copyright (c) 2014 Orcomp development team. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------
#endregion

namespace Orc.GraphExplorer
{
    using System.Windows;
    using System.Windows.Media;
    using Models;

    public interface IEdgeDrawer
    {
        #region Methods
        bool TryStartEdgeDrawing(DataVertex startVertex, Point startPoint, Point lastPoint);
        bool TryFinishEdgeDrawing(DataVertex endVertex);
        void MoveBrush(Point point);
        PathGeometry GetEdgeGeometry();
        #endregion
    }
}