#region Copyright (c) 2014 Orcomp development team.
// -------------------------------------------------------------------------------------------------------------------
// <copyright file="AreaViewDrawEdgeBehavior.cs" company="Orcomp development team">
//   Copyright (c) 2014 Orcomp development team. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------
#endregion

namespace Orc.GraphExplorer.Behaviors
{
    using System.Windows;
    using System.Windows.Input;
    using System.Windows.Media;
    using Enums;
    using Events;
    using GraphX;
    using GraphX.Models;
    using Models;
    using ObjectModel;
    using Views;

    public class AreaViewDrawEdgeBehavior : GraphExplorerViewModelContextBehavior<AreaView>
    {
        #region Methods
        protected override void OnAttached()
        {
            base.OnAttached();
            AssociatedObject.VertexSelected += AssociatedObject_VertexSelected;
            AssociatedObject.TemporaryEdgeCreated += AssociatedObject_TemporaryEdgeCreated;
        }

        private void AssociatedObject_TemporaryEdgeCreated(object sender, EdgeControlCreatedAventArgs e)
        {
            GraphExplorerViewModel.Editor.Service.SetEdEdge(e.EdgeControl);
        }

        private void AssociatedObject_VertexSelected(object sender, VertexSelectedEventArgs args)
        {
            if (args.MouseArgs.LeftButton == MouseButtonState.Pressed)
            {
                SelectVertex(args.VertexControl);
                if (GraphExplorerViewModel.IsInEditing && GraphExplorerViewModel.Status.HasFlag(GraphExplorerStatus.CreateLinkSelectSource))
                {
                    if (!GraphExplorerViewModel.View.IsVertexEditing) //select starting vertex
                    {
                        GraphExplorerViewModel.View.SetEdVertex(args.VertexControl as VertexControl);
                        GraphExplorerViewModel.EdFakeDV = new DataVertex {ID = -666};
                        GraphExplorerViewModel.EdGeometry = GraphExplorerViewModel.View.CreatePathGeometry();
                        Point pos = GraphExplorerViewModel.View.ZoomCtrl.TranslatePoint(args.VertexControl.GetPosition(), GraphExplorerViewModel.View.Area);
                        var lastseg = GraphExplorerViewModel.EdGeometry.Figures[0].Segments[GraphExplorerViewModel.EdGeometry.Figures[0].Segments.Count - 1] as PolyLineSegment;
                        lastseg.Points[lastseg.Points.Count - 1] = pos;

                        // TODO: refactor this
                        var dedge = new DataEdge(GraphExplorerViewModel.View.GetEdVertex(), GraphExplorerViewModel.EdFakeDV);
                        GraphExplorerViewModel.Editor.Logic.Graph.AddVertex(GraphExplorerViewModel.EdFakeDV);
                        GraphExplorerViewModel.Editor.Logic.Graph.AddEdge(dedge);

                        GraphExplorerViewModel.Editor.Service.SetEdgePathManually(GraphExplorerViewModel.EdGeometry);
                        GraphExplorerViewModel.Status = GraphExplorerStatus.CreateLinkSelectTarget;
                        GraphExplorerViewModel.PostStatusMessage("Select Target Node");
                    }

                    else if (!GraphExplorerViewModel.View.IsEdVertex(args.VertexControl as VertexControl) && GraphExplorerViewModel.Status.HasFlag(GraphExplorerStatus.CreateLinkSelectTarget)) //finish draw
                    {
                        GraphExplorerViewModel.CreateEdge(GraphExplorerViewModel.View.GetEdVertex().ID, (args.VertexControl.Vertex as DataVertex).ID);

                        GraphExplorerViewModel.ClearEdgeDrawing();

                        GraphExplorerViewModel.Status = GraphExplorerStatus.Ready;

                        GraphExplorerViewModel.IsAddingNewEdge = false;
                    }
                }
            }
        }

        private void SelectVertex(VertexControl vc)
        {
            var v = vc.Vertex as DataVertex;
            if (v == null)
            {
                return;
            }

            if (GraphExplorerViewModel.SelectedVertices.Contains(v.ID))
            {
                GraphExplorerViewModel.SelectedVertices.Remove(v.ID);
                HighlightBehaviour.SetHighlighted(vc, false);
                //DragBehaviour.SetIsTagged(vc, false);
            }
            else
            {
                GraphExplorerViewModel.SelectedVertices.Add(v.ID);
                HighlightBehaviour.SetHighlighted(vc, true);
                //DragBehaviour.SetIsTagged(vc, true);
            }
        }
        #endregion
    }
}