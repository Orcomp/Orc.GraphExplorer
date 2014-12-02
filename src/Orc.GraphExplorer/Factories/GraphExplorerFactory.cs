#region Copyright (c) 2014 Orcomp development team.
// -------------------------------------------------------------------------------------------------------------------
// <copyright file="GraphExplorerFactory.cs" company="Orcomp development team">
//   Copyright (c) 2014 Orcomp development team. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------
#endregion

namespace Orc.GraphExplorer.Factories
{
    using Catel;
    using Messages;
    using Models;
    using Services;

    public class GraphExplorerFactory : IGraphExplorerFactory
    {
        #region Fields
        private readonly IDataLocationSettingsService _dataLocationSettingsService;
        #endregion

        #region Constructors
        public GraphExplorerFactory(IDataLocationSettingsService dataLocationSettingsService)
        {
            Argument.IsNotNull(() => dataLocationSettingsService);

            _dataLocationSettingsService = dataLocationSettingsService;
        }
        #endregion

        #region IGraphExplorerFactory Members
        public Explorer CreateExplorer()
        {
            var explorer = new Explorer();
            explorer.EditorToolset = new GraphToolset("Editor", true);
            explorer.NavigatorToolset = new GraphToolset("Navigator", false);

            explorer.Settings = new Settings();

            explorer.Settings.DataLocationSettings = _dataLocationSettingsService.Load();
            SettingsChangedMessage.SendWith(true);

            return explorer;
        }
        #endregion
    }
}