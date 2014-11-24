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
    using Services;

    public class DragableButtonViewModel : ViewModelBase, IDragable
    {
        private readonly IDataVertexFactory _dataVertexFactory;

        public DragableButtonViewModel(IDataVertexFactory dataVertexFactory)
        {
            _dataVertexFactory = dataVertexFactory;
        }

        public DragDropEffects GetDragEffects()
        {
            return DragDropEffects.Copy;
        }

        public object GetData()
        {
            return _dataVertexFactory.CreateVertex();
        }

        public Type DataType {
            get
            {
                return typeof(DataVertex);
            }
        }
    }
}