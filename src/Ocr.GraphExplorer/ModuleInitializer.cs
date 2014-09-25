using Catel.IoC;

using Orc.GraphExplorer.Csv.Services;
using Orc.GraphExplorer.Services.Interfaces;
using Orc.GraphExplorer.Services;
using GraphX.Controls.Models;
using Orc.GraphExplorer.Enums;

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
        serviceLocator.RegisterType<IGraphControlFactory, CustomGraphControlFactory>();
        serviceLocator.RegisterType(typeof(IGraphDataService), typeof(CsvGraphDataService), GraphDataServiceEnum.Csv.ToString());
    }
}