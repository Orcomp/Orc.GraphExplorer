using Catel.IoC;
using Catel.MVVM;
using Orc.GraphExplorer.Demo.ViewModels;
using Orc.GraphExplorer.Demo.Views;
using Orchestra.Services;
using Orc.GraphExplorer.Demo.Services;


/// <summary>
/// Used by the ModuleInit. All code inside the Initialize method is ran as soon as the assembly is loaded.
/// </summary>
public static class ModuleInitializer
{    
    /// <summary>
    /// Initializes the module.
    /// </summary>
    public static void Initialize()
    {
        var serviceLocator = ServiceLocator.Default;
        serviceLocator.RegisterType<IMahAppsService, MahAppsService>();

        var viewModelLocator = serviceLocator.ResolveType<IViewModelLocator>();
        viewModelLocator.Register(typeof(MainWindow), typeof(MainViewModel));

        var viewLocator = serviceLocator.ResolveType<IViewLocator>();
        viewLocator.Register(typeof(MainViewModel), typeof(MainWindow));

        /*  var uiVisualizerService = serviceLocator.ResolveType<Catel.Services.IUIVisualizerService>();
        uiVisualizerService.Register("MainWindow", typeof(MainWindow));*/
    }
}