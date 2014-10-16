#region Copyright (c) 2014 Orcomp development team.
// -------------------------------------------------------------------------------------------------------------------
// <copyright file="DrawEdgeBehavior.cs" company="Orcomp development team">
//   Copyright (c) 2014 Orcomp development team. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------
#endregion

namespace Orc.GraphExplorer.Behaviors
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Windows;
    using System.Windows.Input;
    using System.Windows.Interactivity;
    using System.Windows.Media;
    using Catel.IoC;
    using Catel.MVVM.Views;
    using Events;
    using GraphX.Controls;
    using GraphX.Models;
    using Models;
    using Views;
    using Views.Base;

    // TODO: This behavior must be reviewed
    public class DrawEdgeBehavior : Behavior<GraphAreaView>
    {
        #region Fields
        private EdgeView _edge;

        private VertexView _startVertex;
        private DataVertex _fakeEndVertex;
        private PathGeometry _pathGeometry;

        private ZoomControl _zoomControl;
        #endregion

        #region Properties
        private ZoomControl ZoomControl
        {
            get
            {
                if (_zoomControl == null)
                {
                    var toolsetView = ServiceLocator.Default.ResolveType<IViewManager>().GetViewsOfViewModel(AssociatedObject.ViewModel.ToolSetViewModel).OfType<GraphToolsetView>().FirstOrDefault();
                    if (toolsetView != null)
                    {
                        _zoomControl = toolsetView.ZoomControl;
                    }
                }

                return _zoomControl;
            }
        }
        #endregion

        #region Methods
        protected override void OnAttached()
        {
            base.OnAttached();
            AssociatedObject.VertexSelected += AssociatedObject_VertexSelected;
            AssociatedObject.TemporaryEdgeCreated += AssociatedObject_TemporaryEdgeCreated;
            AssociatedObject.ViewModelChanged += AssociatedObject_ViewModelChanged;
        }

        private void AssociatedObject_ViewModelChanged(object sender, System.EventArgs e)
        {
            ZoomControl.PreviewMouseMove += ZoomControl_PreviewMouseMove;
        }

        private void ZoomControl_PreviewMouseMove(object sender, MouseEventArgs e)
        {
            if (AssociatedObject.ViewModel == null || !AssociatedObject.ViewModel.ToolSetViewModel.IsAddingNewEdge || _pathGeometry == null || _startVertex == null || _fakeEndVertex == null || _edge == null)
            {
                return;
            }
            SetLastPoint(e.GetPosition(ZoomControl));
            
            _edge.SetEdgePathManually(_pathGeometry);
        }

        private void AssociatedObject_TemporaryEdgeCreated(object sender, EdgeViewCreatedAventArgs e)
        {
            _edge = e.EdgeViewBase as EdgeView;
            _edge.SetEdgePathManually(_pathGeometry);
        }

        private void AssociatedObject_VertexSelected(object sender, VertexSelectedEventArgs args)
        {
            if (args.MouseArgs.LeftButton == MouseButtonState.Pressed)
            {
                if (AssociatedObject.ViewModel.IsInEditing && AssociatedObject.ViewModel.ToolSetViewModel.IsAddingNewEdge)
                {
                    if (_startVertex == null) //select starting vertex
                    {
                        _startVertex = (VertexView) args.VertexControl;
                        _fakeEndVertex = DataVertex.CreateFakeVertex();
                        var pathFigureCollection = new PathFigureCollection
                        {
                            new PathFigure
                            {
                                IsClosed = false, StartPoint = _startVertex.GetPosition(), Segments = new PathSegmentCollection
                                {
                                    new PolyLineSegment(new List<Point> {new Point()}, true)
                                }
                            }
                        };
                        _pathGeometry = new PathGeometry(pathFigureCollection);

                        SetLastPoint(args.VertexControl.GetPosition());

                        var dedge = new DataEdge(_startVertex.ViewModel.DataVertex, _fakeEndVertex);
                        AssociatedObject.ViewModel.Logic.Graph.AddVertex(_fakeEndVertex);
                        AssociatedObject.ViewModel.Logic.Graph.AddEdge(dedge);
/*

                        GraphExplorerViewModel.Status = GraphExplorerStatus.CreateLinkSelectTarget;
                        GraphExplorerViewModel.PostStatusMessage("Select Target Node");
*/
                    }

                    else if (!Equals(_startVertex, args.VertexControl)) //finish draw
                    {
                        AssociatedObject.ViewModel.Area.AddEdge(_startVertex.ViewModel.DataVertex, (DataVertex) args.VertexControl.Vertex);
                        AssociatedObject.ViewModel.Logic.Graph.RemoveVertex(_fakeEndVertex);

                        _startVertex = null;
                        _fakeEndVertex = null;
                        _edge = null;
                        _pathGeometry.Clear();
                        _pathGeometry = null;

                        AssociatedObject.ViewModel.ToolSetViewModel.IsAddingNewEdge = false;
                    }
                }
            }
        }

        private void SetLastPoint(Point point)
        {
            Point pos = ZoomControl.TranslatePoint(point, AssociatedObject);
            var lastseg = _pathGeometry.Figures[0].Segments[_pathGeometry.Figures[0].Segments.Count - 1] as PolyLineSegment;
            lastseg.Points[lastseg.Points.Count - 1] = pos;
        }
        #endregion
    }
}