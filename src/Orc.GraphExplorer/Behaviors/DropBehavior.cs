#region Copyright (c) 2014 Orcomp development team.
// -------------------------------------------------------------------------------------------------------------------
// <copyright file="DropBehavior.cs" company="Orcomp development team">
//   Copyright (c) 2014 Orcomp development team. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------
#endregion
namespace Orc.GraphExplorer.Behaviors
{
    using System;
    using System.Linq;
    using System.Windows;
    using System.Windows.Interactivity;

    using Catel.MVVM.Views;

    using GraphX;
    using GraphX.Controls;

    using Orc.GraphExplorer.ObjectModel;
    using Orc.GraphExplorer.Operations;
    using Orc.GraphExplorer.ViewModels;
    using Orc.GraphExplorer.Views;
    using Orc.GraphExplorer.Views.Enums;

    public class DropBehavior : Behavior<FrameworkElement>
    {
        protected override void OnAttached()
        {
            base.OnAttached();
            AssociatedObject.PreviewDrop += AssociatedObject_PreviewDrop;
            AssociatedObject.DragEnter += AssociatedObject_DragEnter;
        }

        void AssociatedObject_DragEnter(object sender, DragEventArgs e)
        {
            if (!e.Data.GetDataPresent(typeof(object)))
            {
                e.Effects = DragDropEffects.None;
            }
        }

        private void AssociatedObject_PreviewDrop(object sender, DragEventArgs e)
        {
            var zoomCtrl = AssociatedObject as ZoomView;
            if (zoomCtrl == null)
            {
                return;
            }

            var area = zoomCtrl.Content as AreaView;
            if (area == null)
            {
                return;
            }

            var dropable = zoomCtrl.ViewModel as IDropable;
            if (dropable == null)
            {
                return;
            }

            Point pos = zoomCtrl.TranslatePoint(e.GetPosition(zoomCtrl), area);
            dropable.Drop(e.Data, pos);
        }
    }
}