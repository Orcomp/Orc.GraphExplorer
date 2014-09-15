#region Copyright (c) 2014 Orcomp development team.
// -------------------------------------------------------------------------------------------------------------------
// <copyright file="DrawEdgeBehavior.cs" company="Orcomp development team">
//   Copyright (c) 2014 Orcomp development team. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------
#endregion
namespace Orc.GraphExplorer.Behaviors
{
    using System.Data;
    using System.Windows;
    using System.Windows.Interactivity;
    using System.Windows.Media;

    using Catel.IoC;
    using Catel.MVVM;

    using GraphX;

    using Orc.GraphExplorer.ObjectModel;
    using Orc.GraphExplorer.ViewModels;

    public abstract class DrawEdgeBehavior<T> : Behavior<T>
        where T : FrameworkElement
    {
        private GraphExplorerViewModel _graphExplorerViewModel;
        protected GraphExplorerViewModel GraphExplorerViewModel
        {
            get
            {
                if (_graphExplorerViewModel == null)
                {
                    var viewModelManager = ServiceLocator.Default.ResolveType<IViewModelManager>();

                    _graphExplorerViewModel = viewModelManager.GetFirstOrDefaultInstance<GraphExplorerViewModel>();
                    if (_graphExplorerViewModel == null)
                    {
                        throw new NoNullAllowedException(string.Format("Uable to find viewmodel {0}", typeof(GraphExplorerViewModel)));
                    }
                }
                return _graphExplorerViewModel;
            }
        }        
    }
}