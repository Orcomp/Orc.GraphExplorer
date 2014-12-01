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
    using Catel.MVVM;
    using Catel.Windows;

    using Orc.GraphExplorer.ViewModels;

    /// <summary>
    /// Логика взаимодействия для PropertyView.xaml
    /// </summary>
    public partial class PropertyView
    {
        public PropertyView()
        {
            InitializeComponent();
        }

        protected override void OnViewModelChanged()
        {
            DataContext = ViewModel;
            base.OnViewModelChanged();

            Loaded += PropertyView_Loaded;
            CloseViewModelOnUnloaded = false;
        }

        void PropertyView_Loaded(object sender, RoutedEventArgs e)
        {
            var viewModel = ViewModel;

            var relationalViewModel = viewModel as IRelationalViewModel;
            var parentView = this.FindLogicalOrVisualAncestorByType<VertexView>();
            if (parentView != null && relationalViewModel != null && viewModel.ParentViewModel == null)
            {
                relationalViewModel.SetParentViewModel(parentView.ViewModel);
            }
        }

        public new PropertyViewModel ViewModel {
            get
            {
                return base.ViewModel as PropertyViewModel;
            }
        }
    }
}
