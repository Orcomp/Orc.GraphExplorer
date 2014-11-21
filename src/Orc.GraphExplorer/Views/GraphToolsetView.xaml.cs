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
    using System.ComponentModel;

    using Catel.MVVM;

    using Orc.GraphExplorer.Messages;
    using Orc.GraphExplorer.ViewModels;

    /// <summary>
    /// Логика взаимодействия для GraphToolsetView.xaml
    /// </summary>
    public partial class GraphToolsetView
    {
        public GraphToolsetView()
        {
            CloseViewModelOnUnloaded = false;
            InitializeComponent();
            Loaded += GraphToolsetView_Loaded;
        }

        void GraphToolsetView_Loaded(object sender, RoutedEventArgs e)
        {
            var viewModel = ViewModel;

            var relationalViewModel = viewModel as IRelationalViewModel;
            var parentView = this.FindFirstParentOfType<GraphExplorerView>();
            if (parentView != null && relationalViewModel != null && viewModel.ParentViewModel == null)
            {
                relationalViewModel.SetParentViewModel(parentView.ViewModel);
            }
        }

        protected override void OnViewModelChanged()
        {
         //   DataContext = ViewModel;
            base.OnViewModelChanged();
        }

        public new GraphToolsetViewModel ViewModel {
            get
            {
                return base.ViewModel as GraphToolsetViewModel;
            }
        }
    }
}
