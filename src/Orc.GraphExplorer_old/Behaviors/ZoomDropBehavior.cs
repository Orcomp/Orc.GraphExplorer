#region Copyright (c) 2014 Orcomp development team.
// -------------------------------------------------------------------------------------------------------------------
// <copyright file="ZoomDropBehavior.cs" company="Orcomp development team">
//   Copyright (c) 2014 Orcomp development team. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------
#endregion

namespace Orc.GraphExplorer.Behaviors
{
    using System.Windows;
    using System.Windows.Interactivity;
    using Interfaces;
    using Views;

    public class ZoomDropBehavior : Behavior<ZoomView>
    {
        #region Methods
        protected override void OnAttached()
        {
            base.OnAttached();
            AssociatedObject.PreviewDrop += AssociatedObject_PreviewDrop;
            AssociatedObject.DragEnter += AssociatedObject_DragEnter;
        }

        private void AssociatedObject_DragEnter(object sender, DragEventArgs e)
        {
            if (!e.Data.GetDataPresent(typeof (object)))
            {
                var dropable = AssociatedObject.ViewModel as IDropable;
                if (dropable == null)
                {
                    return;
                }

                e.Effects = dropable.GetDropEffects(e.Data);
            }
        }

        private void AssociatedObject_PreviewDrop(object sender, DragEventArgs e)
        {
            var area = AssociatedObject.Content as GraphArea;
            if (area == null)
            {
                return;
            }

            var dropable = AssociatedObject.ViewModel as IDropable;
            if (dropable == null)
            {
                return;
            }

            Point pos = AssociatedObject.TranslatePoint(e.GetPosition(AssociatedObject), area);
            dropable.Drop(e.Data, pos);
        }
        #endregion
    }
}