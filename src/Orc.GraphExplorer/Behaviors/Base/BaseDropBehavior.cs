#region Copyright (c) 2014 Orcomp development team.
// -------------------------------------------------------------------------------------------------------------------
// <copyright file="BaseDropBehavior.cs" company="Orcomp development team">
//   Copyright (c) 2014 Orcomp development team. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------
#endregion
namespace Orc.GraphExplorer.Behaviors.Base
{
    using System.Linq;
    using System.Windows;
    using System.Windows.Interactivity;

    using Catel.IoC;
    using Catel.MVVM;
    using Catel.MVVM.Views;

    using Orc.GraphExplorer.Behaviors.Interfaces;
    using Orc.GraphExplorer.ViewModels;
    using Orc.GraphExplorer.Views;

    public abstract class BaseDropBehavior : Behavior<FrameworkElement>
    {
        #region Methods
        protected override void OnAttached()
        {
            base.OnAttached();
            AssociatedObject.PreviewDrop += AssociatedObject_PreviewDrop;
            AssociatedObject.DragEnter += AssociatedObject_DragEnter;
        }


        private IUserControl _dropableContent;
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

        protected abstract IUserControl GetDropableContent();        

        private void AssociatedObject_DragEnter(object sender, DragEventArgs e)
        {
            if (DropableContent == null)
            {
                return;
            }

            var dropable = DropableContent.ViewModel as IDropable;

            if (dropable == null || !e.Data.GetDataPresent(dropable.DataTypeFormat))
            {
                return;
            }

            e.Effects = dropable.GetDropEffects(e.Data);
        }

        private void AssociatedObject_PreviewDrop(object sender, DragEventArgs e)
        {
            if (DropableContent == null)
            {
                return;
            }

            var dropable = DropableContent.ViewModel as IDropable;
            if (dropable == null)
            {
                return;
            }

            Point pos = AssociatedObject.TranslatePoint(e.GetPosition(AssociatedObject), (UIElement)DropableContent);
            dropable.Drop(e.Data, pos);
        }
        #endregion
    }
}