#region Copyright (c) 2014 Orcomp development team.
// -------------------------------------------------------------------------------------------------------------------
// <copyright file="ZoomDropBehavior.cs" company="Orcomp development team">
//   Copyright (c) 2014 Orcomp development team. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------
#endregion

namespace Orc.GraphExplorer.Behaviors
{
    using System.Linq;
    using Base;
    using Catel.IoC;
    using Catel.MVVM;
    using Catel.MVVM.Views;
    using ViewModels;

    public class ZoomDropBehavior : BaseDropBehavior
    {
        #region Methods
        protected override IUserControl GetDropableContent()
        {
            var toolset = AssociatedObject.DataContext as GraphToolsetViewModel;
            if (toolset == null)
            {
                return null;
            }

            var serviceLocator = this.GetServiceLocator();

            var viewModelManager = serviceLocator.ResolveType<IViewModelManager>();
            var graphAreaViewModel = viewModelManager.GetChildViewModels(toolset).OfType<GraphAreaViewModel>().FirstOrDefault();
            if (graphAreaViewModel == null)
            {
                return null;
            }

            var viewManager = serviceLocator.ResolveType<IViewManager>();
            return (IUserControl) viewManager.GetViewsOfViewModel(graphAreaViewModel).FirstOrDefault();
        }
        #endregion
    }
}