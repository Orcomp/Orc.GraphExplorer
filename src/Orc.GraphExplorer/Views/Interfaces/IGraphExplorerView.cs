#region Copyright (c) 2014 Orcomp development team.
// -------------------------------------------------------------------------------------------------------------------
// <copyright file="IGraphExplorerView.cs" company="Orcomp development team">
//   Copyright (c) 2014 Orcomp development team. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------
#endregion
namespace Orc.GraphExplorer.Views.Interfaces
{
    using System.Windows.Media;
    using Catel.MVVM.Views;

    using GraphX;

    using Orc.GraphExplorer.ObjectModel;
    using Orc.GraphExplorer.Views.Enums;

    public interface IGraphExplorerView : IView
    {
        bool IsVertexEditing { get; }
        void RemoveEdge(DataEdge edge);
        DataVertex GetEdVertex();
        PathGeometry CreatePathGeometry();
        void SetEdVertex(VertexControl vertexControl);
        bool IsEdVertex(VertexControl vertexControl);
        void ClearEdVertex();
        void ClearLayout(GraphExplorerTab tab);
        void SetHighlighted(GraphExplorerTab tab, DataVertex dataVertex, bool value);
    }
}