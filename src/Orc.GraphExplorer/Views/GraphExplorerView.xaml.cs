#region Copyright (c) 2014 Orcomp development team.
// -------------------------------------------------------------------------------------------------------------------
// <copyright file="GraphExplorerView.xaml.cs" company="Orcomp development team">
//   Copyright (c) 2014 Orcomp development team. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------
#endregion

namespace Orc.GraphExplorer.Views
{
    using System;
    using System.Collections.Generic;
    using System.Windows;
    using System.Windows.Media;

    using Catel.IoC;

    using GraphX;
    using GraphX.Controls;

    using Orc.GraphExplorer.DomainModel;
    using Orc.GraphExplorer.Views.Enums;
    using Orc.GraphExplorer.Views.Interfaces;

    /// <summary>
    /// Interaction logic for GraphExplorerView.xaml
    /// </summary>
    public partial class GraphExplorerView : IGraphExplorerView
    {
        #region Fields
        private VertexControl _edVertex;

        private EdgeControl _edEdge;

        #endregion // Fields
        #region Constructors
        public GraphExplorerView()
        {
            InitializeComponent();

            ServiceLocator.Default.RegisterInstance(GetType(), this);            
        }

        public bool IsEdgeEditing {
            get
            {
                return _edEdge != null;
            }
        }

        public bool IsVertexEditing {
            get
            {
                return _edVertex != null;
            }
        }
        #endregion

        public void ShowAllEdgesLabels(GraphExplorerTab tab, bool show)
        {
            GraphArea area;
            switch (tab)
            {
                case GraphExplorerTab.Main:
                    area = Area;
                    break;
                case GraphExplorerTab.Navigation:
                    area = AreaNav;
                    break;
                default:
                    throw new NotImplementedException();
            }

            area.ShowAllEdgesLabels(show);
            area.InvalidateVisual();
        }

        public void FitToBounds(GraphExplorerTab tab)
        {
            ZoomControl zoom;
            switch (tab)
            {
                case GraphExplorerTab.Main:
                    zoom = zoomctrl;
                    break;
                case GraphExplorerTab.Navigation:
                    zoom = zoomctrlNav;
                    break;
                default:
                    throw new NotImplementedException();
            }

            zoom.ZoomToFill();
            zoom.Mode = ZoomControlModes.Custom;
        }


        public DataEdge GetEdEdge()
        {
            return _edEdge.Edge as DataEdge;
        }

        public void RemoveEdge(DataEdge edge)
        {
            Area.RemoveEdge(edge);
        }

        public void SetEdgePathManually(PathGeometry edGeo)
        {
            _edEdge.SetEdgePathManually(edGeo);
        }

        public DataVertex GetEdVertex()
        {
            return _edVertex.Vertex as DataVertex;
        }

        public PathGeometry CreatePathGeometry()
        {
            return new PathGeometry(new PathFigureCollection { new PathFigure { IsClosed = false, StartPoint = _edVertex.GetPosition(), Segments = new PathSegmentCollection { new PolyLineSegment(new List<Point> { new Point() }, true) } } });
        }

        public void SetEdVertex(VertexControl vertexControl)
        {
            _edVertex = vertexControl;
        }

        public void AddEdge(DataEdge dedge)
        {
            _edEdge = new EdgeControl(_edVertex, null, dedge) { ManualDrawing = true };
            Area.AddEdge(dedge, _edEdge);
        }

        public bool IsEdVertex(VertexControl vertexControl)
        {
            return _edVertex == vertexControl;
        }

        public void ClearEdEdge()
        {
            _edEdge = null;
        }

        public void ClearEdVertex()
        {
            _edVertex = null;
        }
    }
}