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
    using Models;
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

        #endregion // Fields
        #region Constructors
        public GraphExplorerView()
        {
            InitializeComponent();

            ServiceLocator.Default.RegisterInstance(GetType(), this);

            RplaceAreaGridParent(Area, ZoomCtrl);
            RplaceAreaGridParent(AreaNav, ZoomCtrlNav);
        }

        // TODO: this method is like hack. It should be removed in future
        /// <summary>
        /// 
        /// </summary>
        /// <param name="graphArea></param>
        /// <param name="zoomView"></param>
        private void RplaceAreaGridParent(GraphArea graphArea, ZoomView zoomView)
        {
            var grid = graphArea.Parent as Grid;
            if (grid == null)
            {
                return;
            }
            grid.Children.Remove(graphArea);
            zoomView.Content = graphArea;

        }        

        public bool IsVertexEditing {
            get
            {
                return _edVertex != null;
            }
        }
        #endregion

       

        

        public void RemoveEdge(DataEdge edge)
        {
            Area.RemoveEdge(edge);
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

        /*public void AddEdge(DataEdge dedge)
        {
            _edEdge = new EdgeControl(_edVertex, null, dedge) { ManualDrawing = true };
            Area.AddEdge(dedge, _edEdge);
        }*/

        public bool IsEdVertex(VertexControl vertexControl)
        {
            return _edVertex == vertexControl;
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
            vertex.IsHighlightEnabled = value;
        }

        public void SetIsHighlightEnabled(GraphExplorerTab tab, DataEdge edge, bool value)
        {
            edge.IsHighlightEnabled = value;
        }

        public void SetHighlighted(GraphExplorerTab tab, DataEdge edge, bool value)
        {
            var area = GetAreaByTab(tab);
            HighlightBehaviour.SetHighlighted(area.EdgesList[edge], value);
        }

        public void SetIsDragEnabled(GraphExplorerTab tab, DataVertex vertex, bool value)
        {
            vertex.IsDragEnabled = value;
        }
    }
}