#region Copyright (c) 2014 Orcomp development team.
// -------------------------------------------------------------------------------------------------------------------
// <copyright file="ZoomViewDrawEdgeBehavior.cs" company="Orcomp development team">
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
    using Catel.MVVM.Views;

    using GraphX;
    using GraphX.Models;

    using Orc.GraphExplorer.Enums;
    using Orc.GraphExplorer.ObjectModel;
    using Orc.GraphExplorer.ViewModels;
    using Orc.GraphExplorer.Views;

    public class ZoomViewDrawEdgeBehavior : DrawEdgeBehavior<ZoomView>
    {
        protected override void OnAttached()
        {
            base.OnAttached();
            AssociatedObject.PreviewMouseMove +=AssociatedObject_PreviewMouseMove;
        }

        private void AssociatedObject_PreviewMouseMove(object sender, MouseEventArgs e)
        {
            var userControl = this.AssociatedObject as IUserControl;
            if (userControl == null)
            {
                return;
            }

            var viewModel = userControl.ViewModel as ZoomViewModel;
            if (viewModel == null)
            {
                return;
            }

            var dataContext = AssociatedObject.DataContext as GraphExplorerViewModel;
            if (dataContext == null)
            {
                return;
            }

            if (dataContext.Status.HasFlag(GraphExplorerStatus.CreateLinkSelectTarget) && dataContext.EdGeometry != null && GraphExplorerViewModel.Editor.Service.IsEdgeEditing && dataContext.View.IsVertexEditing)
            {
                Point pos = dataContext.View.zoomctrl.TranslatePoint(e.GetPosition(dataContext.View.zoomctrl), dataContext.View.Area);
                var lastseg = dataContext.EdGeometry.Figures[0].Segments[dataContext.EdGeometry.Figures[0].Segments.Count - 1] as PolyLineSegment;
                lastseg.Points[lastseg.Points.Count - 1] = pos;
                GraphExplorerViewModel.Editor.Service.SetEdgePathManually(dataContext.EdGeometry);
            }
        }
    }
}