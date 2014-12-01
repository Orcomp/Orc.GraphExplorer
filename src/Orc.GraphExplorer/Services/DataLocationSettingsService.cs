#region Copyright (c) 2014 Orcomp development team.
// -------------------------------------------------------------------------------------------------------------------
// <copyright file="DataLocationSettingsService.cs" company="Orcomp development team">
//   Copyright (c) 2014 Orcomp development team. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------
#endregion

namespace Orc.GraphExplorer.Services
{
    using Catel;
    using Catel.Configuration;
    using Catel.Services;
    using Models;

    internal class DataLocationSettingsService : IDataLocationSettingsService
    {
        #region Constants
        private const string RelationshipsFile = "RelationshipsFile";
        private const string PropertiesFile = "PropertiesFile";
        private const string EnableProperty = "EnableProperty";
        #endregion

        #region Fields
        private readonly IOpenFileService _openFileService;
        private readonly IConfigurationService _configurationService;
        private DataLocationSettings _actualSettings;
        #endregion

        #region Constructors
        public DataLocationSettingsService(IOpenFileService openFileService, IConfigurationService configurationService)
        {
            Argument.IsNotNull(() => openFileService);
            Argument.IsNotNull(() => configurationService);

            _openFileService = openFileService;
            _configurationService = configurationService;
        }
        #endregion

        #region IDataLocationSettingsService Members
        public DataLocationSettings Load()
        {
            _actualSettings = new DataLocationSettings()
            {
                RelationshipsFile = _configurationService.GetValue<string>(RelationshipsFile, @"Data\Relationships.csv"),
                PropertiesFile = _configurationService.GetValue<string>(PropertiesFile, @"Data\Properties.csv"),
                EnableProperty = _configurationService.GetValue<bool>(EnableProperty, false)
            };

            return _actualSettings;
        }

        public DataLocationSettings GetCurrentOrLoad()
        {
            return _actualSettings ?? Load();
        }

        public void Save(DataLocationSettings dataLocationSettings)
        {
            Argument.IsNotNull(() => dataLocationSettings);

            _actualSettings = dataLocationSettings;

            _configurationService.SetValue(RelationshipsFile, dataLocationSettings.RelationshipsFile);
            _configurationService.SetValue(PropertiesFile, dataLocationSettings.PropertiesFile);
            _configurationService.SetValue(EnableProperty, dataLocationSettings.EnableProperty ?? false);
        }
        #endregion
    }
}