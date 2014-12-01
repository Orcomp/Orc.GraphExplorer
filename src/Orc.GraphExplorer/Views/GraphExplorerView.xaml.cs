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
