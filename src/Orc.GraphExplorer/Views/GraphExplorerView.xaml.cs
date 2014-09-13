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
    using System.Linq;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Data;
    using System.Windows.Media;

    using Catel.IoC;
    using Catel.MVVM.Converters;
    using GraphX;
    using GraphX.Controls;

    using Orc.GraphExplorer.ObjectModel;
    using Orc.GraphExplorer.Views.Enums;
    using Orc.GraphExplorer.Views.Interfaces;

    using QuickGraph;
    using IValueConverter = Catel.MVVM.Converters.IValueConverter;

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

            RplaceAreaGridParent(Area, zoomctrl);
            RplaceAreaGridParent(AreaNav, zoomctrlNav);
        }

        // TODO: this method is like hack. It should be removed in future
        /// <summary>
        /// 
        /// </summary>
        /// <param name="areaView"></param>
        /// <param name="zoomView"></param>
        private void RplaceAreaGridParent(AreaView areaView, ZoomView zoomView)
        {
            var grid = areaView.Parent as Grid;
            if (grid == null)
            {
                return;
            }
            grid.Children.Remove(areaView);
            zoomView.Content = areaView;

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
            var area = GetAreaByTab(tab);
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

        public void ClearLayout(GraphExplorerTab tab)
        {
            var area = GetAreaByTab(tab);
            area.ClearLayout();
        }

        private GraphArea GetAreaByTab(GraphExplorerTab tab)
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
            return area;
        }

        public void SetHighlighted(GraphExplorerTab tab, DataVertex dataVertex, bool value)
        {
            var area = GetAreaByTab(tab);
            HighlightBehaviour.SetHighlighted(area.VertexList[dataVertex], value);
        }

        public void SetIsHighlightEnabled(GraphExplorerTab tab, DataVertex vertex, bool value)
        {
            var area = GetAreaByTab(tab);
            HighlightBehaviour.SetIsHighlightEnabled(area.VertexList[vertex], value);
        }

        public void SetIsHighlightEnabled(GraphExplorerTab tab, DataEdge edge, bool value)
        {
            var area = GetAreaByTab(tab);
            HighlightBehaviour.SetIsHighlightEnabled(area.EdgesList[edge], value);
        }

        public void SetHighlighted(GraphExplorerTab tab, DataEdge edge, bool value)
        {
            var area = GetAreaByTab(tab);
            HighlightBehaviour.SetHighlighted(area.EdgesList[edge], value);
        }

        public void SetIsDragEnabled(GraphExplorerTab tab, DataVertex vertex, bool value)
        {
            var area = GetAreaByTab(tab);
            DragBehaviour.SetIsDragEnabled(area.VertexList[vertex], value);
        }

        //Summary
        //    binding in style will be overrided in graph control, so need create binding after data loaded
        public void SetVertexPropertiesBinding(GraphExplorerTab tab)
        {
            GraphArea area = GetAreaByTab(tab);;
            IValueConverter conv = new BooleanToHidingVisibilityConverter();

            foreach (var vertex in area.VertexList)
            {
                var bindingIsVisible = new Binding("IsVisible") { Source = vertex.Key, Mode = BindingMode.TwoWay, Converter = conv };

                var bindingIsEnabled = new Binding("IsEnabled") { Source = vertex.Key, Mode = BindingMode.TwoWay };

                vertex.Value.SetBinding(UIElement.VisibilityProperty, bindingIsVisible);
                vertex.Value.SetBinding(UIElement.IsEnabledProperty, bindingIsEnabled);
            }

            foreach (var edge in area.EdgesList)
            {
                var bindingIsVisible = new Binding("IsVisible") { Source = edge.Key, Mode = BindingMode.TwoWay, Converter = conv };

                var bindingIsEnabled = new Binding("IsEnabled") { Source = edge.Key, Mode = BindingMode.TwoWay };

                edge.Value.SetBinding(UIElement.VisibilityProperty, bindingIsVisible);
                edge.Value.SetBinding(UIElement.IsEnabledProperty, bindingIsEnabled);
            }
        }
    }
}