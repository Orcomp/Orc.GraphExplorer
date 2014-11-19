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

    using Catel.Data;
    using Catel.MVVM;
    using Models;
    using Orc.GraphExplorer.Behaviors;

    public class DragableButtonViewModel : ViewModelBase, IDragable
    {
        public DragDropEffects GetDragEffects()
        {
            return DragDropEffects.Copy;
        }

        public object GetData()
        {
            return DataVertex.Create();
        }

        public Type DataType {
            get
            {
                return typeof(DataVertex);
            }
        }
    }
}