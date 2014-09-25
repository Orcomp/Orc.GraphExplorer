#region Copyright (c) 2014 Orcomp development team.
// -------------------------------------------------------------------------------------------------------------------
// <copyright file="GraphExplorerViewModelContextBehavior.cs" company="Orcomp development team">
//   Copyright (c) 2014 Orcomp development team. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------
#endregion

namespace Orc.GraphExplorer.Behaviors
{
    using System.Data;
    using System.Windows;
    using System.Windows.Interactivity;
    using Catel.IoC;
    using Catel.MVVM;
    using ViewModels;

    public abstract class GraphExplorerViewModelContextBehavior<T> : Behavior<T>
        where T : FrameworkElement
    {
        #region Fields
        private GraphExplorerViewModel _graphExplorerViewModel;
        #endregion

        #region Properties
        protected GraphExplorerViewModel GraphExplorerViewModel
        {
            get
            {
                if (_graphExplorerViewModel == null)
                {
                    _graphExplorerViewModel = AssociatedObject.DataContext as GraphExplorerViewModel;
                    if (_graphExplorerViewModel == null)
                    {
                        throw new NoNullAllowedException(string.Format("Behavior {0} allowed to use only when the DateContext property of the associated object has type {1}", GetType(), typeof (GraphExplorerViewModel)));
                    }
                }

                return _graphExplorerViewModel;
            }
        }
        #endregion
    }
}