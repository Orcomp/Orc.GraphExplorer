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
    using ObjectModel;
    using Operations;
    using Orc.GraphExplorer.Behaviors;
    using Services.Interfaces;
    using Views;

    public class ZoomViewModel : ViewModelBase, IDropable
    {
        private readonly IOperationObserver _operationObserver;

        public ZoomViewModel(IOperationObserver operationObserver)
        {
            _operationObserver = operationObserver;
        }

        public DragDropEffects GetDropEffects(IDataObject dataObject)
        {
            return DragDropEffects.None;
        }

        public void Drop(IDataObject dataObject, Point position)
        {
            if (dataObject.GetDataPresent(typeof (object)))
            {
                _operationObserver.Do(CreateVertexOperation.NewOperation(this, position));
            }
        }
    }
}