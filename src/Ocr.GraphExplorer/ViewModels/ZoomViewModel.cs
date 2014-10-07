#region Copyright (c) 2014 Orcomp development team.
// -------------------------------------------------------------------------------------------------------------------
// <copyright file="ZoomViewModel.cs" company="Orcomp development team">
//   Copyright (c) 2014 Orcomp development team. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------
#endregion
namespace Orc.GraphExplorer.ViewModels
{
    using System;
    using System.Windows;
    using Behaviors.Interfaces;
    using Catel.IoC;
    using Catel.MVVM;
    using Orc.GraphExplorer.Behaviors;
    using Orc.GraphExplorer.Models;

    using Services.Interfaces;
    using Views;

    public class ZoomViewModel : ViewModelBase, IDropable
    {
        public Type DataTypeFormat
        {
            get { return typeof(DataVertex); }
        }
        public DragDropEffects GetDropEffects(IDataObject dataObject)
        {
            return DragDropEffects.None;
        }

        public void Drop(IDataObject dataObject, Point position)
        {
            //  throw new NotImplementedException();
        }
    }
}