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
    using Catel.Services;
    using Catel.Windows.Interactivity;
    using GraphX.Controls;
    using GraphX.Models;
    using Models;

    using Orc.GraphExplorer.Messages;
    using Orc.GraphExplorer.ViewModels;

    using Views;
    using Views.Base;

    // TODO: This class need to be reviewed
    public class DrawEdgeBehavior : BehaviorBase<GraphAreaView>
    {
        public DrawEdgeBehavior()
        {
        }

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
                    _zoomControl = AssociatedObject.FindFirstParentOfType<ZoomControl>();
                }

                return _zoomControl;
            }
        }

        #endregion

        #region Methods
        protected override void OnAssociatedObjectLoaded()
        {
            base.OnAssociatedObjectLoaded();
            AssociatedObject.VertexSelected += AssociatedObject_VertexSelected;
            AssociatedObject.TemporaryEdgeCreated += AssociatedObject_TemporaryEdgeCreated;
            ZoomControl.PreviewMouseMove += ZoomControl_PreviewMouseMove;
        }

        private void ZoomControl_PreviewMouseMove(object sender, MouseEventArgs e)
        {
            var viewModel = AssociatedObject.ViewModel;
            if (viewModel == null || !viewModel.ToolSetViewModel.IsAddingNewEdge || _pathGeometry == null || _startVertex == null || _fakeEndVertex == null || _edge == null)
            {
                return;
            }
            SetLastPoint(e.GetPosition(ZoomControl));
            
            _edge.SetEdgePathManually(_pathGeometry);
        }

        private void AssociatedObject_TemporaryEdgeCreated(object sender, EdgeViewCreatedAventArgs e)
        {
            _edge = e.EdgeViewBase as EdgeView;
            if (_edge != null)
            {
                _edge.SetEdgePathManually(_pathGeometry);
            }
        }

        private void AssociatedObject_VertexSelected(object sender, VertexSelectedEventArgs args)
        {
            if (args.MouseArgs.LeftButton == MouseButtonState.Pressed)
            {
                var viewModel = AssociatedObject.ViewModel;
                if (viewModel.IsInEditing && viewModel.ToolSetViewModel.IsAddingNewEdge)
                {
                    var graph = viewModel.Logic.Graph;
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
                        graph.AddVertex(_fakeEndVertex);
                        graph.AddEdge(dedge);

                        StatusMessage.SendWith("Select Target Node");
                    }

                    else if (!Equals(_startVertex, args.VertexControl)) //finish draw
                    {
                        viewModel.Area.AddEdge(_startVertex.ViewModel.DataVertex, (DataVertex) args.VertexControl.Vertex);
                        graph.RemoveVertex(_fakeEndVertex);

                        _startVertex = null;
                        _fakeEndVertex = null;
                        _edge = null;
                        _pathGeometry.Clear();
                        _pathGeometry = null;

                        viewModel.ToolSetViewModel.IsAddingNewEdge = false;
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