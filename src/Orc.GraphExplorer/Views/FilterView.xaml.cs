using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Orc.GraphExplorer.Views
{
    using System.Collections.Specialized;
    using System.ComponentModel;
    using System.Threading;
    using Catel;
    using Catel.Data;
    using Catel.MVVM;
    using Models;
    using Orc.FilterBuilder.ViewModels;
    using Orc.GraphExplorer.ViewModels;

    /// <summary>
    /// Логика взаимодействия для FilterView.xaml
    /// </summary>
    public partial class FilterView 
    {
        public FilterView()
        {
            InitializeComponent();
            Loaded += FilterView_Loaded;
            CloseViewModelOnUnloaded = false;
            
            FilterBuilderControl.CloseViewModelOnUnloaded = false;
            FilterBuilderControl.ViewModelChanged += FilterBuilderControl_ViewModelChanged;
        }


        void FilterBuilderControl_ViewModelChanged(object sender, EventArgs e)
        {
            var filterBuilderViewModel = FilterBuilderControl.ViewModel as FilterBuilderViewModel;
            if (filterBuilderViewModel == null)
            {
                return;
            }

        }

        void FilterView_Loaded(object sender, RoutedEventArgs e)
        {                                     
            if (ViewModel.ParentViewModel != null)
            {
                return;
            }

            var relationalViewModel = ViewModel as IRelationalViewModel;
            var parentView = this.FindFirstParentOfType<GraphToolsetView>();
            if (parentView != null && relationalViewModel != null && ViewModel.ParentViewModel == null)
            {
                relationalViewModel.SetParentViewModel(parentView.ViewModel);
            }

            if (IsVisible)
            {
                
            }
        }

        public FilterViewModel ViewModel
        {
            get
            {
                return base.ViewModel as FilterViewModel;
            }
        }
    }
}
