namespace Orc.GraphExplorer.Services
{
    using System.Configuration;
    using Microsoft.Win32;
    using Orc.GraphExplorer.Models;
    using Orc.GraphExplorer.Services.Interfaces;

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

            return new ConfigLocation(this)
                   {
                       RelationshipsFile = config.EdgesFilePath, 
                       PropertiesFile = config.VertexesFilePath, 
                       EnableProperty = config.EnableProperty
                   };
        }

        public void Save(ConfigLocation configLocation)
        {
            var config = GraphExplorerSection.Current.CsvGraphDataServiceConfig;

            config.EdgesFilePath = configLocation.RelationshipsFile;

            config.VertexesFilePath = configLocation.PropertiesFile;

            config.EnableProperty = configLocation.EnableProperty ?? false;

            GraphExplorerSection.Current.Save();
        }

        public string OpenRelationshipsFile()
        {
            return OpenFile("Select Relationship File");
        }

        public string OpenPropertiesFile()
        {
            return OpenFile("Select Properties File");
        }

        private string OpenFile(string title)
        {
            var dlg = new OpenFileDialog { Filter = "All files|*.csv", Title = title };
            if (dlg.ShowDialog() == true)
            {
                return dlg.FileName;
            }

            return string.Empty;
        }
    }
}