#region Copyright (c) 2014 Orcomp development team.
// -------------------------------------------------------------------------------------------------------------------
// <copyright file="DragBehavior.cs" company="Orcomp development team">
//   Copyright (c) 2014 Orcomp development team. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------
#endregion
namespace Orc.GraphExplorer.Behavior
{
    using System.Windows;
    using System.Windows.Input;
    using System.Windows.Interactivity;

    public class DragBehavior : Behavior<FrameworkElement>
    {
        private bool isMouseClicked = false;

        protected override void OnAttached()
        {
            base.OnAttached();
            AssociatedObject.PreviewMouseLeftButtonDown += AssociatedObject_PreviewMouseLeftButtonDown;
            this.AssociatedObject.MouseLeftButtonDown += new MouseButtonEventHandler(AssociatedObject_MouseLeftButtonDown);
            this.AssociatedObject.MouseLeftButtonUp += new MouseButtonEventHandler(AssociatedObject_MouseLeftButtonUp);
            this.AssociatedObject.MouseLeave += new MouseEventHandler(AssociatedObject_MouseLeave);
        }

        void AssociatedObject_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            isMouseClicked = true;
        }

        void AssociatedObject_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            isMouseClicked = true;
        }

        void AssociatedObject_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            isMouseClicked = false;
        }

        void AssociatedObject_MouseLeave(object sender, MouseEventArgs e)
        {
            if (isMouseClicked)
            {
                //set the item's DataContext as the data to be transferred
                /*IDragable dragObject = this.AssociatedObject.DataContext as IDragable;
                if (dragObject != null)
                {
                    DataObject data = new DataObject();
                    data.SetData(dragObject.DataType, this.AssociatedObject.DataContext);
                    System.Windows.DragDrop.DoDragDrop(this.AssociatedObject, data, DragDropEffects.Move);
                }*/
            }
            isMouseClicked = false;
        }
    }
}