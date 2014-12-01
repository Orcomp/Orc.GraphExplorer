#region Copyright (c) 2014 Orcomp development team.
// -------------------------------------------------------------------------------------------------------------------
// <copyright file="BaseDropBehavior.cs" company="Orcomp development team">
//   Copyright (c) 2014 Orcomp development team. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------
#endregion

namespace Orc.GraphExplorer.Behaviors.Base
{
    using System.Windows;
    using Catel.MVVM.Views;
    using Catel.Windows.Interactivity;

    public abstract class BaseDropBehavior : BehaviorBase<FrameworkElement>
    {
        #region Fields
        private IUserControl _dropableContent;

        private IDropable _dropableViewModel;
        #endregion

        #region Properties
        private IUserControl DropableContent
        {
            get
            {
                if (_dropableContent == null)
                {
                    _dropableContent = GetDropableContent();
                }

                return _dropableContent;
            }
        }

        private IDropable DropableViewModel
        {
            get
            {
                if (DropableContent == null)
                {
                    return null;
                }

                if (_dropableViewModel == null)
                {
                    _dropableViewModel = DropableContent.ViewModel as IDropable;
                }

                return _dropableViewModel;
            }
        }
        #endregion

        #region Methods
        protected override void OnAssociatedObjectLoaded()
        {
            base.OnAssociatedObjectLoaded();
            AssociatedObject.PreviewDrop += AssociatedObject_PreviewDrop;
            AssociatedObject.DragEnter += AssociatedObject_DragEnter;
        }

        protected abstract IUserControl GetDropableContent();

        private void AssociatedObject_DragEnter(object sender, DragEventArgs e)
        {
            var dropable = DropableViewModel;
            if (dropable == null || !e.Data.GetDataPresent(dropable.DataTypeFormat))
            {
                return;
            }

            e.Effects = dropable.GetDropEffects(e.Data);
        }

        private void AssociatedObject_PreviewDrop(object sender, DragEventArgs e)
        {
            var dropable = DropableViewModel;
            if (dropable == null)
            {
                return;
            }

            Point pos = AssociatedObject.TranslatePoint(e.GetPosition(AssociatedObject), (UIElement) DropableContent);
            dropable.Drop(e.Data, pos);
        }
        #endregion
    }
}