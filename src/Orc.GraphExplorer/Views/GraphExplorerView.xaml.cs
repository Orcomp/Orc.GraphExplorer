namespace Orc.GraphExplorer.Views
{
    using ViewModels;

    /// <summary>
    /// Логика взаимодействия для GraphExplorerView.xaml
    /// </summary>
    public partial class GraphExplorerView
    {
        public GraphExplorerView()
        {
            InitializeComponent();

            Loaded += GraphExplorerView_Loaded;
        }

        void GraphExplorerView_Loaded(object sender, System.Windows.RoutedEventArgs e)
        {
            if (ViewModel == null)
            {
                return;
            }
        }

        protected override void OnViewModelChanged()
        {
            base.OnViewModelChanged();
           
            DataContext = ViewModel;                       
        }

        public new GraphExplorerViewModel ViewModel {
            get { return base.ViewModel as GraphExplorerViewModel; }
        }
    }
}
