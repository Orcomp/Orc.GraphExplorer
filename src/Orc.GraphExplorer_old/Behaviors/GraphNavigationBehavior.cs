#region Copyright (c) 2014 Orcomp development team.
// -------------------------------------------------------------------------------------------------------------------
// <copyright file="GraphNavigationBehavior.cs" company="Orcomp development team">
//   Copyright (c) 2014 Orcomp development team. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------
#endregion
namespace Orc.GraphExplorer.Behaviors
{
    using System.Windows.Interactivity;
    using ObjectModel;

    using Orc.GraphExplorer.Models;

    using Views;

    public class GraphNavigationBehavior : GraphExplorerViewModelContextBehavior<GraphArea>
    {
        protected override void OnAttached()
        {
            base.OnAttached();

            AssociatedObject.VertexDoubleClick += AssociatedObject_VertexDoubleClick;
        }

        void AssociatedObject_VertexDoubleClick(object sender, GraphX.Models.VertexSelectedEventArgs args)
        {
            if (GraphExplorerViewModel.IsInEditing)
            {
                return;
            }

            var vertex = args.VertexControl.DataContext as DataVertex;

            if (vertex == null || Equals(GraphExplorerViewModel._currentNavItem, vertex))
            {
                return;
            }

            GraphExplorerViewModel._currentNavItem = vertex;

            if (!GraphExplorerViewModel.NavigateTo(vertex))
            {
                return;
            }

            if (!GraphExplorerViewModel.IsNavTabVisible)
            {
                GraphExplorerViewModel.IsNavTabVisible = true;
            }

            GraphExplorerViewModel.IsNavTabSelected = true;
        }
    }
}