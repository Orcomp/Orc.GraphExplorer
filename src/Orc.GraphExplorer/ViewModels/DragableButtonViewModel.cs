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
    using Behaviors;
    using Catel;
    using Catel.MVVM;
    using Models;
    using Services;

    public class DragableButtonViewModel : ViewModelBase, IDragable
    {
        #region Fields
        private readonly IDataVertexFactory _dataVertexFactory;
        #endregion

        #region Constructors
        public DragableButtonViewModel(IDataVertexFactory dataVertexFactory)
        {
            Argument.IsNotNull(() => dataVertexFactory);

            _dataVertexFactory = dataVertexFactory;
        }
        #endregion

        #region IDragable Members
        public DragDropEffects GetDragEffects()
        {
            return DragDropEffects.Copy;
        }

        public object GetData()
        {
            return _dataVertexFactory.CreateVertex();
        }

        public Type DataType
        {
            get { return typeof (DataVertex); }
        }
        #endregion
    }
}