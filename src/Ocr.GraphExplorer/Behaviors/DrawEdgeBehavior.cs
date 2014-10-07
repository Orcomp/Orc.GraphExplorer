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
    using Enums;
    using Events;
    using GraphX;
    using GraphX.Controls;
    using GraphX.Models;
    using Models;
    using Views;

    public class DrawEdgeBehavior : Behavior<GraphAreaView>
    {
        #region Methods
        protected override void OnAttached()
        {
            base.OnAttached();
            AssociatedObject.VertexSelected += AssociatedObject_VertexSelected;
            AssociatedObject.TemporaryEdgeCreated += AssociatedObject_TemporaryEdgeCreated;
            AssociatedObject.ViewModelChanged += AssociatedObject_ViewModelChanged;
        }

        void AssociatedObject_ViewModelChanged(object sender, System.EventArgs e)
        {
            ZoomControl.PreviewMouseMove += ZoomControl_PreviewMouseMove;
        }

        void ZoomControl_PreviewMouseMove(object sender, MouseEventArgs e)
        {
            if (!AssociatedObject.ViewModel.ToolSetViewModel.IsAddingNewEdge || _pathGeometry == null || _startVertex == null || _endVertex == null || _edge == null)
            {
                return;
            }
            var pos = ZoomControl.TranslatePoint(e.GetPosition(ZoomControl), AssociatedObject);
            var lastseg = _pathGeometry.Figures[0].Segments[_pathGeometry.Figures[0].Segments.Count - 1] as PolyLineSegment;
            lastseg.Points[lastseg.Points.Count - 1] = pos;
            _edge.SetEdgePathManually(_pathGeometry);
        }

        private EdgeView _edge;

        private void AssociatedObject_TemporaryEdgeCreated(object sender, EdgeViewCreatedAventArgs e)
        {
            _edge = e.EdgeViewBase as EdgeView;
            _edge.SetEdgePathManually(_pathGeometry);
        }

        private VertexView _startVertex;
        private DataVertex _endVertex;
        private PathGeometry _pathGeometry;

        private void AssociatedObject_VertexSelected(object sender, VertexSelectedEventArgs args)
        {
            if (args.MouseArgs.LeftButton == MouseButtonState.Pressed)
            {
               // SelectVertex(args.VertexControl);
                if (AssociatedObject.ViewModel.IsInEditing && AssociatedObject.ViewModel.ToolSetViewModel.IsAddingNewEdge) 
                {
                    if (_startVertex == null) //select starting vertex
                    {
                        _startVertex = (VertexView)args.VertexControl;
                        _endVertex = DataVertex.CreateFakeVertex();
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


                        Point pos = ZoomControl.TranslatePoint(args.VertexControl.GetPosition(), AssociatedObject);
                        var lastseg = _pathGeometry.Figures[0].Segments[_pathGeometry.Figures[0].Segments.Count - 1] as PolyLineSegment;
                        lastseg.Points[lastseg.Points.Count - 1] = pos;

                        var dedge = new DataEdge(_startVertex.ViewModel.DataVertex, _endVertex);
                        AssociatedObject.ViewModel.Logic.Graph.AddVertex(_endVertex);
                        AssociatedObject.ViewModel.Logic.Graph.AddEdge(dedge);
/*

                        GraphExplorerViewModel.Status = GraphExplorerStatus.CreateLinkSelectTarget;
                        GraphExplorerViewModel.PostStatusMessage("Select Target Node");
*/
                    }
/*
                    else if (!GraphExplorerViewModel.View.IsEdVertex(args.VertexControl as VertexControl) && GraphExplorerViewModel.Status.HasFlag(GraphExplorerStatus.CreateLinkSelectTarget)) //finish draw
                    {
                        GraphExplorerViewModel.CreateEdge(GraphExplorerViewModel.View.GetEdVertex().ID, (args.VertexControl.Vertex as DataVertex).ID);

                        GraphExplorerViewModel.ClearEdgeDrawing();

                        GraphExplorerViewModel.Status = GraphExplorerStatus.Ready;

                        GraphExplorerViewModel.IsAddingNewEdge = false;
                    }*/
                }
            }
        }

        private ZoomControl ZoomControl {
            get
            {
                var toolsetView = ServiceLocator.Default.ResolveType<IViewManager>().GetViewsOfViewModel(AssociatedObject.ViewModel.ToolSetViewModel).OfType<GraphToolsetView>().Single();
                return toolsetView.ZoomControl;
            }
        }

        #endregion
    }
}