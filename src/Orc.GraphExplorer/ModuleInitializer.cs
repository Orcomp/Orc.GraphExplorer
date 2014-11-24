using Catel.IoC;

using Orc.GraphExplorer;
using Orc.GraphExplorer.Csv.Services;
using Orc.GraphExplorer.Services;
using GraphX.Controls.Models;

using Catel.Memento;

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
        serviceLocator.RegisterType<IConfigLocationService, ConfigLocationService>();    
        serviceLocator.RegisterType<IGraphControlFactory, CustomGraphControlFactory>();
        serviceLocator.RegisterType(typeof(IGraphDataGetter), typeof(CsvGraphDataService), GraphDataServiceEnum.Csv);
        serviceLocator.RegisterType<IMementoService, MementoService>();
        serviceLocator.RegisterType<IGraphAreaEditorService, GraphAreaEditorService>();
        serviceLocator.RegisterType<IGraphAreaLoadingService, GraphAreaLoadingService>();
        serviceLocator.RegisterType<IEdgeDrawingService, EdgeDrawingService>();
        serviceLocator.RegisterType<IDataVertexFactory, DataVertexFactory>();
        serviceLocator.RegisterType<IGraphDataService, CsvGraphDataService>();
        serviceLocator.RegisterType<IGraphExplorerFactory, GraphExplorerFactory>();       
    }
}