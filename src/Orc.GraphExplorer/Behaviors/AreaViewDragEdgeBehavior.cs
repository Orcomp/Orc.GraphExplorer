#region Copyright (c) 2014 Orcomp development team.
// -------------------------------------------------------------------------------------------------------------------
// <copyright file="AreaViewDragEdgeBehavior.cs" company="Orcomp development team">
//   Copyright (c) 2014 Orcomp development team. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------
#endregion
namespace Orc.GraphExplorer.Behaviors
{
    using System.Data;
    using System.Windows;
    using System.Windows.Input;
    using System.Windows.Interactivity;
    using System.Windows.Media;

    using Catel.IoC;
    using Catel.MVVM;

    using GraphX;
    using GraphX.Models;

    using Orc.GraphExplorer.Enums;
    using Orc.GraphExplorer.ObjectModel;
    using Orc.GraphExplorer.ViewModels;
    using Orc.GraphExplorer.Views;

    public class AreaViewDragEdgeBehavior : DrawEdgeBehavior<AreaView>
    {
        protected override void OnAttached()
        {
            base.OnAttached();
            AssociatedObject.VertexSelected += AssociatedObject_VertexSelected;
            AssociatedObject.TemporaryEdgeCreated += AssociatedObject_TemporaryEdgeCreated;
        }

        void AssociatedObject_TemporaryEdgeCreated(object sender, EdgeControlCreatedAventArgs e)
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
                        GraphExplorerViewModel.EdFakeDV = new DataVertex { ID = -666 };
                        GraphExplorerViewModel.EdGeometry = GraphExplorerViewModel.View.CreatePathGeometry();
                        Point pos = GraphExplorerViewModel.View.zoomctrl.TranslatePoint(args.VertexControl.GetPosition(), GraphExplorerViewModel.View.Area);
                        var lastseg = GraphExplorerViewModel.EdGeometry.Figures[0].Segments[GraphExplorerViewModel.EdGeometry.Figures[0].Segments.Count - 1] as PolyLineSegment;
                        lastseg.Points[lastseg.Points.Count - 1] = pos;

                        // TODO: refactor this
                        var dedge = new DataEdge(GraphExplorerViewModel.View.GetEdVertex(), GraphExplorerViewModel.EdFakeDV);
                        GraphExplorerViewModel.Logic.Graph.AddVertex(GraphExplorerViewModel.EdFakeDV);
                        GraphExplorerViewModel.Logic.Graph.AddEdge(dedge);
                        
                        
                        
                        GraphExplorerViewModel.Editor.Service.SetEdgePathManually(GraphExplorerViewModel.EdGeometry);
                        GraphExplorerViewModel.Status = GraphExplorerStatus.CreateLinkSelectTarget;
                        GraphExplorerViewModel.PostStatusMessage("Select Target Node");
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

            if (GraphExplorerViewModel.SelectedVertices.Contains(v.Id))
            {
                GraphExplorerViewModel.SelectedVertices.Remove(v.Id);
                HighlightBehaviour.SetHighlighted(vc, false);
                //DragBehaviour.SetIsTagged(vc, false);
            }
            else
            {
                GraphExplorerViewModel.SelectedVertices.Add(v.Id);
                HighlightBehaviour.SetHighlighted(vc, true);
                //DragBehaviour.SetIsTagged(vc, true);
            }
        }        
    }
}