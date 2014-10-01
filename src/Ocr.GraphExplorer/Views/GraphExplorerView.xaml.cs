namespace Orc.GraphExplorer.Views
{
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
            DataContext = ViewModel;
            base.OnViewModelChanged();
        }
    }
}
