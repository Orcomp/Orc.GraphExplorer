#region Copyright (c) 2014 Orcomp development team.
// -------------------------------------------------------------------------------------------------------------------
// <copyright file="AreaViewContextMenuBehavior.cs" company="Orcomp development team">
//   Copyright (c) 2014 Orcomp development team. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------
#endregion
namespace Orc.GraphExplorer.Behaviors
{
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Interactivity;
    using GraphX;
    using Models;
    using ObjectModel;
    using Operations;
    using ViewModels;
    using Views;

    public class AreaViewContextMenuBehavior : GraphExplorerViewModelContextBehavior<GraphArea>
    {
        protected override void OnAttached()
        {
            base.OnAttached();

            AssociatedObject.EdgeSelected += AssociatedObject_EdgeSelected;
            AssociatedObject.VertexSelected += AssociatedObject_VertexSelected;
        }

        void AssociatedObject_VertexSelected(object sender, GraphX.Models.VertexSelectedEventArgs args)
        {
            if (GraphExplorerViewModel.IsInEditing)
            {
                args.VertexControl.ContextMenu = new ContextMenu();
                var miDeleteVertex = new MenuItem { Header = "Delete", Tag = args.VertexControl };
                miDeleteVertex.Click += miDeleteVertex_Click;
                args.VertexControl.ContextMenu.Items.Add(miDeleteVertex);
            }
        }

        private void miDeleteVertex_Click(object sender, RoutedEventArgs e)
        {
            var vCtrl = (sender as MenuItem).Tag as VertexControl;
            if (vCtrl != null)
            {
                var op = new DeleteVertexOperation(GraphExplorerViewModel.Editor, GraphExplorerViewModel.View.Area, (GraphLogic)GraphExplorerViewModel.View.Area.LogicCore, vCtrl.Vertex as DataVertex, (dv) => { }, dv => { GraphExplorerViewModel.View.Area.RelayoutGraph(true); });
                GraphExplorerViewModel.Explorer.OperationObserver.Do(op);
            }
        }

        void AssociatedObject_EdgeSelected(object sender, GraphX.Models.EdgeSelectedEventArgs args)
        {
            if (GraphExplorerViewModel.IsInEditing)
            {
                args.EdgeControl.ContextMenu = new ContextMenu();
                var miDeleteLink = new MenuItem { Header = "Delete Link", Tag = args.EdgeControl };
                miDeleteLink.Click += miDeleteLink_Click;
                args.EdgeControl.ContextMenu.Items.Add(miDeleteLink);
            }
        }

        private void miDeleteLink_Click(object sender, RoutedEventArgs e)
        {
            var eCtrl = (sender as MenuItem).Tag as EdgeControl;
            if (eCtrl != null)
            {
                var edge = eCtrl.Edge as DataEdge;

                var op = new DeleteEdgeOperation(GraphExplorerViewModel.Editor, AssociatedObject, edge.Source, edge.Target, edge, ec =>
                {
                    //do nothing
                }, ec =>
                {
                    //do nothing
                });

                GraphExplorerViewModel.Explorer.OperationObserver.Do(op);
            }
            //throw new NotImplementedException();
        }
    }
}