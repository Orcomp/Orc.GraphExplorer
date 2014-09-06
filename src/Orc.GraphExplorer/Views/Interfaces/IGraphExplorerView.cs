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
    using DomainModel;
    using GraphX;
    using Orc.GraphExplorer.Views.Enums;

    public interface IGraphExplorerView : IView
    {
        void ShowAllEdgesLabels(GraphExplorerTab tab,bool show);

        void FitToBounds(GraphExplorerTab tab);
        bool IsVertexEditing { get; }
        DataEdge GetEdEdge();
        void RemoveEdge(DataEdge edge);
        void SetEdgePathManually(PathGeometry edGeo);
        DataVertex GetEdVertex();
        PathGeometry CreatePathGeometry();
        void SetEdVertex(VertexControl vertexControl);
        void AddEdge(DataEdge dedge);
        bool IsEdVertex(VertexControl vertexControl);
        void ClearEdEdge();
        void ClearEdVertex();
        void ClearLayout(GraphExplorerTab tab);
        DataVertex GetVertexById(GraphExplorerTab tab, int vertexId);
        void SetVertexHighlighted(GraphExplorerTab tab, DataVertex dataVertex, bool value);
        void SetVertexHighlighted(GraphExplorerTab tab, int vertexId, bool value);
    }
}