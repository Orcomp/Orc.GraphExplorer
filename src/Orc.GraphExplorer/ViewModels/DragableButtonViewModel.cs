#region Copyright (c) 2014 Orcomp development team.
// -------------------------------------------------------------------------------------------------------------------
// <copyright file="DragableButtonViewModel.cs" company="Orcomp development team">
//   Copyright (c) 2014 Orcomp development team. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------
#endregion
namespace Orc.GraphExplorer.ViewModels
{
    using System;
    using System.Windows;

    using Catel.MVVM;

    using Orc.GraphExplorer.Behaviors;

    public class DragableButtonViewModel : ViewModelBase, IDragable
    {
        public DragableButtonViewModel()
        {
            
        }

        public DragDropEffects GetDragEffects()
        {
            return DragDropEffects.Copy;
        }

        public Type DataType {
            get
            {
                return typeof(object);
            }
        }
    }
}