#region Copyright (c) 2014 Orcomp development team.
// -------------------------------------------------------------------------------------------------------------------
// <copyright file="ZoomDropBehavior.cs" company="Orcomp development team">
//   Copyright (c) 2014 Orcomp development team. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------
#endregion

namespace Orc.GraphExplorer.Behaviors
{
    using System;
    using System.Linq;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Interactivity;

    using Catel.IoC;
    using Catel.MVVM;
    using Catel.MVVM.Views;
    using Catel.Windows;

    using GraphX.Controls;

    using Interfaces;

    using Orc.GraphExplorer.Behaviors.Base;
    using Orc.GraphExplorer.ViewModels;

    using Views;

    public class ZoomDropBehavior : BaseDropBehavior
    {
        protected override IUserControl GetDropableContent()
        {
            var toolset = AssociatedObject.DataContext as GraphToolsetViewModel;
            if (toolset == null)
            {
                return null;
            }

            var viewModelManager = ServiceLocator.Default.ResolveType<IViewModelManager>();
            var graphAreaViewModel = viewModelManager.GetChildViewModels(toolset).OfType<GraphAreaViewModel>().FirstOrDefault();
            if (graphAreaViewModel == null)
            {
                return null;
            }

            var viewManager = ServiceLocator.Default.ResolveType<IViewManager>();
            return (IUserControl)viewManager.GetViewsOfViewModel(graphAreaViewModel).FirstOrDefault();
        }
    }
}