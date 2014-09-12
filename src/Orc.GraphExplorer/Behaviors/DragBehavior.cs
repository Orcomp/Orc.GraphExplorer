#region Copyright (c) 2014 Orcomp development team.
// -------------------------------------------------------------------------------------------------------------------
// <copyright file="DragBehavior.cs" company="Orcomp development team">
//   Copyright (c) 2014 Orcomp development team. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------
#endregion
namespace Orc.GraphExplorer.Behaviors
{
    using System.Windows;
    using System.Windows.Input;
    using System.Windows.Interactivity;

    using Catel.MVVM.Views;

    public class DragBehavior : Behavior<FrameworkElement>
    {
        private bool isMouseClicked = false;

        protected override void OnAttached()
        {
            base.OnAttached();
          //  AssociatedObject.DragEnter +=AssociatedObject_DragEnter;
            AssociatedObject.PreviewMouseLeftButtonDown += AssociatedObject_PreviewMouseLeftButtonDown;
         //   AssociatedObject.PreviewMouseMove += AssociatedObject_PreviewMouseMove;
           // this.AssociatedObject.MouseLeftButtonDown += new MouseButtonEventHandler(AssociatedObject_MouseLeftButtonDown);
  /*          this.AssociatedObject.MouseLeftButtonUp += new MouseButtonEventHandler(AssociatedObject_MouseLeftButtonUp);
            this.AssociatedObject.MouseLeave += new MouseEventHandler(AssociatedObject_MouseLeave);
  */      }

        void AssociatedObject_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            var userControl = this.AssociatedObject as IUserControl;
            if (userControl == null)
            {
                return;
            }

            IDragable dragObject = userControl.ViewModel as IDragable;
            if (dragObject == null)
            {
                return;
            }
            var data = new DataObject();
            data.SetData(dragObject.DataType, userControl.ViewModel);
            DragDrop.DoDragDrop(this.AssociatedObject, data, dragObject.GetDragEffects());
//            isMouseClicked = true;
        }
    }
}