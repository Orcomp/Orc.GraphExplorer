#region Copyright (c) 2014 Orcomp development team.
// -------------------------------------------------------------------------------------------------------------------
// <copyright file="App.xaml.cs" company="Orcomp development team">
//   Copyright (c) 2014 Orcomp development team. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------
#endregion

namespace Orc.GraphExplorer.Demo
{
    using System;
    using System.Windows;
    using Catel.IoC;
    using Orchestra.Services;
    using Orchestra.Views;

    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        #region Methods
        protected override void OnStartup(StartupEventArgs e)
        {
            var serviceLocator = this.GetServiceLocator();

            var shellService = serviceLocator.ResolveType<IShellService>();
            shellService.CreateWithSplash<ShellWindow>();
        }
        #endregion
    }
}