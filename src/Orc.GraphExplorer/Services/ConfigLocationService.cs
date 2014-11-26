#region Copyright (c) 2014 Orcomp development team.
// -------------------------------------------------------------------------------------------------------------------
// <copyright file="ConfigLocationService.cs" company="Orcomp development team">
//   Copyright (c) 2014 Orcomp development team. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------
#endregion

namespace Orc.GraphExplorer.Services
{
    using System.Configuration;
    using System.Linq;
    using Catel;
    using Catel.Configuration;
    using Catel.Services;
    using Models;

    internal class ConfigLocationService : IConfigLocationService
    {
        #region Fields
        private readonly IOpenFileService _openFileService;
        private readonly IConfigurationService _configurationService;
        #endregion

        #region Constructors
        public ConfigLocationService(IOpenFileService openFileService, IConfigurationService configurationService)
        {
            _openFileService = openFileService;
            _configurationService = configurationService;
        }
        #endregion

        #region IConfigLocationService Members
        public ConfigLocation Load()
        {
            var exeConfiguration = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);

            var section = (GraphExplorerSection) exeConfiguration.GetSection("graphExplorer");

            var config = section.CsvGraphDataServiceConfig;

            return new ConfigLocation()
            {
                RelationshipsFile = config.EdgesFilePath,
                PropertiesFile = config.VertexesFilePath,
                EnableProperty = config.EnableProperty
            };
        }

        public void Save(ConfigLocation configLocation)
        {
            Argument.IsNotNull(() => configLocation);

            var config = GraphExplorerSection.Current.CsvGraphDataServiceConfig;

            config.EdgesFilePath = configLocation.RelationshipsFile;

            config.VertexesFilePath = configLocation.PropertiesFile;

            config.EnableProperty = configLocation.EnableProperty ?? false;

            GraphExplorerSection.Current.Save();
        }

        public void ChangeRelationshipsFileLocation(ConfigLocation configLocation)
        {
            var rellationshipsFile = OpenRelationshipsFile();
            if (string.IsNullOrEmpty(rellationshipsFile))
            {
                return;
            }

            configLocation.RelationshipsFile = rellationshipsFile;
        }

        public void ChangePropertiesFileLocation(ConfigLocation configLocation)
        {
            var propertiesFile = OpenPropertiesFile();
            if (string.IsNullOrEmpty(propertiesFile))
            {
                return;
            }

            configLocation.PropertiesFile = propertiesFile;
        }
        #endregion

        #region Methods
        private string OpenRelationshipsFile()
        {
            return OpenFile("Select Relationship File");
        }

        private string OpenPropertiesFile()
        {
            return OpenFile("Select Properties File");
        }

        private string OpenFile(string title)
        {
            Argument.IsNotNullOrEmpty(() => title);

            _openFileService.IsMultiSelect = false;
            _openFileService.Filter = "All files|*.csv";
            _openFileService.Title = title;

            return _openFileService.DetermineFile() ? _openFileService.FileNames.Single() : null;
        }
        #endregion
    }
}