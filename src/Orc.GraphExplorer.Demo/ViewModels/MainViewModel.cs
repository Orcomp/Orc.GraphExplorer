namespace Orc.GraphExplorer.Demo.ViewModels
{
    using Catel;
    using Catel.MVVM;
    using Catel.Services;
    using Models;
    using Orc.GraphExplorer.Services;
    using Orc.GraphExplorer.Services.Interfaces;

    public class MainViewModel : ViewModelBase
    {
        public MainViewModel()
        {

        }

        public IGraphDataService GraphDataService
        {
            get
            {
                return new CsvGraphDataService();
            }
        }
    }
}
