namespace Orc.GraphExplorer.Services
{
    using System.Configuration;

    using Catel;

    using Microsoft.Win32;
    using Orc.GraphExplorer.Models;

    class ConfigLocationService : IConfigLocationService
    {
        public ConfigLocationService()
        {
        }

        public ConfigLocation Load()
        {
            var exeConfiguration = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);

            var section = (GraphExplorerSection)exeConfiguration.GetSection("graphExplorer");

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

        private string OpenRelationshipsFile()
        {
            return OpenFile("Select Relationship File");
        }

        private string OpenPropertiesFile()
        {
            return OpenFile("Select Properties File");
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

        private string OpenFile(string title)
        {
            Argument.IsNotNullOrEmpty(() => title);

            var dlg = new OpenFileDialog { Filter = "All files|*.csv", Title = title };
            if (dlg.ShowDialog() == true)
            {
                return dlg.FileName;
            }

            return string.Empty;
        }
    }
}