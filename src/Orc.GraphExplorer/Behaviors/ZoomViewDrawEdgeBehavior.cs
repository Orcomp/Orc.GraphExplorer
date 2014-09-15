#region Copyright (c) 2014 Orcomp development team.
// -------------------------------------------------------------------------------------------------------------------
// <copyright file="ZoomViewDrawEdgeBehavior.cs" company="Orcomp development team">
//   Copyright (c) 2014 Orcomp development team. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------
#endregion

namespace Orc.GraphExplorer.Behaviors
{
    using System.Windows;
    using System.Windows.Input;
    using System.Windows.Media;
    using Catel.MVVM.Views;
    using Enums;
    using ViewModels;
    using Views;

    public class ZoomViewDrawEdgeBehavior : GraphExplorerViewModelContextBehavior<ZoomView>
    {
        #region Methods
        protected override void OnAttached()
        {
            base.OnAttached();
            AssociatedObject.PreviewMouseMove += AssociatedObject_PreviewMouseMove;
        }

        private void AssociatedObject_PreviewMouseMove(object sender, MouseEventArgs e)
        {
            var userControl = AssociatedObject as IUserControl;
            if (userControl == null)
            {
                return;
            }

            var viewModel = userControl.ViewModel as ZoomViewModel;
            if (viewModel == null)
            {
                return;
            }

            if (!GraphExplorerViewModel.Status.HasFlag(GraphExplorerStatus.CreateLinkSelectTarget) || GraphExplorerViewModel.EdGeometry == null || !GraphExplorerViewModel.Editor.Service.IsEdgeEditing || !GraphExplorerViewModel.View.IsVertexEditing)
            {
                return;
            }

            var pos = GraphExplorerViewModel.View.zoomctrl.TranslatePoint(e.GetPosition(GraphExplorerViewModel.View.zoomctrl), GraphExplorerViewModel.View.Area);
            var lastseg = GraphExplorerViewModel.EdGeometry.Figures[0].Segments[GraphExplorerViewModel.EdGeometry.Figures[0].Segments.Count - 1] as PolyLineSegment;
            lastseg.Points[lastseg.Points.Count - 1] = pos;
            GraphExplorerViewModel.Editor.Service.SetEdgePathManually(GraphExplorerViewModel.EdGeometry);
        }
        #endregion
    }
}