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
    using Catel.MVVM.Views;
    using Catel.Windows.Interactivity;
    using DragDrop = System.Windows.DragDrop;

    public class DragBehavior : BehaviorBase<FrameworkElement>
    {
        #region Methods
        protected override void OnAssociatedObjectLoaded()
        {
            base.OnAssociatedObjectLoaded();
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