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

    using Catel.MVVM.Converters;
    using Catel.MVVM.Views;
    using Interfaces;

    public class DragBehavior : Behavior<FrameworkElement>
    {
        #region Methods
        protected override void OnAttached()
        {
            base.OnAttached();
            AssociatedObject.PreviewMouseLeftButtonDown += AssociatedObject_PreviewMouseLeftButtonDown;
        }

        private void AssociatedObject_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            var userControl = AssociatedObject as IUserControl;
            if (userControl == null)
            {
                return;
            }

            var dragObject = userControl.ViewModel as IDragable;
            if (dragObject == null)
            {
                return;
            }
            var data = new DataObject();
            data.SetData(dragObject.DataType, dragObject.GetData());
            DragDrop.DoDragDrop(AssociatedObject, data, dragObject.GetDragEffects());
        }
        #endregion

    }
}