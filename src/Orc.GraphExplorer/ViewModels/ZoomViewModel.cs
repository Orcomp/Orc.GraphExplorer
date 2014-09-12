#region Copyright (c) 2014 Orcomp development team.
// -------------------------------------------------------------------------------------------------------------------
// <copyright file="ZoomViewModel.cs" company="Orcomp development team">
//   Copyright (c) 2014 Orcomp development team. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------
#endregion
namespace Orc.GraphExplorer.ViewModels
{
    using System.Windows;

    using Catel.MVVM;

    using Orc.GraphExplorer.Behaviors;

    public class ZoomViewModel : ViewModelBase, IDropable
    {
        public ZoomViewModel()
        {
            
        }

        public DragDropEffects GetDropEffects(IDataObject dataObject)
        {
            return DragDropEffects.None;
        }

        public void Drop(IDataObject dataObject, Point position)
        {
            throw new System.NotImplementedException();
        }
    }
}