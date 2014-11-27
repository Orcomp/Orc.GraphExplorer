#region Copyright (c) 2014 Orcomp development team.
// -------------------------------------------------------------------------------------------------------------------
// <copyright file="FilterView.xaml.cs" company="Orcomp development team">
//   Copyright (c) 2014 Orcomp development team. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------
#endregion

namespace Orc.GraphExplorer.Views
{
    using System.Windows;
    using Catel.MVVM;
    using Catel.Windows;

    using ViewModels;

    /// <summary>
    /// Логика взаимодействия для FilterView.xaml
    /// </summary>
    public partial class FilterView
    {
        #region Constructors
        public FilterView()
        {
            InitializeComponent();
            Loaded += FilterView_Loaded;
            CloseViewModelOnUnloaded = false;

            var filterBuilderControl = FilterBuilderControl;

            filterBuilderControl.CloseViewModelOnUnloaded = false;
        }
        #endregion

        #region Properties
        public new FilterViewModel ViewModel
        {
            get { return base.ViewModel as FilterViewModel; }
        }
        #endregion

        #region Methods
        private void FilterView_Loaded(object sender, RoutedEventArgs e)
        {
            var viewModel = ViewModel;
            if (viewModel == null)
            {
                return;
            }

            var parentViewModel = viewModel.ParentViewModel;
            if (parentViewModel != null)
            {
                return;
            }

            var relationalViewModel = viewModel as IRelationalViewModel;
            var parentView = this.FindLogicalOrVisualAncestorByType<GraphToolsetView>();
            if (parentView != null)
            {
                relationalViewModel.SetParentViewModel(parentView.ViewModel);
            }
        }
        #endregion
    }
}